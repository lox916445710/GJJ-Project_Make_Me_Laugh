using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateShadow : MonoBehaviour
{
    public GameObject shadowPrefab;
    public float scale = 2;
    public float rate = 0.1f;
    public float timer;
    private void Start()
    {
        timer = rate;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = rate;
            GameObject go = Instantiate(shadowPrefab);
            go.transform.position = transform.position;
            go.transform.localScale = transform.localScale * scale;
            go.transform.localEulerAngles = transform.localEulerAngles;
            go.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
            go.GetComponent<SpriteRenderer>().sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
            go.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
            go.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.35f);
            //go.AddComponent<Animator>().runtimeAnimatorController = totalGameManager.instance.shadowAnimator;
            //go.AddComponent<MoveAsBG>();
            //go.AddComponent<LifeTime>().lifeTime = 1;
        }
    }

}
