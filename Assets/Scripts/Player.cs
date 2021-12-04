using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class Player : MonoBehaviour
{
    private static event Action<float> onChangeSpeed;
    public static void ChangeSpeed(float x) => onChangeSpeed?.Invoke(x);
    private GameManager manager;

    private Animator anim;

    [SerializeField] private Transform startingLine;

    private Vector3 positionVector;

    private int positionClamp = 1;

    private float energy = 25f;
    private float playerSpeed = 0.5f;
    private float _blend = 0f;
    private float newBlend = 0f;
    private float runningEndBlend = 1f;
    private float proningEndBlend = 0f;
    private float crawlingEndBlend = 0.5f;
    private float rotationY;
    private float screenX;

    private bool isLost;
    private void Start()
    {
        manager = GameManager._instance;
        anim = GetComponent<Animator>();
        positionVector = transform.position;
        transform.position = positionVector;
        screenX = Screen.width / 2f;
    }
    void Update()
    {
        if (isLost) return;

        if (Input.GetMouseButtonDown(0) && !manager.isGameStarted && !manager.shouldHorizontalMove)
        {
            manager.isGameStarted = true;
            manager.shouldHorizontalMove = true;
        }

        if (manager.isGameStarted && manager.shouldHorizontalMove)
        {
            Debug.Log(positionVector);
            positionVector.z += playerSpeed * Time.deltaTime;
            transform.position = Vector3.Slerp(transform.position, positionVector, 0.5f);
        }

        if (Input.GetMouseButton(0) && manager.isGameStarted && manager.shouldHorizontalMove)
        {
            if (Input.mousePosition.x > screenX)
            {
                positionVector.x = (Input.mousePosition.x - screenX) / screenX;
            }
            else
            {
                positionVector.x = (screenX - Input.mousePosition.x) / -screenX;
            }

            rotationY = Input.GetAxis("Mouse X");
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, rotationY * 200f, 0f), 0.05f);
        }

        if (energy > 50 && energy < 100)
        {
            _blend = newBlend;
            newBlend = Mathf.Lerp(_blend, runningEndBlend, Time.deltaTime);
            anim.SetFloat("Blend", _blend);
        }

        if (energy > 25 && energy < 50)
        {
            _blend = newBlend;
            newBlend = Mathf.Lerp(_blend, crawlingEndBlend, Time.deltaTime);
            anim.SetFloat("Blend", _blend);
        }

        if (energy > 0 && energy < 25)
        {
            _blend = newBlend;
            newBlend = Mathf.Lerp(_blend, proningEndBlend, Time.deltaTime);
            anim.SetFloat("Blend", _blend);
        }

        if(energy == 0)
        {
            GameManager.ChangeState(GameStates.Lose);
        }

        if (playerSpeed > 2f)
            playerSpeed = 2f;

        else if (playerSpeed < 0f)
            playerSpeed = 0f;

        Exhaust();

        positionVector.x = Mathf.Clamp(positionVector.x, -positionClamp, positionClamp);
    }

    private void ChangePlayerSpeed(float speed)
    {
        this.playerSpeed += speed;
    }

    public void IncreaseEnergy(float _energy)
    {
        float increasedEnergy = energy + _energy;
        if (increasedEnergy > 100f)
            increasedEnergy = 100f;
        DOTween.To(() => energy, (increasedEnergy) => energy = increasedEnergy, increasedEnergy, 1f);
    }

    public void DecreaseEnergy(float _energy)
    {
        float decreasedEnergy = energy - _energy;
        if (decreasedEnergy < 0f)
            decreasedEnergy = 0f;
        DOTween.To(() => energy, (decreasedEnergy) => energy = decreasedEnergy, decreasedEnergy, 1f);
    }

    public void SetEnergy(float _energy)
    {
        this.energy = _energy;
    }

    public float GetEnergy()
    {
        return energy;
    }

    private void Exhaust()
    {
        if (manager.shouldHorizontalMove && manager.isGameStarted)
            energy -= 1 * Time.deltaTime;

        if (energy < 0)
            energy = 0;
        manager._ui.UpdateEnergyBar(GetEnergy());
    }

    public void PlayerReset()
    {
        transform.position = startingLine.position;
        energy = 25f;
        playerSpeed = 0.5f;
        _blend = 0f;
        manager.isGameStarted = false;
        manager.shouldHorizontalMove = false;
    }


    private void StateHandler(GameStates a)
    {
       if(a.Equals(GameStates.Lose))
        {
            isLost = true;
        }
    }

    private void OnEnable()
    {
        GameManager.onStateChanged += StateHandler;
        onChangeSpeed += ChangePlayerSpeed;
    }

    private void OnDisable()
    {
        GameManager.onStateChanged -= StateHandler;
        onChangeSpeed -= ChangePlayerSpeed;
    }
}

