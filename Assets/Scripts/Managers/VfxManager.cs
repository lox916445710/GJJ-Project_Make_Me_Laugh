using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 不会吧不会吧，不会特效管理器还要写注释吧（）
/// </summary>
public class VfxManager : MonoBehaviour
{
    public Material defaultMaterial;
    public Material objectChoosedMaterial;
    public Material uiGlowMaterial;


    [Header("特效")]
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
    /// 连这么简单的方法都要看注释，真是杂鱼呢～
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
        //go.name = "VFX：" + createAmount;
        return go;
    }
    /// <summary>
    /// 连这么简单的方法都要看注释，真是杂鱼呢～
    /// </summary>
    /// <param name="id_or_name"></param>
    /// <param name="position"></param>
    public GameObject CreateVfx(string id_or_name, Transform parent)
    {

        GameObject go = Instantiate(GetVfxByIdOrName(id_or_name), parent);

        //if (!go.GetComponent<LifeTime>())
        //    go.AddComponent<LifeTime>();

        go.name = "VFX：" + createAmount;

        return go;
        //Debug.Log(go.name);
    }
    /// <summary>
    /// 连这么简单的方法都要看注释，真是杂鱼呢～～
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
    /// 连这么简单的方法都要看注释，真是杂鱼呢～～
    /// </summary>
    public void CreateVfx(string id_or_name, Vector3 position, Vector3 scale, Quaternion rotation)
    {
        if (id_or_name == "0")
            return;
        GameObject go = Instantiate(GetVfxByIdOrName(id_or_name));
        go.name = "VFX：" + createAmount;
        Debug.Log(go.name);
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.localScale = scale;
        if (!go.GetComponent<LifeTime>())
            go.AddComponent<LifeTime>();
    }
    /// <summary>
    /// 连这么简单的方法都要看注释，真是杂鱼呢～～～
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
    /// 切换对象的材质
    /// </summary>
    /// <param name="spriteRenderer"></param>
    /// <param name="material"></param>
    public void ObjectChangeMaterial(SpriteRenderer spriteRenderer, Material material)
    {
        if (!spriteRenderer)
        {
            Debug.LogWarning("没有获取到这个对象上的贴图组件");
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
