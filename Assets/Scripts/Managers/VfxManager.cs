using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����ɲ���ɣ�������Ч��������Ҫдע�Ͱɣ���
/// </summary>
public class VfxManager : MonoBehaviour
{
    public Material defaultMaterial;
    public Material objectChoosedMaterial;
    public Material uiGlowMaterial;


    [Header("��Ч")]
    public GameObject driftWind;



    public VfxDataObject vfxData;
    public static VfxManager instance;

    public int createAmount;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public GameObject GetVfxByIdOrName(string id_or_name)
    {
        if (id_or_name == "0")
            return null;
        createAmount++;
        GameObject go;
        if (vfxData.datas.Find((VfxData vd) => vd.id == id_or_name) != null)
            go = vfxData.datas.Find((VfxData vd) => vd.id == id_or_name).vfx;
        else
            go = vfxData.datas.Find((VfxData vd) => vd.name == id_or_name).vfx;

        return go;
    }

    /// <summary>
    /// ����ô�򵥵ķ�����Ҫ��ע�ͣ����������ء�
    /// </summary>
    /// <param name="id_or_name"></param>
    /// <param name="position"></param>
    public GameObject CreateVfx(string id_or_name, Vector3 position)
    {
        if (id_or_name == "0")
            return null;
        GameObject go = Instantiate(GetVfxByIdOrName(id_or_name));
        go.transform.position = position;
        if (!go.GetComponent<LifeTime>())
            go.AddComponent<LifeTime>();
        //Debug.Log(go.name);
        //go.name = "VFX��" + createAmount;
        return go;
    }
    /// <summary>
    /// ����ô�򵥵ķ�����Ҫ��ע�ͣ����������ء�
    /// </summary>
    /// <param name="id_or_name"></param>
    /// <param name="position"></param>
    public GameObject CreateVfx(string id_or_name, Transform parent)
    {

        GameObject go = Instantiate(GetVfxByIdOrName(id_or_name), parent);

        //if (!go.GetComponent<LifeTime>())
        //    go.AddComponent<LifeTime>();

        go.name = "VFX��" + createAmount;

        return go;
        //Debug.Log(go.name);
    }
    /// <summary>
    /// ����ô�򵥵ķ�����Ҫ��ע�ͣ����������ء���
    /// </summary>
    public GameObject CreateVfx(string id_or_name, Vector3 position, Vector3 scale)
    {
        if (id_or_name == "0")
            return null;
        GameObject go = Instantiate(GetVfxByIdOrName(id_or_name));
        go.transform.position = position;
        go.transform.localScale = scale;
        if (!go.GetComponent<LifeTime>())
            go.AddComponent<LifeTime>();
        return go;
    }
    /// <summary>
    /// ����ô�򵥵ķ�����Ҫ��ע�ͣ����������ء���
    /// </summary>
    public void CreateVfx(string id_or_name, Vector3 position, Vector3 scale, Quaternion rotation)
    {
        if (id_or_name == "0")
            return;
        GameObject go = Instantiate(GetVfxByIdOrName(id_or_name));
        go.name = "VFX��" + createAmount;
        Debug.Log(go.name);
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.localScale = scale;
        if (!go.GetComponent<LifeTime>())
            go.AddComponent<LifeTime>();
    }
    /// <summary>
    /// ����ô�򵥵ķ�����Ҫ��ע�ͣ����������ء�����
    /// </summary>
    public void CreateVfx(string id_or_name, Vector3 position, Vector3 scale, float lifeTime)
    {
        if (id_or_name == "0")
            return;
        GameObject go = Instantiate(GetVfxByIdOrName(id_or_name));
        go.transform.position = position;
        go.transform.localScale = scale;
        if (!go.GetComponent<LifeTime>())
            go.AddComponent<LifeTime>().lifeTime = lifeTime;
    }
    /// <summary>
    /// �л�����Ĳ���
    /// </summary>
    /// <param name="spriteRenderer"></param>
    /// <param name="material"></param>
    public void ObjectChangeMaterial(SpriteRenderer spriteRenderer, Material material)
    {
        if (!spriteRenderer)
        {
            Debug.LogWarning("û�л�ȡ����������ϵ���ͼ���");
            return;
        }
        spriteRenderer.material = material;

    }








}
[System.Serializable]
public class VfxData
{
    public string name;
    public string id;
    public GameObject vfx;
}
