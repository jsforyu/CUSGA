using JetBrains.Annotations;
//using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : Singleton<FightManager>
{
    [Header("角色属性")]
    public CharacterData_SO playerData;
    public ItemData_SO skillData;
    public CharacterData_SO[] enemyDatas;
    public CharacterData_SO bossData;
    [NonSerialized]
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

    [Header("可控制对象")]
    [Tooltip("音效根节点")]
    public Transform sound;
    [Tooltip("处决成功判定音效")]
    public AudioSource playerExeSE;
    [Tooltip("事件数据 包含音量信息")]
    public EventSO eventSO;
    [Tooltip("背包数据")]
    public InventoryData_SO playerBag;
    [Tooltip("处决特写")]
    public Animator executionShot;
    [Tooltip("背景节点")]
    public SpriteRenderer background;
    [Tooltip("可选择背景图")]
    public Sprite[] background_sprites;
    [Tooltip("玩家出刀次数标记")]
    public RectTransform playerAttackIcon;
    [Tooltip("敌人出刀次数标记")]
    public RectTransform enemyAttackIcon;
    [Tooltip("玩家处决次数标记")]
    public GameObject playerExecutionIcon;
    private Image[] playerExecutionIconUnits;

    [Tooltip("玩家等级")]
    public Text playerLevel;
    [Tooltip("敌人等级")]
    public Text enemyLevel;

    [Header("音效")]
    [Tooltip("完美格挡")]
    public AudioSource perfectBlockAudio;
    [Tooltip("普通格挡")]
    public AudioSource normalBlockAudio;
    [Tooltip("格挡失败")]
    public AudioSource noBlockAudio;
    [Tooltip("处决特写")]
    public AudioSource exeShotAudio;
    [Tooltip("落叶")]
    public ParticleSystem[] leafParticleSystems;

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
    private float playerExecutionTimeRecord = 0; // 玩家处决已经过时间
    private int playerExecutionCountRecord = 0;  // 玩家处决成功次数
    private int enemyExecutionRecord = 0;   // 敌人已处决次数
    private bool stopExecution = false;     // 停止处决标志位
    Round round;        // 记录当前处于哪一回合（玩家，敌人，玩家处决，敌人处决）
    Stack<Round> next_rounds = new Stack<Round>();   // 临时记录接下来将进入的回合
    int canEnterNextStep = 0;   // 达到2时（玩家、敌人皆准备完毕时），进入下一小回合


    private float attack_icon_width = 62.5f;
    private float ab_move_speed = 400;        // 字符移动速度
    private float area_move_speed = 50;    // 判定区域移动速度
    private float enemy_move_speed;     //敌人字符移动速度

    private float enemyExecutedDamage = 0;
    private float playerExecutedDamage = 0;

    // 可选择的敌人
    public GameObject[] enemies;
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
        // 初始化音量
        foreach (AudioSource audio in sound.GetComponentsInChildren<AudioSource>())
        {
            audio.volume = eventSO.gameVolume;
        }
        // 随机敌人
        int random_enemy_index = UnityEngine.Random.Range(0, enemies.Length);
        enemies[random_enemy_index].SetActive(true);
        // 随机背景
        int random_bg_index = UnityEngine.Random.Range(0, background_sprites.Length);
        background.sprite = background_sprites[random_bg_index];
        // 初始化双方攻击/处决次数标记
        playerAttackIcon.sizeDelta = new Vector2(0, playerAttackIcon.sizeDelta.y);
        enemyAttackIcon.sizeDelta = new Vector2(0, enemyAttackIcon.sizeDelta.y);
        playerExecutionIconUnits = playerExecutionIcon.GetComponentsInChildren<Image>();

        // 初始化变量
        enemyData = enemyDatas[playerData.等级 - 1];
        if (eventSO.currentevent == 11) // 进入boss房
        {
            enemyData = bossData;
        }
        playerData.当前生命值 = playerData.最大生命值;
        enemyData.当前生命值 = enemyData.最大生命值;
        playerData.当前架势条 = 0;
        enemyData.当前架势条 = 0;
        playerController = PlayerController.Instance;
        enemyController = EnemyController.Instance;
        skillData = playerBag.currentJianJi;
        enemy_move_speed = enemyData.攻击符速度;

        // 初始化双方等级展示
        playerLevel.text = "Lv " + playerData.等级.ToString();
        enemyLevel.text = "Lv " + enemyData.等级.ToString();

        // 开始玩家回合
        TurnToPlayer();
        canEnterNextStep = 2;
    }

    private void Update()
    {
        // 测试员秘籍
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.G))
        {
            enemyData.当前生命值 = 0;
        }

        if (Round.PlayerExecution == round) // 玩家处决剩余时间记录
        {
            playerExecutionTimeRecord += Time.deltaTime;
            Countdown.Instance.FreshCountDown(Mathf.Min(playerExecutionTimeRecord, playerExecutionDuration), playerExecutionDuration);
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
        playerExecutionTimeRecord = 0;
        playerExecutionCountRecord = 0;
        enemyExecutionRecord = 0;
        enemyExecutedDamage = 0;
        playerExecutedDamage = 0;
    }

    IEnumerator WaitToNextStep(float second)
    {
        yield return new WaitForSeconds(second);
        canEnterNextStep = 2;
    }

    private void LeafSpeedControl(float multiplier)
    {
        foreach (var leafParticleSystem in leafParticleSystems)
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[leafParticleSystem.particleCount];

            // 获取粒子系统中所有粒子的信息
            int particleCount = leafParticleSystem.GetParticles(particles);

            // 修改已存在粒子速度
            for (int i = 0; i < particleCount; i++)
            {
                particles[i].velocity *= multiplier;  // 将当前速度乘以倍率
            }

            leafParticleSystem.SetParticles(particles, particleCount);

            // 修改粒子系统参数使后续粒子速度乘以倍率
            ParticleSystem.MainModule mainModule = leafParticleSystem.main;
            ParticleSystem.EmissionModule emissionModule = leafParticleSystem.emission;
            ParticleSystem.RotationOverLifetimeModule rotationModule = leafParticleSystem.rotationOverLifetime;
            ParticleSystem.ForceOverLifetimeModule forceModule = leafParticleSystem.forceOverLifetime;
            ParticleSystem.MinMaxCurve temp;
            // 开始速度
            temp = mainModule.startSpeed;
            temp.constantMin *= multiplier;
            temp.constantMax *= multiplier;
            mainModule.startSpeed = temp;
            // 发射速度
            temp = emissionModule.rateOverTime;
            temp.constant *= multiplier;
            emissionModule.rateOverTime = temp;
            // 旋转速度 x
            temp = rotationModule.x;
            temp.constantMin *= multiplier;
            temp.constantMax *= multiplier;
            rotationModule.x = temp;
            // 旋转速度 y
            temp = rotationModule.y;
            temp.constantMin *= multiplier;
            temp.constantMax *= multiplier;
            rotationModule.y = temp;
            // 旋转速度 z
            temp = rotationModule.z;
            temp.constantMin *= multiplier;
            temp.constantMax *= multiplier;
            rotationModule.z = temp;
            // 水平力
            temp = forceModule.x;
            temp.constantMin *= multiplier * multiplier;
            temp.constantMax *= multiplier * multiplier;
            forceModule.x = temp;
            // 垂直力
            temp = forceModule.y;
            temp.constantMin *= multiplier * multiplier;
            temp.constantMax *= multiplier * multiplier;
            forceModule.y = temp;
        }
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
        // 更新攻击次数标记
        playerAttackIcon.sizeDelta = new Vector2(playerAttackCount * attack_icon_width, playerAttackIcon.sizeDelta.y);
    }

    void TurnToPlayerExecution()
    {
        next_rounds.Clear();
        next_rounds.Push(round);
        next_rounds.Push(Round.PlayerExecution);
        round = Round.ExecutionShot;

        executionShot.SetInteger("PlayerRandom", UnityEngine.Random.Range(0, 4));
        executionShot.SetTrigger("PlayerExe");
        background.color = new Color(90 / 255f, 90 / 255f, 90 / 255f);
        LeafSpeedControl(0.25f);
        // 音效
        exeShotAudio.Play();

        // 打开处决记录Icon
        playerExecutionIcon.SetActive(true);
        foreach(var i in playerExecutionIconUnits)
        {
            i.color = new Color(135 / 255f, 135 / 255f, 135 / 255f);
        }
    }

    void TurnToEnemy()
    {
        InitAttackRecord();
        round = Round.Enemy;
        enemyAttackCount = enemyData.挥刀次数;
        playerController.TurnToStartPos();
        enemyController.TurnToStartPos();
        // 更新攻击次数标记
        enemyAttackIcon.sizeDelta = new Vector2(enemyAttackCount * attack_icon_width, enemyAttackIcon.sizeDelta.y);
    }

    void TurnToEnemyExecution()
    {
        next_rounds.Clear();
        next_rounds.Push(round);
        next_rounds.Push(Round.EnemyExecution);
        round = Round.ExecutionShot;

        executionShot.SetInteger("EnemyRandom", UnityEngine.Random.Range(0, 3));
        executionShot.SetTrigger("EnemyExe");
        background.color = new Color(90 / 255f, 90 / 255f, 90 / 255f);
        LeafSpeedControl(0.25f);
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
            FightResultUI.Instance.Settle(true);
        }
        else    // 输
        {
            playerController.Death();
            FightResultUI.Instance.Settle(false);
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
                AlphabetController.Instance.StartWorking(Round.Enemy, enemy_move_speed);
            }
            else
            {
                TurnToPlayer();
                NextStep();
            }
        }
        else if (Round.PlayerExecution == round)
        {
            if (playerExecutionTimeRecord >= playerExecutionDuration && playerExecutionCountRecord == 0)
            {
                stopExecution = true;
            }
            if (stopExecution)
            {
                round = next_rounds.Pop();
                enemyController.SetAnimatorSpeed(1);
                LeafSpeedControl(4);

                // 关闭处决记录Icon
                playerExecutionIcon.SetActive(false);
                playerExecutionCountRecord = 0;

                if (enemyExecutedDamage > EPS)   // 处决期间造成了伤害
                {
                    enemyController.Block(false);
                    canEnterNextStep++;
                    // 音效和震动
                    noBlockAudio.Play();
                    StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeFrequency, enemyAttackedMag));
                }
                else
                {
                    canEnterNextStep += 2;
                }
                enemyData.当前架势条 = 0;
                enemyData.当前生命值 -= enemyExecutedDamage;
                InitExecutionRecord();
                stopExecution = false;
                background.color = new Color(200 / 255f, 200 / 255f, 200 / 255f);
            }
            else
            {
                if (playerExecutionTimeRecord < playerExecutionDuration)
                {
                    enemyController.SetAnimatorSpeed(0.5f);
                    AlphabetController.Instance.StartWorking(Round.PlayerExecution, playerExeAlpMoveSpeed);
                }
                else
                {
                    if (playerExecutionCountRecord > 0)
                    {
                        EnemyExecuted();
                        playerController.Attack(UnityEngine.Random.Range(0, 3), false);
                        playerExecutionCountRecord--;
                    }
                }
            }
        }
        else if (Round.EnemyExecution == round)
        {
            if (stopExecution)
            {
                round = next_rounds.Pop();
                playerController.SetAnimatorSpeed(1);
                LeafSpeedControl(4);
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
                enemyController.Attack(UnityEngine.Random.Range(0, 3), false);
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
        bool isEnemyBlock = UnityEngine.Random.Range(0, 1.0f) < enemyData.BlockProb(playerData, enemyData) ? true : false;
        var 玩家架势条 = playerData.最大架势条;
        var 敌人架势条 = enemyData.最大架势条;
        var 玩家攻击力 = playerData.攻击力;
        var 招式攻击力 = 玩家攻击力 * skillData.attackMultiplier[skillIndex];
        var 敌人攻击力 = enemyData.攻击力;
        if (isEnemyBlock)
        {
            enemyData.当前架势条 = Mathf.Max(enemyData.当前架势条, enemyData.当前架势条 + 敌人架势条 / (10 - (招式攻击力 - 敌人攻击力)));
            enemyData.当前生命值 -= 招式攻击力 * 0.1f;
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
        playerData.当前架势条 -= skillData.angerRecovery;
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
            // 更新攻击次数标记
            playerAttackIcon.sizeDelta = new Vector2(playerAttackIcon.sizeDelta.x - attack_icon_width, playerAttackIcon.sizeDelta.y);
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
            // 更新攻击次数标记
            enemyAttackIcon.sizeDelta = new Vector2(enemyAttackIcon.sizeDelta.x - attack_icon_width, enemyAttackIcon.sizeDelta.y);
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
                // 玩家处决成功，记录次数
                if (playerExecutionCountRecord < playerExecutionIconUnits.Length)
                    playerExecutionIconUnits[playerExecutionCountRecord].color = new Color(140 / 255f, 44 / 255f, 43 / 255f);
                playerExecutionCountRecord++;
                canEnterNextStep += 2;
                // 判定处决成功的音效
                playerExeSE.Play();
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
