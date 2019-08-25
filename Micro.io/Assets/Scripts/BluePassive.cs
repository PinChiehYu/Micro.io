using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePassive : MonoBehaviour
{
    [SerializeField]
    private PropSetting propSetting;
    [SerializeField]
    private int spawnRangeMin;
    [SerializeField]
    private int spawnRangeMax;


    private int arenaSize;

    [SerializeField]
    private float triggerSpawningPeriod = 3f;
    private float propTimer = 0f;

    private void Start()
    {
        arenaSize = GameObject.Find("BattleManager").GetComponent<BattleManager>().arenaSize;
    }

    private void Update()
    {
        propTimer += Time.deltaTime;
        if (propTimer > triggerSpawningPeriod)
        {
            propTimer = 0f;
            InstantiateProp();
        }
    }

    private void InstantiateProp()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y, 0);
        Vector3 vector = Vector3.zero;
        do
        {
            vector.x = Random.Range(-spawnRangeMax, spawnRangeMax);
            vector.y = Random.Range(-spawnRangeMax, spawnRangeMax);
        }
        while (vector.magnitude > spawnRangeMax || vector.magnitude < spawnRangeMin || (position + vector).magnitude > arenaSize - 5);

        int type = Random.Range(0, 4);
        Effect effect = new Effect
        {
            Type = (EffectType)type,
            IsPositive = true,
            Amount = Random.Range(propSetting.EffectRanges[type].Min, propSetting.EffectRanges[type].Max),
            RemainTiming = Random.Range(3f, 11f)
        };

        GameObject prop = Instantiate(propSetting.Prefab, position + vector, Quaternion.identity);
        int iconIndex = type * 2;
        prop.GetComponent<Prop>().BuildProp(effect, propSetting.PropAnimations[iconIndex]);
    }
}
