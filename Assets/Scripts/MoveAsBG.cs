using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ͱ���һ���ƶ����ǲ���ع�
/// </summary>
public class MoveAsBG : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.position -= Vector3.right * (totalGameManager.instance.bgScrollSpeed + totalGameManager.instance.globalScrollSppedCorrection) * Time.deltaTime;
       
    }
}
