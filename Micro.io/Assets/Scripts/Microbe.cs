using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Microbe : MonoBehaviour
{
    public new Camera camera;
    private Transform mask;

    public ControlSetting controlSetting;
    public MicrobeSetting microbeSetting;

    private Transform aim;

    private Vector3 direction;
    private float fireCooldown;

    // Start is called before the first frame update
    void Start()
    {
        aim = transform.GetChild(0);
        mask = camera.transform.GetChild(0);
    }

    // Update is called once per frame
    private void Update()
    {
        GetDirectionInput();
        UpdateBodyRotation();
        Debug.DrawRay(transform.position, transform.up);

        UpdateCamera();

        UpdateCooldown();
        GetFireInput();
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * microbeSetting.MoveSpeed * Time.fixedDeltaTime);
    }

    private void GetDirectionInput()
    {
        direction = Vector3.zero;
        if (Input.GetKey(controlSetting.Up))
        {
            direction.y += 1;
        }
        if (Input.GetKey(controlSetting.Down))
        {
            direction.y -= 1;
        }
        if (Input.GetKey(controlSetting.Right))
        {
            direction.x += 1;
        }
        if (Input.GetKey(controlSetting.Left))
        {
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
        if (fireCooldown < microbeSetting.FireCooldown)
        {
            fireCooldown += Time.deltaTime;
        }
    }

    private void GetFireInput()
    {
        if (Input.GetKey(controlSetting.Fire) && fireCooldown >= microbeSetting.FireCooldown)
        {
            Fire();
            fireCooldown = 0;
        }
    }

    private void Fire()
    {
        Instantiate(microbeSetting.Projectile, transform.position, transform.rotation);
    }
}
