using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    [Header("��ǰ����")]
    public int nowCombo;
    /// <summary>
    /// �������
    /// </summary>
    public int maxCombo = 0;
    /// <summary>
    /// �ж�ʱ�䣬��Ҫ�޸�
    /// </summary>
    [Header("�����ϵ���ʱ��")]

    public float comboResetTime;
    /// <summary>
    /// Ĭ�ϵ��ж�ʱ�䣬��Ҫ�޸�
    /// </summary>
    public float comboResetTime_Default;
    /// <summary>
    /// �жϼ�ʱ������UI��ʾ
    /// </summary>
    public float comboResetTimer;
    [Header("��ǰ�÷ֳ˷�����")]
    public float nowScoreCorrection;
    [Header("��ǰ�÷ּӷ�����")]
    public float nowScoreCorrection_Plus;
    [Header("ÿ��һ���˵ö��ٷ�")]
    public float eachCrackScore;
    [Header("��ǰ�÷�")]
    public float nowScore;
    public Rank nowRnak = Rank.C;
    [Header("�÷����ֶ�������ʱ�ĸ�����")]
    public Transform scoreTextParent;
    [Header("���ܵĵ÷���ɫ")]
    public List<Color> possibleScoreColors;

    public static ScoreManager instance;



    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        comboResetTime = comboResetTime_Default;
    }

    private void Update()
    {
        ComnboTimeCaculate();
        ComboDeclineCaculate();
        ScoreCorrectionCaculate();
    }

    /// <summary>
    /// ���������жϵ�ʱ��Ӧ���Ƕ���
    /// </summary>
    public void ComnboTimeCaculate()
    {
        //����Խ�࣬�ж�ʱ��Խ�̣�����������һ���̶�ʱ��Ϊ���
        //��ʼ��3s�����Ϊ1�룿

        //�����԰�Ϊ��λ����ʼΪ3��100Ϊ2��200Ϊ1��������С�����������Ȼ��ǻ��������
        float x = (float)nowCombo / 100;
        comboResetTime = comboResetTime_Default - x;

        //�߽紦��
        if (comboResetTime >= comboResetTime_Default)
            comboResetTime = comboResetTime_Default;
        if (comboResetTime <= 1)
            comboResetTime = 1;
    }

    /// <summary>
    /// ���������ж�
    /// </summary>
    public void ComboDeclineCaculate()
    {
        if (nowCombo != 0)
        {

            comboResetTimer -= Time.deltaTime;
            if (comboResetTimer <= 0)
            {
                //��
                nowCombo = 0;
                comboResetTime = comboResetTime_Default;
                comboResetTimer = comboResetTime;
                UIManager.instance.RefreshCombo(nowCombo.ToString());
                UIManager.instance.RefreshCorrection();
            }
        }
    }

    /// <summary>
    /// �÷ֲ�������
    /// </summary>
    public void ScoreCorrectionCaculate()
    {
        //���ݵ�ǰcombo������÷ֲ���
        //combo/20?
        nowScoreCorrection = (float)nowCombo / 50;

    }

    /// <summary>
    /// ��ȡһ��ײ��֮��Ӧ�û�õķ���
    /// </summary>
    /// <returns></returns>
    public float GetScore()
    {
        float x = (eachCrackScore + nowScoreCorrection_Plus) * (nowScoreCorrection + 1);
        return x;
    }

    /// <summary>
    /// ײ��Ŀ��֮��ִ�е�һϵ����÷���صķ���
    /// </summary>
    /// <param name="pos"></param>
    public void CrackOneTarget(Vector3 pos)
    {
        //Ҫ����ķ���
        float score = GetScore();


        //�������ֱ�ʾ����
        GameObject go = Instantiate(totalGameManager.instance.scoreTextPrefab, scoreTextParent);
        go.transform.position = pos;

        TextMeshProUGUI text = go.GetComponentInChildren<TextMeshProUGUI>();
        text.text = score.ToString();
        text.color = possibleScoreColors[Random.Range(0, possibleScoreColors.Count)];
        if (totalGameManager.instance.train.nowState == TrainState.drift)
        {
            text.GetComponent<Animator>().enabled = true;
        }

        nowScore += score;
        //��������
        nowCombo += 1;
        if (nowCombo >= maxCombo)
            maxCombo = nowCombo;
        //�����ж�ʱ������
        comboResetTimer = comboResetTime;
        //ˢ��UI
        //Debug.Log(nowCombo);
        UIManager.instance.RefreshCombo(nowCombo.ToString());
        UIManager.instance.RefreshScore(nowScore.ToString());
        UIManager.instance.RefreshRank(RankCaculate());
        UIManager.instance.RefreshCorrection();

    }

    /// <summary>
    /// ��������
    /// </summary>
    public string RankCaculate()
    {
        //���ݵ÷�switch���У�
        if (nowScore <= 100000)
            return "D";
        else if (nowScore <= 400000)
            return "C";
        else if (nowScore <= 1000000)
            return "B";
        else if (nowScore <= 2000000)
            return "A";
        else if (nowScore <= 3000000)
            return "S";
        else if (nowScore <= 5000000)
            return "SS";
        else
            return "SSS";
    }

}
public enum Rank
{
    C, B, A, S, SS, SSS
}
