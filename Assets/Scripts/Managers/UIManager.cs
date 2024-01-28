using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public bool isTyping;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI correctionText;
    public TextMeshProUGUI speakText;
    public TextMeshProUGUI rankText;

    public Image humanSprite;
    public GameObject canDriftInfo;
    public GameObject canSkillInfo;

    public Image energyBar;
    public Image comboBar;

    public Animator othersUI;

    [Header("首界面")]
    public Animator startMenu;
    [Header("结算界面")]
    public Animator endMenu;
    [Multiline(4)]
    public List<string> speaks;

    [Multiline(4)]
    public List<string> speaks_OnDriftBarFull;
    [Multiline(4)]
    public List<string> speaks_OnDriftBarEnd;
    [Multiline(4)]
    public List<string> speaks_OnFinalFull;

    public static UIManager instance;


    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;

        }
        instance = this;
    }

    private void Update()
    {
        RefreshEnergyBar();
        RefreshCombonBar();
    }

    public void RefreshEnergyBar()
    {
        energyBar.fillAmount = totalGameManager.instance.train.nowDashBar / totalGameManager.instance.train.totalDashBar;
        if (energyBar.fillAmount == 1)
        {
            if (totalGameManager.instance.nowGameState == GameState.S3)
                energyBar.color = Color.blue;
            if (totalGameManager.instance.nowGameState == GameState.S4)
                energyBar.color = Color.red;


        }
        else
            energyBar.color = Color.white;
    }

    public void RefreshCombonBar()
    {
        comboBar.fillAmount = ScoreManager.instance.comboResetTimer / ScoreManager.instance.comboResetTime;
    }


    public void RefreshScore(string newScore)
    {
        scoreText.text = newScore;
        scoreText.GetComponent<Animator>().Play("Text-Change");
    }

    public void RefreshCombo(string newCombo)
    {
        comboText.text = newCombo;
        comboText.GetComponent<Animator>().Play("Text-Change");
    }

    public void RefreshRank(string newRank)
    {
        if (newRank != rankText.text)
            rankText.GetComponent<Animator>().Play("Text-Change");
        rankText.text = newRank;

    }
    public void RefreshCorrection()
    {
        correctionText.text = "SOCRE(100 + " + ScoreManager.instance.nowScoreCorrection_Plus + " ) * " + (ScoreManager.instance.nowScoreCorrection + 1).ToString();
        correctionText.GetComponent<Animator>().Play("Text-Change");
    }
    Coroutine typingCorountine;
    public void Speak(string text)
    {
        if (typingCorountine != null)
        {
            StopCoroutine(typingCorountine);
        }
        typingCorountine = StartCoroutine(typeText(text, speakText));

    }
    int fullIndex = 0;
    int emptyIndex = 0;
    /// <summary>
    /// 当能量条充满的时候触发的speak
    /// </summary>
    public void Speak_OnDriftFull()
    {

        if (typingCorountine != null)
        {
            StopCoroutine(typingCorountine);
        }
        if (totalGameManager.instance.nowGameState == GameState.S3)
            typingCorountine = StartCoroutine(typeText(speaks_OnDriftBarFull[fullIndex], speakText));
        if (totalGameManager.instance.nowGameState == GameState.S4)
            typingCorountine = StartCoroutine(typeText(speaks_OnFinalFull[0], speakText));


        fullIndex++;
        if (fullIndex >= speaks_OnDriftBarFull.Count - 1)
            fullIndex = speaks_OnDriftBarFull.Count - 1;

    }
    /// <summary>
    /// 当能量条清零的时候触发的speak
    /// </summary>
    public void Speak_OnDriftEmpty()
    {

        if (typingCorountine != null)
        {
            StopCoroutine(typingCorountine);
        }
        typingCorountine = StartCoroutine(typeText(speaks_OnDriftBarEnd[emptyIndex], speakText));
        emptyIndex++;
        if (emptyIndex >= speaks_OnDriftBarEnd.Count - 1)
            emptyIndex = speaks_OnDriftBarEnd.Count - 1;

    }



    /// <summary>
    /// 打字协程(作用到的是用于显示的文本）
    /// </summary>
    /// <param name="fullText"></param>
    /// <returns></returns>
    IEnumerator typeText(string fullText, TextMeshProUGUI text_ForShow)
    {
        isTyping = true;
        bool isJumping = false;



        string nowText = "";
        for (int i = 0; i < fullText.Length + 1; i++)
        {

            //检测到<时开始跳过，检测到>时结束跳过
            if (i < fullText.Length)
            {
                if (fullText[i] == '<')
                    isJumping = true;
            }
            if (i - 1 >= 0)
            {
                if (fullText[i - 1] == '>')
                    isJumping = false;
            }
            if (isJumping == true)
                continue;

            nowText = fullText.Substring(0, i);
            text_ForShow.text = nowText;
            yield return new WaitForSeconds(0.05f);

        }

        isTyping = false;

    }
}
