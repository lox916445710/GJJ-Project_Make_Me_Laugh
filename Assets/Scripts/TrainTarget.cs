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
            //Debug.Log("����һϵ����ײʱ�ķ���");

            //��Ч
            VfxManager.instance.CreateVfx(manager.crashingVfxids[Random.Range(0, manager.crashingVfxids.Count)], transform.position +
                new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), new Vector3(3, 3, 3));

            //����
            CameraManager.instance.impulseController.CreateImpulse(0);

            //�÷�
            ScoreManager.instance.CrackOneTarget(transform.position);

            //ͳ�ƴ���
            totalGameManager.instance.allCrashedAmounts++;

            //����ʬ��
            manager.CreateDiedBody(transform.position);

            //��Ч
            Music_Sound_Manager.instance.CreateCrackSound();

            //�𳵵�ײ���¼�
            totalGameManager.instance.train.OnCrack();



            Destroy(gameObject);

        }
    }
}
