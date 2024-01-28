using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 和背景一起移动但是不会回归
/// </summary>
public class MoveAsBG : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.position -= Vector3.right * (totalGameManager.instance.bgScrollSpeed + totalGameManager.instance.globalScrollSppedCorrection) * Time.deltaTime;
       
    }
}
