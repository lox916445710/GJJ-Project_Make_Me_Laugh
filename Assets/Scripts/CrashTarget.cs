using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashTarget : MonoBehaviour
{
    public float speed;

    private void Start()
    {
        speed += Random.Range(-2, 2);
    }
    private void Update()
    {
        transform.position += Vector3.left * (speed + totalGameManager.instance.globalScrollSppedCorrection) * Time.deltaTime;

    }
}
