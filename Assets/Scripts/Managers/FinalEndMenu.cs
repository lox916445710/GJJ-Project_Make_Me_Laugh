using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// �������ս������Ľű�
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
    /// ������ʾ����÷ֵ�Э��
    /// </summary>
    /// <returns></returns>
    IEnumerator ScoreShowIenumerator()
    {
        yield return new WaitForSeconds(1);
        scoreText.text = "�ܵ÷֣�" + ScoreManager.instance.nowScore.ToString();
        scoreText.GetComponent<Animator>().Play("Text-Change");
        yield return new WaitForSeconds(1);
        totalAmountText.text = "�ܹ�������" + totalGameManager.instance.allCrashedAmounts.ToString() + "   ��";
        totalAmountText.GetComponent<Animator>().Play("Text-Change");
        yield return new WaitForSeconds(1);
        continueAmountText.text = "���������" + ScoreManager.instance.maxCombo.ToString() + "   ��";
        continueAmountText.GetComponent<Animator>().Play("Text-Change");
        yield return new WaitForSeconds(1);
        FinalRankText.text = ScoreManager.instance.RankCaculate();
        FinalRankText.GetComponent<Animator>().Play("Text-Change");
        yield return new WaitForSeconds(1);
        againBtn.gameObject.SetActive(true);
        exitBtn.gameObject.SetActive(true);
    }
}
