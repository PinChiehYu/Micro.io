using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MicrobeSetting : ScriptableObject
{
    public float MoveSpeed;
    public float RotateSpeed;

    public GameObject Projectile;
    public float FireCooldown;

}
