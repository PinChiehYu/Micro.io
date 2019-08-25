using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Microbe : MonoBehaviour
{
    private int id;

    public new Camera camera;

    public ControlSetting controlSetting;
    public MicrobeSetting microbeSetting;

    private Transform aim, lightSpot;
    private Animator animator;

    private Dictionary<EffectType, Effect> effectList;

    private Vector3 direction;
    private float fireCooldown;

    private float arenaRange;

    private bool pause;
    
    private void Awake()
    {
        pause = true;
        effectList = new Dictionary<EffectType, Effect>();
    }

    private void Start()
    {
        aim = transform.GetChild(0);
        arenaRange = GameObject.Find("BattleManager").GetComponent<BattleManager>().arenaSize;
        animator = gameObject.GetComponent<Animator>();
    }

    public void Initialize(int id, ControlSetting controlSetting, MicrobeSetting microbeSetting)
    {
        this.id = id;
        this.controlSetting = controlSetting;
        this.microbeSetting = microbeSetting;

        camera = GameObject.Find("CameraP" + id.ToString()).GetComponent<Camera>();
        lightSpot = camera.transform.GetChild(1);
        lightSpot.localPosition = new Vector3(0, 10, 10);

        pause = false;
    }

    private void Update()
    {
        if (pause) return;

        GetDirectionInput();
        UpdateBodyRotation();
        Debug.DrawRay(transform.position, transform.up);

        UpdateCamera();

        UpdateCooldown();
        GetFireInput();

        UpdateEffects();
    }

    private void FixedUpdate()
    {
        if (pause) return;

        float coef = 1f;
        if (GetEffect(EffectType.MoveSpeed, out bool positive, out float rate))
        {
            coef = positive ? 1f + rate : 1f - rate;
        }

        Vector3 oldPos = transform.position;
        transform.Translate(Vector3.up * microbeSetting.MoveSpeed * coef * Time.fixedDeltaTime);
        if (transform.position.magnitude >= arenaRange) transform.position = oldPos;
    }

    private void GetDirectionInput()
    {
        if (Input.GetKey(controlSetting.Up))
        {
            lightSpot.localPosition = new Vector3(0, 10, 10);
            direction.x = 0;
            direction.y += 1;
        }
        if (Input.GetKey(controlSetting.Down))
        {
            lightSpot.localPosition = new Vector3(0, -10, 10);
            direction.x = 0;
            direction.y -= 1;
        }
        if (Input.GetKey(controlSetting.Right))
        {
            lightSpot.localPosition = new Vector3(10, 0, 10);
            direction.y = 0;
            direction.x += 1;
        }
        if (Input.GetKey(controlSetting.Left))
        {
            lightSpot.localPosition = new Vector3(-10, 0, 10);
            direction.y = 0;
            direction.x -= 1;
        }

        direction.Normalize();
    }

    private void UpdateBodyRotation()
    {
        float targetangle = Vector3.SignedAngle(transform.up, direction, Vector3.forward);
        transform.Rotate(new Vector3(0, 0, Mathf.Sign(targetangle) * Mathf.Min(microbeSetting.RotateSpeed, Mathf.Abs(targetangle))));
    }

    private void UpdateCamera()
    {
        camera.transform.position = new Vector3(aim.position.x, aim.position.y, -10);
    }

    private void UpdateCooldown()
    {
        if (fireCooldown < microbeSetting.ReloadSpeed)
        {
            float coef = 1f;
            if (!GetEffect(EffectType.ReloadSpeed, out bool positive, out float rate))
            {
                coef = positive ? 1f + rate : 1f - rate;
            }

            fireCooldown += Time.deltaTime * coef;
        }
    }

    private void GetFireInput()
    {
        if (Input.GetKey(controlSetting.Fire) && fireCooldown >= microbeSetting.ReloadSpeed)
        {
            Fire();
            fireCooldown = 0;
        }
    }

    private void Fire()
    {
        animator.Play("Fire");
        GameObject projectile = Instantiate(microbeSetting.Projectile, transform.position + Vector3.forward, transform.rotation);
        projectile.GetComponent<Projectile>().Owner = id;

        float coef = 1f;
        if (GetEffect(EffectType.ShootDistance, out bool positive, out float rate))
        {
            coef = positive ? 1f + rate : 1f - rate;
        }

        projectile.GetComponent<Projectile>().Distance *= coef;
    }

    public void ReceiveEffects(Effect effect)
    {
        Debug.Log(effect.Type.ToString() + ":" + effect.IsPositive.ToString()  + ":" + effect.Amount.ToString());

        if (effect.Type == EffectType.Score)
        {
            int point = effect.IsPositive ? (int)effect.Amount : (int)(-effect.Amount);
            GameObject.Find("BattleManager").GetComponent<BattleManager>().AddPoint(id, point);
        }
        else
        {
            effectList[effect.Type] = effect;
        }
    }

    private void UpdateEffects()
    {
        List<EffectType> keys = new List<EffectType>(effectList.Keys);

        foreach (EffectType key in keys)
        {
            if (effectList[key].RemainTiming <= 0f)
            {
                effectList.Remove(key);
            }
            else
            {
                effectList[key].RemainTiming -= Time.deltaTime;
            }
        }
    }

    private bool GetEffect(EffectType type, out bool isPositive, out float amount)
    {
        if (effectList.ContainsKey(type))
        {
            amount = effectList[type].Amount;
            isPositive = effectList[type].IsPositive;
            return true;
        }
        else
        {
            isPositive = true;
            amount = 0;
            return false;
        }
    }
}