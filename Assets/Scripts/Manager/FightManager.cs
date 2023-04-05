using JetBrains.Annotations;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FightManager : Singleton<FightManager>
{
    [Header("角色属性")]
    public CharacterData_SO playerData;
    public CharacterData_SO enemyData;

    [Header("字符判定参数")]
    [Tooltip("完美格挡间隔距离")]
    public float blockPerfectGap;
    [Tooltip("普通格挡间隔距离")]
    public float blockSuccessGap;
    [Tooltip("攻击成功间隔距离")]
    public float attackSuccessGap;
    [Tooltip("处决成功间隔距离")]
    public float executionSuccessGap;

    [Header("控制参数")]
    [Tooltip("玩家处决总持续时间")]
    public float playerExecutionDuration;
    [Tooltip("敌人处决总次数")]
    public int enemyExecutionCount;
    [Tooltip("玩家处决时字符移动速度")]
    public float playerExeAlpMoveSpeed;
    [Tooltip("玩家攻击失败停顿时间")]
    public float attackFailWaitDuration;

    [Tooltip("处决特写")]
    public Animator executionShot;

    // 控制记录参数
    private int playerAttackCount;          // 玩家攻击总次数
    private int enemyAttackCount;           // 敌人攻击总次数
    private int playerAttackRecord = 0;     // 玩家已攻击次数
    private int enemyAttackRecord = 0;      // 敌人已攻击次数
    private float playerExcutionRecord = 0; // 玩家处决已经过时间
    private int enemyExecutionRecord = 0;   // 敌人已处决次数
    private bool stopExecution = false;     // 停止处决标志位
    Round round;        // 记录当前处于哪一回合（玩家，敌人，玩家处决，敌人处决）
    Stack<Round> next_rounds = new Stack<Round>();   // 临时记录接下来将进入的回合
    int canEnterNextStep = 0;   // 达到2时（玩家、敌人皆准备完毕时），进入下一小回合

    
    private float ab_move_speed = 300;        // 字符移动速度
    private float area_move_speed = 50;    // 判定区域移动速度

    private float enemyExecutedDamage = 0;
    private float playerExecutedDamage = 0;

    // 角色控制脚本
    private PlayerController playerController;
    private EnemyController enemyController;

    // Alphabet与动画攻击方向的对应关系
    Dictionary<Alphabet, int> alphabetToAttackDir = new Dictionary<Alphabet, int>()
    {
        { Alphabet.W, 0 },
        { Alphabet.D, 1 },
        { Alphabet.A, 1 },
        { Alphabet.S, 2 }
    };

    private void Start()
    {
        // 初始化
        playerData.当前生命值 = playerData.最大生命值;
        enemyData.当前生命值 = enemyData.最大生命值;
        playerData.当前架势条 = 0;
        enemyData.当前架势条 = 0;
        playerController = PlayerController.Instance;
        enemyController = EnemyController.Instance;

        // 开始玩家回合
        TurnToPlayer();
        canEnterNextStep = 2;
    }

    private void Update()
    {
        if (Round.PlayerExecution == round && !stopExecution)
        {
            playerExcutionRecord += Time.deltaTime;
            // 结束玩家处决
            if (playerExcutionRecord > playerExecutionDuration)
            {
                stopExecution = true;
            }
        }
        if (2 <= canEnterNextStep)
        {
            canEnterNextStep = 0;
            NextStep();
        }
    }

    private void InitAttackRecord()
    {
        playerAttackRecord = 0;
        enemyAttackRecord = 0;
    }

    private void InitExecutionRecord()
    {
        playerExcutionRecord = 0;
        enemyExecutionRecord = 0;
        enemyExecutedDamage = 0;
        playerExecutedDamage = 0;
    }

    #region ------战斗基本逻辑-------
    void TurnToPlayer()
    {
        InitAttackRecord();
        round = Round.Player;
        playerAttackCount = playerData.挥刀次数;
        playerController.TurnToStartPos();
        enemyController.TurnToStartPos();
    }

    void TurnToPlayerExecution()
    {
        next_rounds.Clear();
        next_rounds.Push(round);
        next_rounds.Push(Round.PlayerExecution);
        round = Round.ExecutionShot;

        executionShot.SetInteger("PlayerRandom", Random.Range(0, 4));
        executionShot.SetTrigger("PlayerExe");
    }

    void TurnToEnemy()
    {
        InitAttackRecord();
        round = Round.Enemy;
        enemyAttackCount = enemyData.挥刀次数;   // 暂定
        playerController.TurnToStartPos();
        enemyController.TurnToStartPos();
    }

    void TurnToEnemyExecution()
    {
        next_rounds.Clear();
        next_rounds.Push(round);
        next_rounds.Push(Round.EnemyExecution);
        round = Round.ExecutionShot;

        executionShot.SetInteger("EnemyRandom", Random.Range(0, 3));
        executionShot.SetTrigger("EnemyExe");
    }

    // 处决特写结束，开始执行相应逻辑
    public void StartExecution()
    {
        round = next_rounds.Pop();
        NextStep();
    }

    bool JudgeHP()  // 返回游戏是否结束
    {
        if (playerData.当前生命值 <= 0)
        {
            FightOver(false);
            return true;
        }
        if (enemyData.当前生命值 <= 0)
        {
            FightOver(true);
            return true;
        }
        return false;
    }

    void FightOver(bool result)
    {
        if (result) // 赢
        {
            enemyController.Death();
        }
        else    // 输
        {
            playerController.Death();
        }
        Debug.Log("战斗结果:" +  result);
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
        if (enemyData.当前架势条 >= enemyData.最大架势条)
        {
            // 暂定
            TurnToPlayerExecution();
        }
    }
    public void JudgePlayerAnger()
    {
        if (playerData.当前架势条 >= playerData.最大架势条)
        {
            // 暂定
            TurnToEnemyExecution();
        }
    }
    public bool PlayerAttackSuccess()
    {
        bool isEnemyBlock = Random.Range(0, 1.0f) < enemyData.BlockProb(playerData, enemyData) ? true : false;
        var 玩家架势条 = playerData.最大架势条;
        var 敌人架势条 = enemyData.最大架势条;
        var 玩家攻击力 = playerData.攻击力;
        var 敌人攻击力 = enemyData.攻击力;
        if (isEnemyBlock)
        {
            enemyData.当前架势条 = Mathf.Max(enemyData.当前架势条, enemyData.当前架势条 + 敌人架势条 / (10 - (玩家攻击力 - 敌人攻击力)));
        }
        else
        {
            enemyData.当前架势条 += 敌人架势条 / 10 + 玩家攻击力;
            enemyData.当前生命值 -= 玩家攻击力;
        }
        return isEnemyBlock;
    }
    public void EnemyExecuted()
    {
        var 玩家攻击力 = playerData.攻击力;
        enemyExecutedDamage += 玩家攻击力 / 2;
    }
    public void PlayerPerfectBlock()
    {
        var 玩家架势条 = playerData.最大架势条;
        var 敌人架势条 = enemyData.最大架势条;
        var 玩家攻击力 = playerData.攻击力;
        var 敌人攻击力 = enemyData.攻击力;
        enemyData.当前架势条 = Mathf.Max(enemyData.当前架势条, enemyData.当前架势条 + 敌人架势条 / (15 - (玩家攻击力 - 敌人攻击力)));
        playerData.当前架势条 = Mathf.Min(玩家架势条 - 1, playerData.当前架势条 + 玩家架势条 / 20);
    }
    public void PlayerNormalBlock()
    {
        var 玩家架势条 = playerData.最大架势条;
        var 敌人架势条 = enemyData.最大架势条;
        var 玩家攻击力 = playerData.攻击力;
        var 敌人攻击力 = enemyData.攻击力;
        playerData.当前架势条 = Mathf.Max(playerData.当前架势条, playerData.当前架势条 + 玩家架势条 / (10 - (敌人攻击力 - 玩家攻击力)));
    }
    public void PlayerNoBlock()
    {
        var 玩家架势条 = playerData.最大架势条;
        var 敌人架势条 = enemyData.最大架势条;
        var 玩家攻击力 = playerData.攻击力;
        var 敌人攻击力 = enemyData.攻击力;
        playerData.当前架势条 += (玩家架势条 / 10) + 敌人攻击力;
        playerData.当前生命值 -= 敌人攻击力;
    }
    public void PlayerExecuted()
    {
        var 敌人攻击力 = enemyData.攻击力;
        playerExecutedDamage += 敌人攻击力 * 2;
    }
    #endregion

    // 由AlphabetController调用的接口
    public void ReceiveResult(float result, Alphabet ab)
    {
        int attackDir = alphabetToAttackDir[ab];
        if (Round.Player == round)
        {
            // 攻击成功
            if (result <= attackSuccessGap)
            {
                playerController.Attack(attackDir, true);
                enemyController.Block(PlayerAttackSuccess());
            }
            // 攻击失败
            else
            {
                StartCoroutine(WaitToNextStep(attackFailWaitDuration));
            }
        }
        else if (Round.Enemy == round)
        {
            // 完美格挡
            if (result < blockPerfectGap)
            {
                PlayerPerfectBlock();
                enemyController.Attack(attackDir, true);
                playerController.Block(true);
            }
            // 普通格挡
            else if (result < blockSuccessGap)
            {
                PlayerNormalBlock();
                enemyController.Attack(attackDir, true);
                playerController.Block(true);
            }
            // 格挡失败
            else
            {
                PlayerNoBlock();
                enemyController.Attack(attackDir, true);
                playerController.Block(false);
            }
        }
        else if (Round.PlayerExecution == round)
        {
            if (result > float.MaxValue - 10)
            {
                // 玩家处决失败，停顿一下进入下一次
                WaitToNextStep(attackFailWaitDuration);
            }
            else
            {
                // 玩家处决成功
                EnemyExecuted();
                playerController.Attack(attackDir, false);
            }
        }
    }

    // 结束当前小回合，进入下一小回合
    public void NextStep()
    {
        // 进入下一小回合前，先判定架势条
        if (Round.Player == round || Round.Enemy == round)
        {
            if (JudgeHP()) { return; }
            JudgeAnger();
        }

        if (Round.Player == round)
        {
            if (playerAttackRecord < playerAttackCount)
            {
                playerAttackRecord++;
                AlphabetController.Instance.StartWorking(Round.Player, ab_move_speed, area_move_speed);
            }
            else
            {
                TurnToEnemy();
                NextStep();
            }
        }
        else if (Round.Enemy == round)
        {
            if (enemyAttackRecord < enemyAttackCount)
            {
                enemyAttackRecord++;
                AlphabetController.Instance.StartWorking(Round.Enemy, ab_move_speed);
            }
            else
            {
                TurnToPlayer();
                NextStep();
            }
        }
        else if (Round.PlayerExecution == round)
        {
            if (stopExecution)
            {
                round = next_rounds.Pop();
                //enemyController.SetAnimatorSpeed(0, "EnemyIdle", 1);
                enemyController.Block(false);
                canEnterNextStep++;
                enemyData.当前架势条 = 0;
                enemyData.当前生命值 -= enemyExecutedDamage;
                InitExecutionRecord();
                stopExecution = false;
            }
            else
            {
                //enemyController.SetAnimatorSpeed(0, "EnemyIdle", 0);
                AlphabetController.Instance.StartWorking(Round.PlayerExecution, playerExeAlpMoveSpeed);
            }
        }
        else if (Round.EnemyExecution == round)
        {
            if (stopExecution)
            {
                round = next_rounds.Pop();
                //playerController.SetAnimatorSpeed(0, "PlayerIdle", 1);
                playerController.Block(false);
                canEnterNextStep++;
                playerData.当前架势条 = 0;
                playerData.当前生命值 -= playerExecutedDamage;
                InitExecutionRecord();
                stopExecution = false;
            }
            else
            {
                //playerController.SetAnimatorSpeed(0, "PlayerIdle", 0);
                // 不调用字符判定系统，直接调用敌人处决
                PlayerExecuted();
                enemyController.Attack(Random.Range(0, 3), false);
                // 处决结束
                if (++enemyExecutionRecord >= enemyExecutionCount)
                {
                    stopExecution = true;
                }
            }
        }
    }

    IEnumerator WaitToNextStep(float second)
    {
        yield return new WaitForSeconds(second);
        canEnterNextStep = 2;
    }

    public void CharacterGetReady()
    {
        if (Round.Enemy == round || Round.Player == round)
        {
            canEnterNextStep++;
        }
        else if (Round.EnemyExecution == round || Round.PlayerExecution == round)
        {
            canEnterNextStep += 2;
        }
    }
}
