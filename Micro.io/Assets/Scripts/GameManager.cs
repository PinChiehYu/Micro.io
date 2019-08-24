using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static GameManager _instance;
    private static readonly object _lock = new object();
    public static GameManager Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }
    }

    public int player1Char, player2Char;
    public int Winner { get; private set; }

    private GameManager()
    {
        ResetAllRecord();
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void OnSceneChange(Scene from, Scene to)
    {
        if (to.name == "Title" || to.name == "Choose")
        {
            ResetAllRecord();
        }
        else if (to.name == "Battle")
        {
            Debug.Log(player1Char.ToString() + ":" + player2Char.ToString());
            if (player1Char == -1 || player2Char == -1)
            {
                Debug.LogError("Character not determin!");
            }
        }
        else if (to.name == "Result")
        {
            if (Winner == 0)
            {
                Debug.LogError("Winner not determin!");
            }
        }
    }

    private void ResetAllRecord()
    {
        player1Char = player2Char = -1;
        Winner = 0;
    }
}