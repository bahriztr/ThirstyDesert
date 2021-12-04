using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GoodItem : MonoBehaviour
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

        Player.ChangeSpeed(0.5f);
        manager._player.IncreaseEnergy(10f);
        
        if (manager._player.GetEnergy() > 100f)
            manager._player.SetEnergy(100f);

        manager._ui.UpdateEnergyBar(manager._player.GetEnergy());
    }
}
