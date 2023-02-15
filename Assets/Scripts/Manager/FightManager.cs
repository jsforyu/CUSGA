using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject[] enemyAttackDir;
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
                EnemyAttack();
                break;
            default: break;
        }
    }
    void EnemyAttack()
    {
        if (enemyAttackNums>0 && canAttack)
        {
            enemyAni.SetTrigger("Attack");
            canAttack = false;
            int temp = Random.Range(0, 6);
            ClearDir();
            Debug.Log(temp);
            enemyAttackDir[temp].SetActive(true);
            enemyAttackDir[temp].GetComponent<EnemyAttackDir>().AttackTime = enemyData.�����ٶ�;
            enemyAttackNums--;
            Debug.Log("�ӵ�����"+enemyAttackNums);
        }
        if (enemyAttackNums <= 0 && canAttack) { ChangeAllStates(); }
    }
    public void ClearDir()
    {
        foreach (var dir in enemyAttackDir)
        {
            dir.SetActive(false);
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
