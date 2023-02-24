using System.Collections;
using System.Collections.Generic;
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
    public float attackTime=0;
    [HideInInspector]
    public float recodeTime = 0;
    [HideInInspector]
    public int enemyAttackNums=0;
    [HideInInspector]
    public bool canAttack = true;
    [HideInInspector]
    public bool canChange = true;
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
                break;
            case PlayerStats.Defence:
                Enemy_Attack();
                break;
            default: break;
        }
    }
    void Enemy_Attack()
    {
        if(canAttack&&enemyAttackNums>0)
        {
            int temp_alpha = Random.Range(0, 5);
            alphas[temp_alpha].SetActive(true);
            canAttack = false;
            enemyAttackNums--;
        }
    }
    void JudgeAttackSpeed()
    {
        if (playerData.�����ٶ� <= enemyData.�����ٶ�) { playerController.playerStats = PlayerStats.Attack;  }
        else { enemyContorller.enemyStats = EnemyStats.Attack; enemyAttackNums = enemyData.�ӵ�����; }
    }
    void ChangeAllStates()
    {
        if (playerController.playerStats == PlayerStats.Attack) { playerController.playerStats = PlayerStats.Defence; TurnToEnemy(); }
        else { playerController.playerStats = PlayerStats.Attack; TurnToEnemy(); }
        if (enemyContorller.enemyStats == EnemyStats.Attack) { enemyContorller.enemyStats = EnemyStats.Defence; }
        else enemyContorller.enemyStats = EnemyStats.Attack;
    }
    void TurnToEnemy()
    {
        enemyAttackNums = enemyData.�ӵ�����;
        attackTime = recodeTime = 0;
    }
    void TurnToPlayer()
    {
        attackTime = 0;
        recodeTime = 0;
    }
}
