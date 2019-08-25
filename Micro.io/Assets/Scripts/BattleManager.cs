using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    private const int playerNum = 2;
    public int arenaSize;

    [SerializeField]
    private int maxTiming = 90;

    [SerializeField]
    private List<GameObject> characterList;

    [SerializeField]
    private List<ControlSetting> controlSettingList;
    [SerializeField]
    private List<MicrobeSetting> microbeSettingList;
    [SerializeField]
    private PropSetting propSetting;

    private int[] playerScores;
    private float countdownTimer;
    private float propTimer;

    private bool endBattle;

    private Text[] scoreTexts;
    private Text timerText;

    void Awake()
    {
        Random.InitState(System.Guid.NewGuid().GetHashCode());
        playerScores = new int[playerNum];
        scoreTexts = new Text[playerNum];
        endBattle = false;
        countdownTimer = maxTiming;
        propTimer = 0;
    }

    void Start()
    {
        InstantiateCharacters();

        for (int id = 0; id < playerNum; id++)
        {
            scoreTexts[id] = GameObject.Find("Score_P" + id.ToString()).GetComponent<Text>();
        }

        timerText = GameObject.Find("Timer").GetComponent<Text>();
    }

    private void InstantiateCharacters()
    {
        for (int id = 0; id < playerNum; id++)
        {
            int charId = GameManager.Instance.PlayerChar[id];
            Microbe microbe = Instantiate(characterList[charId], new Vector3(5 * (id * 2 - 1), 0, 0), Quaternion.identity).GetComponent<Microbe>();
            microbe.Initialize(id, controlSettingList[id], microbeSettingList[charId]);
            microbe.name = id.ToString() + charId.ToString();
        }
    }

    private void Update()
    {
        UpdateUI();
        CreateProp();
        UpdateTimer();
    }

    private void UpdateUI()
    {
        for (int id = 0; id < playerNum; id++)
        {
            scoreTexts[id].text = playerScores[id].ToString();
        }
    }

    private void CreateProp()
    {
        propTimer += Time.deltaTime;
        float poss = countdownTimer > (maxTiming / 2) ? 0.5f : 0.9f;
        if (propTimer > 0.5f && poss <= Random.Range(0f, 1f))
        {
            propTimer = 0f;
            InstantiateProp();
        }
    }

    private void InstantiateProp()
    {
        Vector3 position = Vector3.zero;
        float bind = arenaSize - 5; // 5 is for margin
        do
        {
            position.x = Random.Range(-bind, bind);
            position.y = Random.Range(-bind, bind);
        }
        while (position.magnitude > bind);
        position.z = 1f;

        int type = Random.Range(0, 4);
        int positive = Random.Range(0, 2);
        Effect effect = new Effect
        {
            Type = (EffectType)type,
            IsPositive = positive < 1,
            Amount = Random.Range(propSetting.EffectRanges[type].Min, propSetting.EffectRanges[type].Max),
            RemainTiming = Random.Range(3f, 11f)
        };

        GameObject prop = Instantiate(propSetting.Prefab, position, Quaternion.identity);
        int iconIndex = type * 2 + positive;
        prop.GetComponent<Prop>().BuildProp(effect, propSetting.PropAnimations[iconIndex]);
    }

    private void UpdateTimer()
    {
        if (countdownTimer <= 0f && !endBattle)
        {
            countdownTimer = 0f;
            EndBattle();
        }
        else
        {
            countdownTimer -= Time.deltaTime;
            timerText.text = ((int)countdownTimer).ToString();
        }
    }

    private void EndBattle()
    {
        endBattle = true;

        GameManager.Instance.RecordWinner(playerScores);
        SceneManager.LoadScene("Result");
    }

    private void ResetBattle()
    {
        countdownTimer = maxTiming;
        timerText.text = maxTiming.ToString();
        ResetScore();
    }

    private void ResetScore()
    {
        for (int id = 0; id < playerNum; id++)
        {
            playerScores[id] = 0;
            scoreTexts[id].text = "0";
        }
    }

    public void ProjectileHit(int owner, int target)
    {
        playerScores[owner] += 100;
        playerScores[target] -= 50;
    }

    public void AddPoint(int player, int point)
    {
        playerScores[player] += point;
    }
}
