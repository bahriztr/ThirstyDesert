using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    private GameManager manager;

    [SerializeField] private Image energyBar;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject levelFinishedMenu;
    [SerializeField] private List<Canvasses> _canvasses;

    private void Start()
    {
        manager = GameManager._instance;
    }

    public void UpdateEnergyBar(float energy)
    {
        energyBar.fillAmount = energy / 100f;
    }
    public void OnClickPauseButton()
    {
        GameManager.ChangeState(GameStates.Pause);
    }

    public void OnClickResumeButton()
    {
        Time.timeScale = 1f;
    }

    public void OnClickNextButton()
    {
        manager._player.PlayerReset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

        public void OnClickRetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        manager._player.PlayerReset();
    }

    private void StateHandler(GameStates a)
    {
        foreach (var canvas in _canvasses)
        {
            canvas._canvas.SetActive(canvas._state.Equals(a));
        }

        if (a == GameStates.Win)
        {
            Time.timeScale = 0f;
        }

        else if (a == GameStates.Lose)
        {
            Time.timeScale = 0f;
        }
        else if (manager.isGamePaused && a == GameStates.Pause)
        {
            Time.timeScale = 0f;
        }
        else if (a == GameStates.Resume)
        {
            OnClickResumeButton();
        }
        
    }
    private void OnEnable()
    {
        GameManager.onStateChanged += StateHandler;
    }
    private void OnDisable()
    {
        GameManager.onStateChanged -= StateHandler;
    }


}

[Serializable]
public class Canvasses
{
    public GameStates _state;
    public GameObject _canvas;
}

