using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    [Header("当前连击")]
    public int nowCombo;
    /// <summary>
    /// 最大连击
    /// </summary>
    public int maxCombo = 0;
    /// <summary>
    /// 中断时间，需要修改
    /// </summary>
    [Header("连击断掉的时间")]

    public float comboResetTime;
    /// <summary>
    /// 默认的中段时间，不要修改
    /// </summary>
    public float comboResetTime_Default;
    /// <summary>
    /// 中断计时，用于UI显示
    /// </summary>
    public float comboResetTimer;
    [Header("当前得分乘法补正")]
    public float nowScoreCorrection;
    [Header("当前得分加法补正")]
    public float nowScoreCorrection_Plus;
    [Header("每创一个人得多少分")]
    public float eachCrackScore;
    [Header("当前得分")]
    public float nowScore;
    public Rank nowRnak = Rank.C;
    [Header("得分文字对象生成时的父对象")]
    public Transform scoreTextParent;
    [Header("可能的得分颜色")]
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
    /// 计算连击中断的时间应该是多少
    /// </summary>
    public void ComnboTimeCaculate()
    {
        //连击越多，中断时间越短，在连击到达一定程度时变为最短
        //初始是3s，最短为1秒？

        //试试以百为单位，初始为3，100为2，200为1并保持最小，但补正力度还是会继续增加
        float x = (float)nowCombo / 100;
        comboResetTime = comboResetTime_Default - x;

        //边界处理
        if (comboResetTime >= comboResetTime_Default)
            comboResetTime = comboResetTime_Default;
        if (comboResetTime <= 1)
            comboResetTime = 1;
    }

    /// <summary>
    /// 计算连击中断
    /// </summary>
    public void ComboDeclineCaculate()
    {
        if (nowCombo != 0)
        {

            comboResetTimer -= Time.deltaTime;
            if (comboResetTimer <= 0)
            {
                //断
                nowCombo = 0;
                comboResetTime = comboResetTime_Default;
                comboResetTimer = comboResetTime;
                UIManager.instance.RefreshCombo(nowCombo.ToString());
                UIManager.instance.RefreshCorrection();
            }
        }
    }

    /// <summary>
    /// 得分补正计算
    /// </summary>
    public void ScoreCorrectionCaculate()
    {
        //根据当前combo数计算得分补正
        //combo/20?
        nowScoreCorrection = (float)nowCombo / 50;

    }

    /// <summary>
    /// 获取一次撞击之中应该获得的分数
    /// </summary>
    /// <returns></returns>
    public float GetScore()
    {
        float x = (eachCrackScore + nowScoreCorrection_Plus) * (nowScoreCorrection + 1);
        return x;
    }

    /// <summary>
    /// 撞到目标之后执行的一系列与得分相关的方法
    /// </summary>
    /// <param name="pos"></param>
    public void CrackOneTarget(Vector3 pos)
    {
        //要计算的分数
        float score = GetScore();


        //生成文字表示对象
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
        //连击计算
        nowCombo += 1;
        if (nowCombo >= maxCombo)
            maxCombo = nowCombo;
        //连击中断时间重置
        comboResetTimer = comboResetTime;
        //刷新UI
        //Debug.Log(nowCombo);
        UIManager.instance.RefreshCombo(nowCombo.ToString());
        UIManager.instance.RefreshScore(nowScore.ToString());
        UIManager.instance.RefreshRank(RankCaculate());
        UIManager.instance.RefreshCorrection();

    }

    /// <summary>
    /// 评级计算
    /// </summary>
    public string RankCaculate()
    {
        //根据得分switch就行？
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
