using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiedBody : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(18f, 28f), Random.Range(22f, 37f)), ForceMode2D.Impulse);
        gameObject.AddComponent<LifeTime>().lifeTime = 10;
    }
    private void Update()
    {
        transform.localEulerAngles += new Vector3(0, 0, Time.deltaTime * 1000);
    }
}
