using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// 控制最终结算界面的脚本
/// </summary>
public class FinalEndMenu : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI totalAmountText;
    public TextMeshProUGUI continueAmountText;
    public TextMeshProUGUI FinalRankText;


    public Button againBtn;
    public Button exitBtn;

    public bool shoudWork;
    public bool worked;
    private void Start()
    {
        againBtn.onClick.AddListener(delegate
        {
            SceneManager.LoadScene("SampleScene");
        });
        exitBtn.onClick.AddListener(delegate
        {
            Application.Quit();
        });
    }
    private void Update()
    {
        if (shoudWork && !worked)
        {
            worked = true;
            StartCoroutine(ScoreShowIenumerator());
        }
    }
    
    
    /// <summary>
    /// 最终显示各项得分的协程
    /// </summary>
    /// <returns></returns>
    IEnumerator ScoreShowIenumerator()
    {
        yield return new WaitForSeconds(1);
        scoreText.text = "总得分：" + ScoreManager.instance.nowScore.ToString();
        scoreText.GetComponent<Animator>().Play("Text-Change");
        yield return new WaitForSeconds(1);
        totalAmountText.text = "总共创飞了" + totalGameManager.instance.allCrashedAmounts.ToString() + "   人";
        totalAmountText.GetComponent<Animator>().Play("Text-Change");
        yield return new WaitForSeconds(1);
        continueAmountText.text = "最大连创：" + ScoreManager.instance.maxCombo.ToString() + "   人";
        continueAmountText.GetComponent<Animator>().Play("Text-Change");
        yield return new WaitForSeconds(1);
        FinalRankText.text = ScoreManager.instance.RankCaculate();
        FinalRankText.GetComponent<Animator>().Play("Text-Change");
        yield return new WaitForSeconds(1);
        againBtn.gameObject.SetActive(true);
        exitBtn.gameObject.SetActive(true);
    }
}
