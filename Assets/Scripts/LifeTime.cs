using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    public float lifeTime;
    private void Start()
    {
        if (lifeTime == 0)
            lifeTime = 7.5f;
    }
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
            Die();
    }
    public void Die()
    {
        //Debug.Log("ËÀÁË");
        Destroy(gameObject);
    }
    
}
