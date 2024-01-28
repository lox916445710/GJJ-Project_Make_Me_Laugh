using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseController : MonoBehaviour
{
    public List<CinemachineImpulseSource> impulseSources;

    public void CreateImpulse(int index)
    {
        impulseSources[index].GenerateImpulse();
    }
}
