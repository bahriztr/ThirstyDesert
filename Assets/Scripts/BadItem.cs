using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BadItem : MonoBehaviour
{
    private GameManager manager;
    private void Start()
    {
        manager = GameManager._instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        gameObject.transform.DOScale(0f, 0.8f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });

        Player.ChangeSpeed(-0.5f);
        manager._player.DecreaseEnergy(5f);

        if (manager._player.GetEnergy() < 0f)
            manager._player.SetEnergy(0f);

        manager._ui.UpdateEnergyBar(manager._player.GetEnergy());
    }
}
