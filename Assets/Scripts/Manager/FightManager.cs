using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FightManager : Singleton<FightManager>
{
    [Header("��ɫ����")]
    public CharacterData_SO playerData;
    public CharacterData_SO enemyData;
    public Animator playerAni;
    public Animator enemyAni;
    public PlayerController playerController;
    public EnemyController enemyContorller;
    [Header("ս������")]
    public float attackTime=0;
    public float recodeTime = 0;
    public int enemyAttackNums=0;
    public bool canAttack = true;
    public bool canChange = true;
    public GameObject[] alphas;
    public float speed;
    public float[] normalAlphas;
    public float[] bestAlphas;
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
