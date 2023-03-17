using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
public class FightManager : Singleton<FightManager>
{
    [Header("角色属性")]
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
    [Header("战斗数据")]
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
    [Header("玩家数据")]
    [HideInInspector]
    public int currentIndex = 0;
    public GameObject[] playeralphas;
    public int playerAttackNums = 0;
    [HideInInspector]
    public List<int> index=new List<int>();
    [Header("音符数据")]
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
    #region ------战斗基本逻辑-------
    void Player_Attack()
    {
        if (canAttack && playerAttackNums > 0)
        {
            playeralphas[index[currentIndex]].SetActive(true);
            currentIndex++;
            if (currentIndex >= index.Count) { currentIndex = 0; }
            canAttack = false;
            playerAttackNums--;
            Debug.Log("主角攻击");
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
        if (playerData.攻击速度 <= enemyData.攻击速度) { playerController.playerStats = PlayerStats.Attack; playerAttackNums = playerData.挥刀次数; ; }
        else { enemyContorller.enemyStats = EnemyStats.Attack; enemyAttackNums = enemyData.挥刀次数; }
        playerData.当前架势条 = 0;
        enemyData.当前架势条 = 0;
    }
    void TurnToEnemy()
    {
        enemyAttackNums = enemyData.挥刀次数;
        attackTime = recodeTime = 0;
    }
    void TurnToPlayer()
    {
        playerAttackNums = playerData.挥刀次数;
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
    #region------架势条系统------
    public void JudgeEnemyAnger()
    {
        if(enemyData.当前架势条>=enemyData.最大架势条)
        {
            Time.timeScale = 0;
        }
    }
    public void JudgePlayerAnger()
    {
        if(playerData.当前架势条>=playerData.最大架势条)
        {

        }
    }
    public void PlayerPerfectBlock()
    {
        var 玩家架势条 = playerData.最大架势条;
        var 敌人架势条 = enemyData.最大架势条;
        var 玩家攻击力 = playerData.攻击力;
        var 敌人攻击力 = enemyData.攻击力;
        enemyData.当前架势条 = Mathf.Min(敌人架势条, enemyData.当前架势条 + 敌人架势条 / (10 - (敌人攻击力 - 玩家攻击力)));
        playerData.当前架势条 = Mathf.Min(玩家架势条, playerData.当前架势条 + 玩家架势条 / 20);
    }
    public void PlayerNormalBlock()
    {
        var 玩家架势条 = playerData.最大架势条;
        var 敌人架势条 = enemyData.最大架势条;
        var 玩家攻击力 = playerData.攻击力;
        var 敌人攻击力 = enemyData.攻击力;
        playerData.当前架势条 = Mathf.Min(玩家架势条, playerData.当前架势条 + 玩家架势条 / (10 - (敌人攻击力 - 玩家攻击力)));
    }
    public void PlayerNoBlock()
    {
        var 玩家架势条 = playerData.最大架势条;
        var 敌人架势条 = enemyData.最大架势条;
        var 玩家攻击力 = playerData.攻击力;
        var 敌人攻击力 = enemyData.攻击力;
        playerData.当前架势条 = Mathf.Min(玩家架势条,(玩家架势条 / 10) + 敌人攻击力);
        playerData.当前生命值 = Mathf.Max(playerData.当前生命值 - 敌人攻击力, 0);
    }
    #endregion
}
