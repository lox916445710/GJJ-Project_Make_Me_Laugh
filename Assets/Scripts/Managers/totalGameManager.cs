using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class totalGameManager : MonoBehaviour
{
    public bool DEBUG;
    public bool SkipState2;
    [Header("火车")]
    public Train train;
    [Header("初始状态")]
    public GameState firstGameState;
    [Header("背景滚动速度")]
    public float bgScrollSpeed;
    [Header("滚动速度全局加成，所有的向后位移的效果速度都会加上这个值")]
    public float globalScrollSppedCorrection;
    [Header("全局加成的自动增加速度")]
    public float globalSpeedIncreaseSpeed;
    //[Header("生成目标时间的减小速度")]
    [Header("一栏背景的长度")]
    public float onePartDistance;
    [Header("要创的对象预制件")]
    public GameObject targetPrefab;
    [Header("对象的尸体预制件")]
    public GameObject targetDiedBodyPrefab;
    public Transform targetsParent;
    [Header("分数文字预制件")]
    public GameObject scoreTextPrefab;
    [Header("将会生成对象的三个点位")]
    public List<Transform> targetGeneratePoint;
    [Header("创的时候可能生成的vfx的ids")]
    public List<string> crashingVfxids;

    [Header("创建目标间隔时间")]
    public float eachCreateTime;
    [Header("创建目标间隔时间减少速度")]
    public float createTimeDeclineSpeed;
    [Header("最小间隔时间")]
    public float minCreateTime;


    [Header("上（0）中（1）下（2）三条线")]
    public List<Transform> threeTrainLines;


    [Header("总共出现多少人")]
    public int allAppearedTarget;
    [Header("总共创了多少人")]
    public int allCrashedAmounts;
    [Header("总共得了多少分")]
    public float score;
    [Header("总共漂移了多少次")]
    public float allDriftAmounts;


    public RuntimeAnimatorController shadowAnimator;
    public GameState nowGameState;
    public static totalGameManager instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;

    }
    private void Update()
    {
        if (nowGameState == GameState.S3 || nowGameState == GameState.S4)
        {
            globalScrollSppedCorrection += Time.deltaTime * globalSpeedIncreaseSpeed;
            eachCreateTime -= Time.deltaTime * createTimeDeclineSpeed;
            if (eachCreateTime <= 0.01f)
                eachCreateTime = 0.01f;
        }
    }
    private void Start()
    {
        StartCoroutine(CreatingTarget());
        if (firstGameState == GameState.S1)
        {
            State1Start();
        }
        if (firstGameState == GameState.S2)
        {
            State2Start();

        }
        if (firstGameState == GameState.S3)
        {
            State3Start();
        }
    }
    public void SetSkipState2(bool isOn)
    {
        SkipState2 = isOn;
    }
    /// <summary>
    /// 隐藏首界面并进入第二阶段
    /// </summary>
    public void HideStartMenu()
    {
        UIManager.instance.startMenu.enabled = true;
        if (!SkipState2)
            State2Start();
        if (SkipState2)
            State3Start();
        StartCoroutine(ShowUIs());
    }
    /// <summary>
    /// 显示除初始界面之外其他的UI
    /// </summary>
    IEnumerator ShowUIs()
    {
        CanvasGroup cg = GameObject.Find("BatttleUI").GetComponent<CanvasGroup>();
        while (cg.alpha < 1)
        {
            cg.alpha += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }


    /// <summary>
    /// 游戏进入第二阶段
    /// </summary>
    public void State2Start()
    {
        nowGameState = GameState.S2;
        StartCoroutine(StartSpeaking());
    }

    /// <summary>
    /// 游戏进入第一阶段
    /// </summary>
    public void State1Start()
    {
        nowGameState = GameState.S1;

    }

    IEnumerator StartSpeaking()
    {

        //yield return new WaitForSeconds(4);
        for (int i = 0; i < UIManager.instance.speaks.Count; i++)
        {
            string s = UIManager.instance.speaks[i];

            if (i != 0)
                yield return new WaitForSeconds(5.5f);
            else
                yield return new WaitForSeconds(2.5f);

            UIManager.instance.Speak(s);
            if (i == 6 || i == 9)
            {
                yield return new WaitForSeconds(1.5f);
                CreateTurtuialTargets();
            }
        }
        yield return new WaitForSeconds(3);
        State3Start();

    }


    public void State3Start()
    {
        nowGameState = GameState.S3;
    }

    public void State4Start()
    {
        nowGameState = GameState.S4;
        eachCreateTime = 0.1f;
        globalScrollSppedCorrection += 10;
        train.eachCrackDashBarRecover /= 2;
    }

    public void CreateTurtuialTargets()
    {
        StartCoroutine(Createturtuialtargets());

    }
    /// <summary>
    /// 展示结算界面
    /// </summary>

    public void ShowFinalUI()
    {

    }






















    IEnumerator Createturtuialtargets()
    {
        for (int i = 0; i < 30; i++)
        {
            CreateTargetAtLine(i);
            yield return new WaitForSeconds(0.05f);
        }
    }




    IEnumerator CreatingTarget()
    {

        while (true)
        {
            yield return new WaitForSeconds(eachCreateTime);
            //只在3或4阶段生成
            if (nowGameState == GameState.S3 || nowGameState == GameState.S4)
                CreateTarget();
        }
    }
    public void CreateTargetAtLine(int line)
    {
        int index = line % 3;
        GameObject go = Instantiate(targetPrefab, targetsParent);
        go.transform.position = targetGeneratePoint[index].position;
    }
    public void CreateTarget()
    {
        int index = Random.Range(0, targetGeneratePoint.Count);
        GameObject go = Instantiate(targetPrefab, targetsParent);
        go.transform.position = targetGeneratePoint[index].position;
    }
    public GameObject CreateDiedBody(Vector3 pos)
    {
        GameObject go = Instantiate(targetDiedBodyPrefab, targetsParent);
        go.transform.position = pos;
        return go;
    }
}
public enum GameState
{
    /// <summary>
    /// 四种游戏状态，分别代表未开始时，纲进入时，正式游戏时和结束时，用于判断那些方法应该执行
    /// </summary>
    S1, S2, S3, S4
}