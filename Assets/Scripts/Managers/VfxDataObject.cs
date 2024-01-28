using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "特效信息数据", menuName = "ScriptableObject/特效信息数据", order = 0)]
public class VfxDataObject : ScriptableObject
{
    public List<VfxData> datas;
}
