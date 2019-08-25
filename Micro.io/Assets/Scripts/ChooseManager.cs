using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseManager : MonoBehaviour
{
    public ControlSetting[] controlSetting;

    private const int charNumber = 4;

    private Vector3[] charPivot;
    private Transform[] selectionFrame;
    private int[] selectionIndex;
    private bool[] isReady;
    private Animator[] animators;
    private AudioSource[] audioSources;

    [SerializeField]
    private AudioClip switchClip;
    [SerializeField]
    private AudioClip lockClip;
    [SerializeField]
    private AudioClip unlockClip;

    private bool isWaiting;
    private bool isFinish;
    private float countdown;

    [SerializeField]
    private Text timer;

    void Awake()
    {
        timer.transform.parent.localScale = Vector3.zero;
        charPivot = new Vector3[charNumber];
        selectionIndex = new int[2];
        selectionFrame = new Transform[2];
        animators = new Animator[2];
        isReady = new bool[2];
        audioSources = new AudioSource[2];

        isWaiting = false;
        isFinish = false;
    }

    private void Start()
    {
        for (int i = 0; i < charNumber; i++)
        {
            charPivot[i] = GameObject.Find("Characs").transform.GetChild(i).transform.position;
        }

        for (int id = 0; id < 2; id++)
        {
            selectionFrame[id] = GameObject.Find("SelectionP" + id.ToString()).transform;
            animators[id] = selectionFrame[id].GetComponent<Animator>();
            audioSources[id] = selectionFrame[id].GetComponent<AudioSource>();
            isReady[id] = false;

            ChangeCharacter(id, 0);
        }
    }

    private void Update()
    {
        if (isFinish) return;

        for (int id = 0; id < 2; id++)
        {
            GetPlayerInput(id);
        }

        CheckBothPlayerReady();
    }

    private void GetPlayerInput(int id)
    {
        if (!isReady[id] && Input.GetKeyDown(controlSetting[id].Left))
        {
            ChangeCharacter(id, -1);
            audioSources[id].PlayOneShot(switchClip);
        }
        else if (!isReady[id] && Input.GetKeyDown(controlSetting[id].Right))
        {
            ChangeCharacter(id, 1);
            audioSources[id].PlayOneShot(switchClip);
        }
        else if (Input.GetKeyDown(controlSetting[id].Fire))
        {
            isReady[id] = !isReady[id];
            if (isReady[id])
            {
                animators[id].Play("lock");
                audioSources[id].PlayOneShot(lockClip);
            }
            else
            {
                animators[id].Play("select");
                audioSources[id].PlayOneShot(unlockClip);
                isWaiting = false;
                timer.transform.parent.localScale = Vector3.zero;
            }
        }
    }

    private void ChangeCharacter(int id, int dir)
    {

        selectionIndex[id] = (selectionIndex[id] + dir + charNumber) % charNumber;
        selectionFrame[id].position = charPivot[selectionIndex[id]];
    }

    private void CheckBothPlayerReady()
    {
        if (isReady[0] && isReady[1])
        {
            if (!isWaiting)
            {
                countdown = 4f;
                isWaiting = true;
                timer.transform.parent.localScale = Vector3.one;
            }
            else
            {
                if (countdown <= 0f)
                {
                    FinishSelect();
                }
                else
                {
                    countdown -= Time.deltaTime;
                    timer.text = ((int)countdown).ToString();
                }
            }
        }
    }

    private void FinishSelect()
    {
        isFinish = true;
        for (int id = 0; id < 2; id++)
        {
            GameManager.Instance.ChooseCharacter(id, selectionIndex[id]);
        }

        SceneManager.LoadScene("Battle");
    }
}