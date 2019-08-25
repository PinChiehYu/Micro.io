using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Owner = -1;
    public float Distance = 1f;

    [SerializeField]
    private Sprite bang;
    private bool hit = false;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(Countdown());
    }

    void Update()
    {
        if (!hit)
        {
            transform.Translate(Vector3.up);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Microbe"))
        {
            return;
        }

        int target = Int32.Parse(col.name);
        if (target != Owner)
        {
            GameObject.Find("BattleManager").GetComponent<BattleManager>().ProjectileHit(Owner, target);
            StartCoroutine(HitEffect(transform.position));
        }
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(Distance);
        Destroy(gameObject);
    }

    private IEnumerator HitEffect(Vector3 hitPoint)
    {
        hit = true;
        GetComponent<SpriteRenderer>().sprite = bang;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
