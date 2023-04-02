using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

enum Round { Player, Enemy };

public class FightManager : Singleton<FightManager>
{
    [Header("角色属性")]
    public CharacterData_SO playerData;
    public CharacterData_SO enemyData;

    private PlayerController playerController;
    private EnemyController enemyController;

    [Header("战斗数据")]
    private float attackTime = 0;
    private float recodeTime = 0;
    private float changeTime = 0;
    private int enemyAttackNums = 0;
    private bool canAttack = true;
    private bool canChange = true;

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

    [Header("架势条")]
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

    void TurnToEnemy()
    {
        enemyAttackNums = enemyData.挥刀次数;   // 暂定
        attackTime = recodeTime = 0;
    }
    void TurnToPlayer()
    {
        playerAttackNums = playerData.挥刀次数;
        attackTime = recodeTime = 0;
    }
    #endregion

    #region------架势条系统------
    public void JudgeAnger()
    {
        JudgeEnemyAnger();
        JudgePlayerAnger(); 
    }
    public void JudgeEnemyAnger()
    {
        //if(enemyData.当前架势条>=enemyData.最大架势条)
        //{
        //    Time.timeScale = 0;
        //    rip.SetActive(true);
        //    enemyData.当前架势条 = 0;
        //}
    }
    public void JudgePlayerAnger()
    {
        //if(playerData.当前架势条>=playerData.最大架势条)
        //{

        //}
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
        playerData.当前架势条 = Mathf.Min(玩家架势条, (玩家架势条 / 10) + 敌人攻击力);
        playerData.当前生命值 = Mathf.Max(playerData.当前生命值 - 敌人攻击力, 0);
    }
    #endregion
}
