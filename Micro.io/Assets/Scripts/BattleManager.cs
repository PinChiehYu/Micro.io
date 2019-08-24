using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int P1_score = 0, P2_score = 0;
    public int maxTiming;
    private Text P1_score_text, P2_score_text, timer_text;
    private float timer;

    private bool endBattle;

    void Awake()
    {
        endBattle = false;
        timer = 0;
    }

    void Start()
    {
        P1_score_text = GameObject.Find("P1_Score").GetComponent<Text>();
        P2_score_text = GameObject.Find("P2_Score").GetComponent<Text>();
        timer_text = GameObject.Find("Timer").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f && !endBattle)
        {
            timer = 0f;
            EndBattle();
        }
        else
        {
            timer -= Time.deltaTime;
            timer_text.text = ((int)timer).ToString();
        }
    }

    private void EndBattle()
    {
        endBattle = true;

        if (P1_score > P2_score)
        {

        }
        if (P1_score < P2_score)
        {

        }
        if (P1_score == P2_score)
        {

        }
    }

    void ResetBattle()
    {
        timer = maxTiming;
        timer_text.text = ((int)timer).ToString();
        ScoreChange(1, -P1_score);
        ScoreChange(2, -P2_score);
    }

    void ScoreChange(int Player, int AddScore)
    {
        if (Player == 1)
        {
            P1_score += AddScore;
            P1_score_text.text = P1_score.ToString();
        }
        if (Player == 2)
        {
            P2_score += AddScore;
            P2_score_text.text = P2_score.ToString();
        }
    }
}
