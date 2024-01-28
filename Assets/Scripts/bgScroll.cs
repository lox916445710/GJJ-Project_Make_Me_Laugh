using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgScroll : MonoBehaviour
{
    public Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }


    void FixedUpdate()
    {
        transform.position -= Vector3.right * (totalGameManager.instance.bgScrollSpeed + totalGameManager.instance.globalScrollSppedCorrection) * Time.deltaTime;
        if (Vector3.Distance(transform.position, startPos) >= totalGameManager.instance.onePartDistance)
        {
            transform.position = startPos;
        }
    }
}
