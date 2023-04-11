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
    public ItemData_SO skillData;
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
    [Tooltip("玩家处决伤害系数")]
    public float playerExecutionCoef;
    [Tooltip("敌人处决总次数")]
    public int enemyExecutionCount;
    [Tooltip("敌人处决伤害系数")]
    public float enemyExecutionCoef;
    [Tooltip("玩家处决时字符移动速度")]
    public float playerExeAlpMoveSpeed;
    [Tooltip("玩家攻击失败停顿时间")]
    public float attackFailWaitDuration;

    [Tooltip("处决特写")]
    public Animator executionShot;
    [Tooltip("背景")]
    public SpriteRenderer background;

    [Header("音效")]
    [Tooltip("完美格挡")]
    public AudioSource perfectBlockAudio;
    [Tooltip("普通格挡")]
    public AudioSource normalBlockAudio;
    [Tooltip("格挡失败")]
    public AudioSource noBlockAudio;
    [Tooltip("处决特写")]
    public AudioSource exeShotAudio;

    [Header("镜头震动")]
    [Tooltip("震动时间")]
    public float shakeDuration = 0.15f;
    [Tooltip("震动频率")]
    public float shakeFrequency = 10f;
    [Tooltip("完美格挡震动幅度")]
    public float perfectBlockMag = 0.1f;
    [Tooltip("普通格挡震动幅度")]
    public float normalBlockMag = 0.15f;
    [Tooltip("玩家受击震动幅度")]
    public float playerAttackedMag = 0.2f;
    [Tooltip("敌人受击震动幅度")]
    public float enemyAttackedMag = 0.25f;

    // 控制记录参数
    private int skillIndex = 0;             // 当前处于剑技第几式
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

    
    private float ab_move_speed = 400;        // 字符移动速度
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

    const float EPS = 0.001f;

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
            Countdown.Instance.FreshCountDown(playerExcutionRecord, playerExecutionDuration);
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

    #region -------辅助方法--------
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

    IEnumerator WaitToNextStep(float second)
    {
        yield return new WaitForSeconds(second);
        canEnterNextStep = 2;
    }
    #endregion

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
        background.color = new Color(90 / 255f, 90 / 255f, 90 / 255f);
        // 音效
        exeShotAudio.Play();
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
        background.color = new Color(90 / 255f, 90 / 255f, 90 / 255f);
        // 音效
        exeShotAudio.Play();
    }

    // 处决特写结束，开始执行相应逻辑
    public void StartExecution()
    {
        round = next_rounds.Pop();
        NextStep();
    }

    bool JudgeHP()  // 返回游戏是否结束
    {
        if (playerData.当前生命值 <= EPS)
        {
            FightOver(false);
            return true;
        }
        if (enemyData.当前生命值 <= EPS)
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

    // 结束当前小回合，进入下一小回合
    private void NextStep()
    {
        // 进入下一小回合前，先判定架势条
        if (Round.Player == round || Round.Enemy == round)
        {
            if (JudgeHP()) { return; }
            JudgeAnger();
        }
        // 清空计时条
        if (Round.PlayerExecution != round) { Countdown.Instance.FreshCountDown(1, 1); }

        if (Round.Player == round)
        {
            if (playerAttackRecord < playerAttackCount)
            {
                playerAttackRecord++;
                AlphabetController.Instance.StartWorking(Round.Player, ab_move_speed * skillData.abSpeedMultiplier[skillIndex], area_move_speed);
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
                enemyController.SetAnimatorSpeed(1);
                if (enemyExecutedDamage > EPS)   // 处决期间造成了伤害
                {
                    enemyController.Block(false);
                }
                canEnterNextStep++;
                enemyData.当前架势条 = 0;
                enemyData.当前生命值 -= enemyExecutedDamage;
                InitExecutionRecord();
                stopExecution = false;
                background.color = new Color(200 / 255f, 200 / 255f, 200 / 255f);
                // 音效和震动
                noBlockAudio.Play();
                StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeFrequency, enemyAttackedMag));
            }
            else
            {
                enemyController.SetAnimatorSpeed(0.5f);
                AlphabetController.Instance.StartWorking(Round.PlayerExecution, playerExeAlpMoveSpeed);
            }
        }
        else if (Round.EnemyExecution == round)
        {
            if (stopExecution)
            {
                round = next_rounds.Pop();
                playerController.SetAnimatorSpeed(1);
                playerController.Block(false);
                canEnterNextStep++;
                playerData.当前架势条 = 0;
                playerData.当前生命值 -= playerExecutedDamage;
                InitExecutionRecord();
                stopExecution = false;
                background.color = new Color(200 / 255f, 200 / 255f, 200 / 255f);
                // 音效和震动
                noBlockAudio.Play();
                StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeFrequency, playerAttackedMag));
            }
            else
            {
                playerController.SetAnimatorSpeed(0.5f);
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
    public bool PlayerAttack()
    {
        bool isEnemyBlock = Random.Range(0, 1.0f) < enemyData.BlockProb(playerData, enemyData) ? true : false;
        var 玩家架势条 = playerData.最大架势条;
        var 敌人架势条 = enemyData.最大架势条;
        var 玩家攻击力 = playerData.攻击力;
        var 招式攻击力 = 玩家攻击力 * skillData.attackMultiplier[skillIndex];
        var 敌人攻击力 = enemyData.攻击力;
        if (isEnemyBlock)
        {
            enemyData.当前架势条 = Mathf.Max(enemyData.当前架势条, enemyData.当前架势条 + 敌人架势条 / (10 - (招式攻击力 - 敌人攻击力)));
        }
        else
        {
            enemyData.当前架势条 += 敌人架势条 / 10 + 招式攻击力;
            enemyData.当前生命值 -= 招式攻击力;
        }
        // 音效和震动
        if (isEnemyBlock)
        {
            normalBlockAudio.Play();
            StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeFrequency, normalBlockMag));
        }
        else
        {
            // 攻击未被格挡时，执行招式可能带有的血量恢复效果
            playerData.当前生命值 += 玩家攻击力 * skillData.healMultiplier[skillIndex];
            if (playerData.当前生命值 >= playerData.最大生命值) { playerData.当前生命值 = playerData.最大生命值; }

            noBlockAudio.Play();
            StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeFrequency, enemyAttackedMag));
        }
        // 架势条恢复
        playerData.当前架势条 -= skillData.angerRecovery[skillIndex];
        if (playerData.当前架势条 < EPS) { playerData.当前架势条 = 0; }
        // 进入下一招式
        skillIndex = (skillIndex + 1) % skillData.attackMultiplier.Length;
        // 返回敌人格挡结果
        return isEnemyBlock;
    }
    public void EnemyExecuted()
    {
        var 玩家攻击力 = playerData.攻击力;
        enemyExecutedDamage += 玩家攻击力 * playerExecutionCoef;
        // 音效
        noBlockAudio.Play();
    }
    public void PlayerPerfectBlock()
    {
        var 玩家架势条 = playerData.最大架势条;
        var 敌人架势条 = enemyData.最大架势条;
        var 玩家攻击力 = playerData.攻击力;
        var 敌人攻击力 = enemyData.攻击力;
        enemyData.当前架势条 = Mathf.Max(enemyData.当前架势条, enemyData.当前架势条 + 敌人架势条 / (15 - (玩家攻击力 - 敌人攻击力)));
        playerData.当前架势条 = Mathf.Min(玩家架势条 - 1, playerData.当前架势条 + 玩家架势条 / 20);
        // 音效和震动
        perfectBlockAudio.Play();
        StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeFrequency, perfectBlockMag));
    }
    public void PlayerNormalBlock()
    {
        var 玩家架势条 = playerData.最大架势条;
        var 敌人架势条 = enemyData.最大架势条;
        var 玩家攻击力 = playerData.攻击力;
        var 敌人攻击力 = enemyData.攻击力;
        playerData.当前架势条 = Mathf.Max(playerData.当前架势条, playerData.当前架势条 + 玩家架势条 / (10 - (敌人攻击力 - 玩家攻击力)));
        // 音效和震动
        normalBlockAudio.Play();
        StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeFrequency, normalBlockMag));
    }
    public void PlayerNoBlock()
    {
        var 玩家架势条 = playerData.最大架势条;
        var 敌人架势条 = enemyData.最大架势条;
        var 玩家攻击力 = playerData.攻击力;
        var 敌人攻击力 = enemyData.攻击力;
        playerData.当前架势条 += (玩家架势条 / 10) + 敌人攻击力;
        playerData.当前生命值 -= 敌人攻击力;
        // 音效和震动
        noBlockAudio.Play();
        StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeFrequency, playerAttackedMag));
    }
    public void PlayerExecuted()
    {
        var 敌人攻击力 = enemyData.攻击力;
        playerExecutedDamage += 敌人攻击力 * enemyExecutionCoef;
        // 音效
        noBlockAudio.Play();
    }
    #endregion

    #region --------外部接口---------
    // 由AlphabetController调用的接口
    public void ReceiveResult(float result, Alphabet ab)
    {
        int attackDir = alphabetToAttackDir[ab];
        if (Round.Player == round)
        {
            // 攻击判定成功
            if (result <= attackSuccessGap && result >= 0)
            {
                playerController.Attack(attackDir, true);
                enemyController.Block(PlayerAttack());
            }
            // 攻击判定失败
            else
            {
                StartCoroutine(WaitToNextStep(attackFailWaitDuration));
            }
        }
        else if (Round.Enemy == round)
        {
            // 完美格挡
            if (result <= blockPerfectGap && result >= 0)
            {
                PlayerPerfectBlock();
                enemyController.Attack(attackDir, true);
                playerController.Block(true);
            }
            // 普通格挡
            else if (result <= blockSuccessGap && result >= 0)
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
            if (result == -1)
            {
                // 玩家处决失败，停顿一下进入下一次
                StartCoroutine(WaitToNextStep(attackFailWaitDuration));
            }
            else
            {
                // 玩家处决成功
                EnemyExecuted();
                playerController.Attack(attackDir, false);
            }
        }
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
    #endregion
}
