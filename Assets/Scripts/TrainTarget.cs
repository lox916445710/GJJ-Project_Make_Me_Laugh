using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTarget : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            totalGameManager manager = totalGameManager.instance;
            //Debug.Log("触发一系列碰撞时的方法");

            //特效
            VfxManager.instance.CreateVfx(manager.crashingVfxids[Random.Range(0, manager.crashingVfxids.Count)], transform.position +
                new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), new Vector3(3, 3, 3));

            //震屏
            CameraManager.instance.impulseController.CreateImpulse(0);

            //得分
            ScoreManager.instance.CrackOneTarget(transform.position);

            //统计次数
            totalGameManager.instance.allCrashedAmounts++;

            //创建尸体
            manager.CreateDiedBody(transform.position);

            //音效
            Music_Sound_Manager.instance.CreateCrackSound();

            //火车的撞击事件
            totalGameManager.instance.train.OnCrack();



            Destroy(gameObject);

        }
    }
}
