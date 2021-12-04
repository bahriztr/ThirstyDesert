using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private GameManager manager;

    private void Start()
    {
        manager = GameManager._instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        GameManager.ChangeState(GameStates.Win);
    }
    
}
