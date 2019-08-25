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

    public int[] PlayerChar;
    public int Winner { get; private set; }

    private GameManager()
    {
        PlayerChar = new int[2];
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
            foreach (int charId in PlayerChar)
            {
                if (charId < 0)
                {
                    Debug.LogError("Someone didn't choose Character!");
                }
            }
        }
        else if (to.name == "Result")
        {
            if (Winner < 0)
            {
                Debug.LogError("Winner not determin!");
            }
        }
    }

    private void ResetAllRecord()
    {
        for (int i = 0; i < PlayerChar.Length; i++)
        {
            PlayerChar[i] = -1;
        }
        Winner = -1;
    }

    public void ChooseCharacter(int id, int charID)
    {
        PlayerChar[id] = charID;
    }

    public void RecordWinner(int[] PlayerScores)
    {
        int maxScore = int.MinValue;
        for (int id = 0; id < PlayerScores.Length; id++)
        {
            if (PlayerScores[id] > maxScore)
            {
                maxScore = PlayerScores[id];
                Winner = id;
            }
        }

        //Debug.Log("Winner ID:" + Winner.ToString() + PlayerScores[0].ToString() + ", " + PlayerScores[1].ToString()); 
    }
}