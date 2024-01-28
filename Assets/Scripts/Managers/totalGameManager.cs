using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class totalGameManager : MonoBehaviour
{
    public bool DEBUG;
    public bool SkipState2;
    [Header("��")]
    public Train train;
    [Header("��ʼ״̬")]
    public GameState firstGameState;
    [Header("���������ٶ�")]
    public float bgScrollSpeed;
    [Header("�����ٶ�ȫ�ּӳɣ����е����λ�Ƶ�Ч���ٶȶ���������ֵ")]
    public float globalScrollSppedCorrection;
    [Header("ȫ�ּӳɵ��Զ������ٶ�")]
    public float globalSpeedIncreaseSpeed;
    //[Header("����Ŀ��ʱ��ļ�С�ٶ�")]
    [Header("һ�������ĳ���")]
    public float onePartDistance;
    [Header("Ҫ���Ķ���Ԥ�Ƽ�")]
    public GameObject targetPrefab;
    [Header("�����ʬ��Ԥ�Ƽ�")]
    public GameObject targetDiedBodyPrefab;
    public Transform targetsParent;
    [Header("��������Ԥ�Ƽ�")]
    public GameObject scoreTextPrefab;
    [Header("�������ɶ����������λ")]
    public List<Transform> targetGeneratePoint;
    [Header("����ʱ��������ɵ�vfx��ids")]
    public List<string> crashingVfxids;

    [Header("����Ŀ����ʱ��")]
    public float eachCreateTime;
    [Header("����Ŀ����ʱ������ٶ�")]
    public float createTimeDeclineSpeed;
    [Header("��С���ʱ��")]
    public float minCreateTime;


    [Header("�ϣ�0���У�1���£�2��������")]
    public List<Transform> threeTrainLines;


    [Header("�ܹ����ֶ�����")]
    public int allAppearedTarget;
    [Header("�ܹ����˶�����")]
    public int allCrashedAmounts;
    [Header("�ܹ����˶��ٷ�")]
    public float score;
    [Header("�ܹ�Ư���˶��ٴ�")]
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
    /// �����׽��沢����ڶ��׶�
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
    /// ��ʾ����ʼ����֮��������UI
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
    /// ��Ϸ����ڶ��׶�
    /// </summary>
    public void State2Start()
    {
        nowGameState = GameState.S2;
        StartCoroutine(StartSpeaking());
    }

    /// <summary>
    /// ��Ϸ�����һ�׶�
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
    /// չʾ�������
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
            //ֻ��3��4�׶�����
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
    /// ������Ϸ״̬���ֱ����δ��ʼʱ���ٽ���ʱ����ʽ��Ϸʱ�ͽ���ʱ�������ж���Щ����Ӧ��ִ��
    /// </summary>
    S1, S2, S3, S4
}