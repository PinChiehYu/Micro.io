using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private GameObject UI;
    private Animator p1Anim, p2Anim;
    private int winner;

    [SerializeField]
    float timer = 3f;

    private void Start()
    {
        winner = GameManager.Instance.Winner;
        p1Anim = GameObject.Find("p1").GetComponent<Animator>();
        p2Anim = GameObject.Find("p2").GetComponent<Animator>();
        UI = GameObject.Find("Canvas");
        UI.SetActive(false);

        Vector3 tmp = p1Anim.transform.localScale;
        p1Anim.transform.localScale = new Vector3(-tmp.x, tmp.y, tmp.z);

        //play anim depending character
        p1Anim.Play(GameManager.Instance.PlayerChar[0].ToString());
        p2Anim.Play(GameManager.Instance.PlayerChar[1].ToString());

        if (winner == 2)
        {
            p1Anim.SetFloat("result", 2);
            p2Anim.SetFloat("result", 2);
        }

        if (winner == 0)
        {
            p1Anim.SetFloat("result", 0);
            p2Anim.SetFloat("result", 1);
        }
        if (winner == 1)
        {
            p1Anim.SetFloat("result", 1);
            p2Anim.SetFloat("result", 0);
        }
        
        StartCoroutine(Countdown(timer));
    }

    IEnumerator Countdown(float timer)
    {
        yield return new WaitForSeconds(timer);
        UI.SetActive(true);
        if (winner == 2)
        {
            GameObject.Find("Win").transform.localScale = Vector3.zero;
        }
        else
        {
            GameObject.Find("Draw").transform.localScale = Vector3.zero;
        }
    }
}
