using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : Singleton<FightManager>
{
    [Header("角色属性")]
    public CharacterData_SO playerData;
    public CharacterData_SO enemyData;
    public PlayerController playerController;
    public EnemyController enemyContorller;
    [Header("战斗数据")]
    public int attackTime=0;
    private void Start()
    {
        Invoke("JudgeAttackSpeed", 1f); 
    }
    private void Update()
    {
        
    }
    void JudgeAttackSpeed()
    {
        if (playerData.攻击速度 >= enemyData.攻击速度) { playerController.playerStats = PlayerStats.Attack; }
        else { enemyContorller.enemyStats = EnemyStats.Attack; }
    }
    void ChangeAllStates()
    {
        if (playerController.playerStats == PlayerStats.Attack) { playerController.playerStats = PlayerStats.Defence; }
        else playerController.playerStats = PlayerStats.Attack;
        if (enemyContorller.enemyStats == EnemyStats.Attack) { enemyContorller.enemyStats = EnemyStats.Defence; }
        else enemyContorller.enemyStats = EnemyStats.Attack;
    }
}
