using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    GameObject UI;
    Animator p1Anim, p2Anim;
    int winner;
    [SerializeField]
    float timer = 3f;
    // Start is called before the first frame update
    void Start()
    {
        winner = GameManager.Instance.Winner;
        p1Anim = GameObject.Find("P1").GetComponent<Animator>();
        p2Anim = GameObject.Find("P2").GetComponent<Animator>();
        UI = GameObject.Find("Canvas");
        UI.SetActive(false);

        Vector3 tmp = p2Anim.transform.localScale;
        p2Anim.transform.localScale = new Vector3(-tmp.x, tmp.y, tmp.z);
        if (winner == 1)
        {
            p1Anim.SetFloat("result", 0);
            p2Anim.SetFloat("result", 1);
        }
        if (winner == 2)
        {
            p1Anim.SetFloat("result", 0);
            p2Anim.SetFloat("result", 1);
        }
        if (winner == 3)
        {
            p1Anim.SetFloat("result", 2);
            p2Anim.SetFloat("result", 2);
        }
    }

    IEnumerator Countdown(float timer)
    {
        yield return new WaitForSeconds(timer);
        UI.SetActive(true);
    }
}
