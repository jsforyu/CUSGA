using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

enum Round { Player, Enemy };

public class FightManager : Singleton<FightManager>
{
    [Header("��ɫ����")]
    public CharacterData_SO playerData;
    public CharacterData_SO enemyData;

    private PlayerController playerController;
    private EnemyController enemyController;

    [Header("ս������")]
    private float attackTime = 0;
    private float recodeTime = 0;
    private float changeTime = 0;
    private int enemyAttackNums = 0;
    private bool canAttack = true;
    private bool canChange = true;

    [Header("�������")]
    [HideInInspector]
    public int currentIndex = 0;
    public GameObject[] playeralphas;
    public int playerAttackNums = 0;
    [HideInInspector]
    public List<int> index=new List<int>();

    [Header("��������")]
    public GameObject[] alphas;
    public float[] normalAlphas;
    public float[] bestAlphas;
    public float speed;

    [Header("������")]
    public GameObject rip;
    public float ripSpeed;

    private void Start()
    {
        playerController = PlayerController.Instance;
        enemyController = EnemyController.Instance;
        TurnToPlayer();
    }

    private void Update()
    {

    }

    #region ------ս�������߼�-------
    void Player_Attack()
    {
        if (canAttack && playerAttackNums > 0)
        {
            playeralphas[index[currentIndex]].SetActive(true);
            currentIndex++;
            if (currentIndex >= index.Count) { currentIndex = 0; }
            canAttack = false;
            playerAttackNums--;
            Debug.Log("���ǹ���");
        }
        if (playerAttackNums <= 0 && canAttack) { changeTime += Time.deltaTime; if (changeTime >= 1f) { ChangeAllStates(); } }
    }
    void Enemy_Attack()
    {
        enemyAni.SetTrigger("Attack");
        if (canAttack && enemyAttackNums > 0)
        {
            int temp_alpha = Random.Range(0, 5);
            alphas[temp_alpha].SetActive(true);
            canAttack = false;
            enemyAttackNums--;
        }
        if (enemyAttackNums <= 0 && canAttack) { changeTime += Time.deltaTime;if (changeTime >= 1f) { ChangeAllStates(); } }
    }

    void TurnToEnemy()
    {
        enemyAttackNums = enemyData.�ӵ�����;   // �ݶ�
        attackTime = recodeTime = 0;
    }
    void TurnToPlayer()
    {
        playerAttackNums = playerData.�ӵ�����;
        attackTime = recodeTime = 0;
    }
    #endregion

    #region------������ϵͳ------
    public void JudgeAnger()
    {
        JudgeEnemyAnger();
        JudgePlayerAnger(); 
    }
    public void JudgeEnemyAnger()
    {
        //if(enemyData.��ǰ������>=enemyData.��������)
        //{
        //    Time.timeScale = 0;
        //    rip.SetActive(true);
        //    enemyData.��ǰ������ = 0;
        //}
    }
    public void JudgePlayerAnger()
    {
        //if(playerData.��ǰ������>=playerData.��������)
        //{

        //}
    }
    public void PlayerPerfectBlock()
    {
        var ��Ҽ����� = playerData.��������;
        var ���˼����� = enemyData.��������;
        var ��ҹ����� = playerData.������;
        var ���˹����� = enemyData.������;
        enemyData.��ǰ������ = Mathf.Min(���˼�����, enemyData.��ǰ������ + ���˼����� / (10 - (���˹����� - ��ҹ�����)));
        playerData.��ǰ������ = Mathf.Min(��Ҽ�����, playerData.��ǰ������ + ��Ҽ����� / 20);
    }
    public void PlayerNormalBlock()
    {
        var ��Ҽ����� = playerData.��������;
        var ���˼����� = enemyData.��������;
        var ��ҹ����� = playerData.������;
        var ���˹����� = enemyData.������;
        playerData.��ǰ������ = Mathf.Min(��Ҽ�����, playerData.��ǰ������ + ��Ҽ����� / (10 - (���˹����� - ��ҹ�����)));
    }
    public void PlayerNoBlock()
    {
        var ��Ҽ����� = playerData.��������;
        var ���˼����� = enemyData.��������;
        var ��ҹ����� = playerData.������;
        var ���˹����� = enemyData.������;
        playerData.��ǰ������ = Mathf.Min(��Ҽ�����, (��Ҽ����� / 10) + ���˹�����);
        playerData.��ǰ����ֵ = Mathf.Max(playerData.��ǰ����ֵ - ���˹�����, 0);
    }
    #endregion
}
