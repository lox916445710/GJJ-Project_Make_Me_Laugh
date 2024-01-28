using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;

public class Train : MonoBehaviour
{

    /// <summary>
    /// �����
    /// </summary>
    [Header("��ǰ�������")]
    public float nowDashBar;
    [Header("���������ֵ")]
    public float totalDashBar;
    [Header("ÿ��ײ����������")]
    public float eachCrackDashBarRecover;
    [Header("������������ٶ�")]
    public float dashEnergyDeclineSpeed;

    public Line nowLine = Line.middle;
    public Line targetLine = Line.middle;
    public float minx;
    public float maxx;
    Vector3 v;
    [Header("�����Ч����λ��")]
    public Transform fallVfxPoint;

    public bool isChanging;
    public Vector3 targetPos;
    [Header("�л��������ʱ��")]
    public float railChangeBuffer;
    public float railChangeBufferTimer;
    public bool canChangeRail = true;


    public Material material_normal;
    public Material material_drift;
    public Material material_skill;
    public TrainState nowState;

    public Transform graphic;
    public GameObject smokes;
    Animator animator;

    private void Start()
    {
        StartCoroutine(SelfShakeIenumerator());
        animator = GetComponent<Animator>();
        if (totalGameManager.instance.DEBUG)
        {
            eachCrackDashBarRecover = 100;
        }
    }
    private void Update()
    {
        MovingCheck();
        MovingInputCheck();
        //�л������ȴʱ��
        if (!canChangeRail && nowLine == targetLine)
        {
            railChangeBufferTimer -= Time.deltaTime;
            if (railChangeBufferTimer <= 0)
            {
                canChangeRail = true;
                railChangeBufferTimer = railChangeBuffer;
            }
        }
        if (nowState == TrainState.skill)
        {
            //graphic.transform.parent.localRotation =Quaternion.Euler(0, 0, Time.deltaTime * 1000);
            graphic.transform.parent.eulerAngles += new Vector3(0, 0, Time.deltaTime * 1000);
            graphic.transform.localPosition = Vector3.zero;

        }
        //if (Input.GetKeyDown(KeyCode.Z))
        //    StartCoroutine(GameOverIenumerator());
    }

    public void OnCrack()
    {
        //������������ͨ״̬+��Ϸ״̬����3��4״̬
        if (nowState == TrainState.normal && totalGameManager.instance.nowGameState == GameState.S3 || totalGameManager.instance.nowGameState == GameState.S4)
        {
            if (nowDashBar != totalDashBar && nowDashBar + eachCrackDashBarRecover >= totalDashBar)
            {

                UIManager.instance.Speak_OnDriftFull();

            }
            nowDashBar += eachCrackDashBarRecover;

            if (nowDashBar >= totalDashBar)
            {

                nowDashBar = totalDashBar;
                //3״̬�¿���Ư���ж�
                if (totalGameManager.instance.nowGameState == GameState.S3)
                {
                    graphic.GetComponent<SpriteRenderer>().material = material_drift;
                    UIManager.instance.canDriftInfo.SetActive(true);
                }
                //4״̬�¿������ռ����ж�
                if (totalGameManager.instance.nowGameState == GameState.S4)
                {
                    graphic.GetComponent<SpriteRenderer>().material = material_skill;
                    UIManager.instance.canSkillInfo.SetActive(true);

                }
            }
        }
    }
    float xSpeed = 4;
    public void MovingInputCheck()
    {
        if (nowState == TrainState.normal)
            xSpeed = 8;
        if (nowState == TrainState.drift)
            xSpeed = 24;
        if (nowState == TrainState.normal)
        {
            if (Input.GetKey(KeyCode.W))
                ChangeLine(true);
            if (Input.GetKey(KeyCode.S))
                ChangeLine(false);
        }
        if (Input.GetKey(KeyCode.A) && transform.position.x > minx)
        {
            transform.position += Vector3.left * xSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x < maxx)
        {
            transform.position += Vector3.right * xSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (nowDashBar == totalDashBar)
            {
                if (totalGameManager.instance.nowGameState == GameState.S3)
                {
                    Debug.Log("���Կ�ʼƯ��");
                    Drift();
                }
                if (totalGameManager.instance.nowGameState == GameState.S4)
                {
                    Debug.Log("���Կ�ʼ����");
                    Skill();
                }
            }
        }
    }

    public void MovingCheck()
    {
        if (nowLine != targetLine)
        {
            isChanging = true;
            if (targetLine == Line.down)
            {
                targetPos = totalGameManager.instance.threeTrainLines[2].position;
            }
            if (targetLine == Line.middle)
            {
                targetPos = totalGameManager.instance.threeTrainLines[1].position;
            }
            if (targetLine == Line.up)
            {
                targetPos = totalGameManager.instance.threeTrainLines[0].position;
            }
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetPos.y, transform.position.z), 14 * Time.deltaTime);
            //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, targetPos.y, 0), ref v, 0.6f);
            if (Mathf.Abs(transform.position.y - targetPos.y) < 0.05f)
            {
                animator.Play("Train_Normal");
                CameraManager.instance.impulseController.CreateImpulse(0);
                //Debug.Log("�ﵽ�յ�");
                nowLine = targetLine;
                //������Ч
                VfxManager.instance.CreateVfx("fall", fallVfxPoint.position, new Vector3(5, 5, 5));
                Music_Sound_Manager.instance.CreateCrackSound();
                isChanging = false;

            }

        }





    }
    /// <summary>
    /// �л����
    /// </summary>
    /// <param name="up"></param>
    public void ChangeLine(bool up)
    {
        if (!canChangeRail)
        {
            //Debug.Log("������ȴ��");
            return;
        }
        if (isChanging)
        {
            //Debug.Log("�Ѿ��ڱ任��");
            return;
        }
        if ((up && nowLine == Line.up) || (!up && nowLine == Line.down))
        {
            //Debug.Log("����߽�");
            return;
        }
        if (up)
        {
            if (nowLine == Line.middle)
                targetLine = Line.up;
            if (nowLine == Line.down)
                targetLine = Line.middle;
        }
        if (!up)
        {
            if (nowLine == Line.up)
                targetLine = Line.middle;
            if (nowLine == Line.middle)
                targetLine = Line.down;
        }
        animator.Play("Train_Normal");
        animator.Play("Train_Jump");
        UIManager.instance.humanSprite.GetComponent<Animator>().Play("Human_Do");
        canChangeRail = false;
    }

    /// <summary>
    /// Ư��
    /// </summary>
    public void Drift()
    {
        if (nowState == TrainState.normal)
            StartCoroutine(DriftIenumeratot());
    }

    IEnumerator DriftIenumeratot()
    {
        isChanging = false;
        targetLine = Line.middle;
        nowLine = Line.middle;


        transform.position = new Vector3(transform.position.x, totalGameManager.instance.threeTrainLines[1].position.y, transform.position.z);
        nowState = TrainState.drift;
        //��ʼ����



        Music_Sound_Manager.instance.PlayDriftMusic();
        UIManager.instance.canDriftInfo.SetActive(false);
        UIManager.instance.humanSprite.GetComponent<Animator>().Play("Human_Doying");
        UIManager.instance.humanSprite.material = material_drift;
        animator.Play("Train_Normal");
        animator.Play("Train_Drift");
        shouldShake = true;
        VfxManager.instance.driftWind.SetActive(true);
        ScoreManager.instance.nowScoreCorrection_Plus += 200;
        totalGameManager.instance.globalScrollSppedCorrection += 30;
        totalGameManager.instance.eachCreateTime /= 5;
        graphic.GetComponent<CreateShadow>().enabled = true;
        graphic.GetComponent<SpriteRenderer>().material = material_drift;
        while (nowDashBar > 0)
        {
            yield return new WaitForEndOfFrame();
            nowDashBar -= Time.deltaTime * dashEnergyDeclineSpeed;
        }
        // ��������
        animator.Play("Train_Normal");
        Music_Sound_Manager.instance.PlayNormalMusic();
        shouldShake = false;
        nowState = TrainState.normal;
        VfxManager.instance.driftWind.SetActive(false);
        ScoreManager.instance.nowScoreCorrection_Plus -= 200;
        totalGameManager.instance.globalScrollSppedCorrection -= 30;
        totalGameManager.instance.eachCreateTime *= 5;
        graphic.GetComponent<CreateShadow>().enabled = false;
        targetLine = Line.middle;
        graphic.GetComponent<SpriteRenderer>().material = material_normal;
        UIManager.instance.humanSprite.GetComponent<Animator>().Play("Human_Normal");
        UIManager.instance.Speak_OnDriftEmpty();
        UIManager.instance.humanSprite.material = material_normal;

        //��Ư�ƴ�����1
        totalGameManager.instance.allDriftAmounts++;
        if (!totalGameManager.instance.DEBUG)
        {
            if (totalGameManager.instance.allDriftAmounts >= 4)
            {
                //�����Ĵ�֮����Ϸ�������һ���׶Σ����ٴγ���֮���������ļ���
                totalGameManager.instance.State4Start();

            }
        }
        if (totalGameManager.instance.DEBUG)
        {
            totalGameManager.instance.State4Start();
        }
    }
    /// <summary>
    /// ���ڿ����Ƿ������ı���
    /// </summary>
    bool shouldShake = false;
    IEnumerator SelfShakeIenumerator()
    {
        while (true)
        {

            yield return new WaitForSeconds(0.2f);
            if (shouldShake)
            {

                graphic.GetComponent<CinemachineImpulseSource>().GenerateImpulse();

            }


        }
    }

    /// <summary>
    /// ��糵
    /// </summary>
    public void Skill()
    {
        if (nowState == TrainState.normal)
        {
            StartCoroutine(SkillIenumeratotr());
            StartCoroutine(GameOverIenumerator());
        }
    }
    IEnumerator SkillIenumeratotr()
    {
        Music_Sound_Manager.instance.musicAudioSource.Stop();
        isChanging = false;
        targetLine = Line.middle;
        nowLine = Line.middle;
        graphic.GetComponent<CreateShadow>().enabled = true;

        UIManager.instance.othersUI.Play("SkillsUI_Work");
        // ��ȡ���������е� SpriteRenderer ���
        SpriteRenderer[] sprites = FindObjectsOfType<SpriteRenderer>();
        // �������鲢��ÿ�� SpriteRenderer ������в���
        foreach (SpriteRenderer sprite in sprites)
        {

            sprite.color = Color.gray;
        }
        // ��ȡ���������е� Tilemap ���
        Tilemap[] tiles = FindObjectsOfType<Tilemap>();

        // �������鲢��ÿ�� Tilemap ������в���
        foreach (Tilemap tile in tiles)
        {
            tile.color = Color.gray;
        }
        UIManager.instance.humanSprite.color = Color.red;

        transform.position = new Vector3(transform.position.x, totalGameManager.instance.threeTrainLines[1].position.y, transform.position.z);
        nowState = TrainState.skill;

        //Music_Sound_Manager.instance.PlayPowerMusic();
        Music_Sound_Manager.instance.PlayPowerSound();
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1;


        // �������鲢��ÿ�� SpriteRenderer ������в���
        foreach (SpriteRenderer sprite in sprites)
        {
            // ������д��Ĵ���
            sprite.color = Color.white;
        }
        UIManager.instance.humanSprite.color = Color.white;


        Music_Sound_Manager.instance.PlayPowerMusic();
        UIManager.instance.canSkillInfo.SetActive(false);
        animator.Play("Train_Normal");
        animator.Play("train_Skill");
        UIManager.instance.humanSprite.GetComponent<Animator>().Play("Human_Strom");
        UIManager.instance.humanSprite.material = material_skill;
        shouldShake = true;
        VfxManager.instance.driftWind.SetActive(true);
        ScoreManager.instance.nowScoreCorrection_Plus += 100;
        totalGameManager.instance.globalScrollSppedCorrection += 50;
        totalGameManager.instance.eachCreateTime /= 4;
        while (nowDashBar > 0)
        {

            yield return new WaitForEndOfFrame();
            nowDashBar -= Time.deltaTime * dashEnergyDeclineSpeed;
        }





        animator.Play("Train_Normal");
        Music_Sound_Manager.instance.PlayNormalMusic();
        shouldShake = false;
        nowState = TrainState.normal;
        VfxManager.instance.driftWind.SetActive(false);
        ScoreManager.instance.nowScoreCorrection_Plus -= 200;
        totalGameManager.instance.globalScrollSppedCorrection -= 30;
        totalGameManager.instance.eachCreateTime *= 5;
        graphic.GetComponent<CreateShadow>().enabled = false;
        targetLine = Line.middle;
        graphic.GetComponent<SpriteRenderer>().material = material_normal;
        UIManager.instance.humanSprite.GetComponent<Animator>().Play("Human_Normal");
        UIManager.instance.Speak_OnDriftEmpty();
        UIManager.instance.humanSprite.material = material_normal;




    }


    /// <summary>
    /// ���ļ��ܿ���֮�󣬵���ʱһ��ʱ��糵�Ա�
    /// </summary>
    /// <returns></returns>
    IEnumerator GameOverIenumerator()
    {
        //smokes.SetActive(true);
        yield return new WaitForSeconds(10);
        for (int i = 0; i < 30; i++)
        {

            Vector3 offset = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 0);
            Debug.Log(offset);
            VfxManager.instance.CreateVfx("FinalBomb", graphic.transform.position + offset, new Vector3(3, 3, 3));

            yield return new WaitForSeconds(0.1f);

        }
        Music_Sound_Manager.instance.Cracksource.volume /= 0.8f;

        VfxManager.instance.CreateVfx("FinalBomb", graphic.transform.position, new Vector3(10, 10, 10));

        UIManager.instance.endMenu.Play("UI_Empty");
        Destroy(GameObject.Find("SkilText").gameObject);
        Destroy(gameObject);


    }
}
public enum Line
{
    up, middle, down
}
public enum TrainState
{
    normal, drift, skill
}