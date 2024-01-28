using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTextFrame : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-4f, 4f), Random.Range(5f, 10f)), ForceMode2D.Impulse);
    }
}
