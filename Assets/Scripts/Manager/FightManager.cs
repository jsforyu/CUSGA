using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
public class FightManager : Singleton<FightManager>
{
    [Header("��ɫ����")]
    [HideInInspector]
    public CharacterData_SO playerData;
    [HideInInspector]
    public CharacterData_SO enemyData;
    [HideInInspector]
    public Animator playerAni;
    [HideInInspector]
    public Animator enemyAni;
    [HideInInspector]
    public PlayerController playerController;
    [HideInInspector]
    public EnemyController enemyContorller;
    [Header("ս������")]
    [HideInInspector]
    public float attackTime = 0;
    [HideInInspector]
    public float recodeTime = 0;
    [HideInInspector]
    public float changeTime = 0;
    [HideInInspector]
    public int enemyAttackNums = 0;
    [HideInInspector]
    public bool canAttack = true;
    [HideInInspector]
    public bool canChange = true;
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
    private void Start()
    {
        JudgeAttackSpeed();
       
    }
    private void Update()
    {
        switch (playerController.playerStats)
        {
            case PlayerStats.Attack:
                Player_Attack();
                break;
            case PlayerStats.Defence:
                Enemy_Attack();
                break;
            default: break;
        }
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
    void JudgeAttackSpeed()
    {
        if (playerData.�����ٶ� <= enemyData.�����ٶ�) { playerController.playerStats = PlayerStats.Attack; playerAttackNums = playerData.�ӵ�����; ; }
        else { enemyContorller.enemyStats = EnemyStats.Attack; enemyAttackNums = enemyData.�ӵ�����; }
        playerData.��ǰ������ = 0;
        enemyData.��ǰ������ = 0;
    }
    void TurnToEnemy()
    {
        enemyAttackNums = enemyData.�ӵ�����;
        attackTime = recodeTime = 0;
    }
    void TurnToPlayer()
    {
        playerAttackNums = playerData.�ӵ�����;
        attackTime = 0;
        recodeTime = 0;
    }
    void ChangeAllStates()
    {
        if (playerController.playerStats == PlayerStats.Attack) { playerController.playerStats = PlayerStats.Defence; TurnToEnemy(); }
        else { playerController.playerStats = PlayerStats.Attack; TurnToPlayer(); }
        if (enemyContorller.enemyStats == EnemyStats.Attack) { enemyContorller.enemyStats = EnemyStats.Defence; }
        else enemyContorller.enemyStats = EnemyStats.Attack;
        changeTime = 0;
    }
    #endregion
    #region------������ϵͳ------
    public void JudgeEnemyAnger()
    {
        if(enemyData.��ǰ������>=enemyData.��������)
        {
            Time.timeScale = 0;
        }
    }
    public void JudgePlayerAnger()
    {
        if(playerData.��ǰ������>=playerData.��������)
        {

        }
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
        playerData.��ǰ������ = Mathf.Min(��Ҽ�����,(��Ҽ����� / 10) + ���˹�����);
        playerData.��ǰ����ֵ = Mathf.Max(playerData.��ǰ����ֵ - ���˹�����, 0);
    }
    #endregion
}
