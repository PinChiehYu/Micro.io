using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Update is called once per frame
    void Start()
    {
        StartCoroutine(Countdown());
    }

    void Update()
    {
        transform.Translate(Vector3.up);
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
