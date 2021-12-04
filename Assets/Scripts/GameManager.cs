using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates
{
    Pause,
    Resume,
    Win,
    Lose
}

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    public UIManager _ui;
    public Player _player;
    public bool shouldHorizontalMove;
    public bool isGameStarted;
    public bool isGamePaused = false;


    public static event Action<GameStates> onStateChanged;
    public static void ChangeState(GameStates a) => onStateChanged?.Invoke(a);

    public GameStates _states;
        private void Awake()
    {
        SetInstance();
    }

    private void SetInstance()
    {
        _instance = this;
        if (_instance == null)
        {
            SetInstance();
        }
    }
}
