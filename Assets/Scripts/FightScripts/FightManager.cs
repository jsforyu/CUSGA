using JetBrains.Annotations;
//using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : Singleton<FightManager>
{
    [Header("��ɫ����")]
    public CharacterData_SO playerData;
    public ItemData_SO skillData;
    public CharacterData_SO[] enemyDatas;
    public CharacterData_SO bossData;
    [NonSerialized]
    public CharacterData_SO enemyData;

    [Header("�ַ��ж�����")]
    [Tooltip("�����񵲼������")]
    public float blockPerfectGap;
    [Tooltip("��ͨ�񵲼������")]
    public float blockSuccessGap;
    [Tooltip("�����ɹ��������")]
    public float attackSuccessGap;
    [Tooltip("�����ɹ��������")]
    public float executionSuccessGap;

    [Header("���Ʋ���")]
    [Tooltip("��Ҵ����ܳ���ʱ��")]
    public float playerExecutionDuration;
    [Tooltip("��Ҵ����˺�ϵ��")]
    public float playerExecutionCoef;
    [Tooltip("���˴����ܴ���")]
    public int enemyExecutionCount;
    [Tooltip("���˴����˺�ϵ��")]
    public float enemyExecutionCoef;
    [Tooltip("��Ҵ���ʱ�ַ��ƶ��ٶ�")]
    public float playerExeAlpMoveSpeed;
    [Tooltip("��ҹ���ʧ��ͣ��ʱ��")]
    public float attackFailWaitDuration;

    [Header("�ɿ��ƶ���")]
    [Tooltip("��Ч���ڵ�")]
    public Transform sound;
    [Tooltip("�����ɹ��ж���Ч")]
    public AudioSource playerExeSE;
    [Tooltip("�¼����� ����������Ϣ")]
    public EventSO eventSO;
    [Tooltip("��������")]
    public InventoryData_SO playerBag;
    [Tooltip("������д")]
    public Animator executionShot;
    [Tooltip("�����ڵ�")]
    public SpriteRenderer background;
    [Tooltip("��ѡ�񱳾�ͼ")]
    public Sprite[] background_sprites;
    [Tooltip("��ҳ����������")]
    public RectTransform playerAttackIcon;
    [Tooltip("���˳����������")]
    public RectTransform enemyAttackIcon;
    [Tooltip("��Ҵ����������")]
    public GameObject playerExecutionIcon;
    private Image[] playerExecutionIconUnits;

    [Tooltip("��ҵȼ�")]
    public Text playerLevel;
    [Tooltip("���˵ȼ�")]
    public Text enemyLevel;

    [Header("��Ч")]
    [Tooltip("������")]
    public AudioSource perfectBlockAudio;
    [Tooltip("��ͨ��")]
    public AudioSource normalBlockAudio;
    [Tooltip("��ʧ��")]
    public AudioSource noBlockAudio;
    [Tooltip("������д")]
    public AudioSource exeShotAudio;
    [Tooltip("��Ҷ")]
    public ParticleSystem[] leafParticleSystems;

    [Header("��ͷ��")]
    [Tooltip("��ʱ��")]
    public float shakeDuration = 0.15f;
    [Tooltip("��Ƶ��")]
    public float shakeFrequency = 10f;
    [Tooltip("�������𶯷���")]
    public float perfectBlockMag = 0.1f;
    [Tooltip("��ͨ���𶯷���")]
    public float normalBlockMag = 0.15f;
    [Tooltip("����ܻ��𶯷���")]
    public float playerAttackedMag = 0.2f;
    [Tooltip("�����ܻ��𶯷���")]
    public float enemyAttackedMag = 0.25f;

    // ���Ƽ�¼����
    private int skillIndex = 0;             // ��ǰ���ڽ����ڼ�ʽ
    private int playerAttackCount;          // ��ҹ����ܴ���
    private int enemyAttackCount;           // ���˹����ܴ���
    private int playerAttackRecord = 0;     // ����ѹ�������
    private int enemyAttackRecord = 0;      // �����ѹ�������
    private float playerExecutionTimeRecord = 0; // ��Ҵ����Ѿ���ʱ��
    private int playerExecutionCountRecord = 0;  // ��Ҵ����ɹ�����
    private int enemyExecutionRecord = 0;   // �����Ѵ�������
    private bool stopExecution = false;     // ֹͣ������־λ
    Round round;        // ��¼��ǰ������һ�غϣ���ң����ˣ���Ҵ��������˴�����
    Stack<Round> next_rounds = new Stack<Round>();   // ��ʱ��¼������������Ļغ�
    int canEnterNextStep = 0;   // �ﵽ2ʱ����ҡ����˽�׼�����ʱ����������һС�غ�


    private float attack_icon_width = 62.5f;
    private float ab_move_speed = 400;        // �ַ��ƶ��ٶ�
    private float area_move_speed = 50;    // �ж������ƶ��ٶ�
    private float enemy_move_speed;     //�����ַ��ƶ��ٶ�

    private float enemyExecutedDamage = 0;
    private float playerExecutedDamage = 0;

    // ��ѡ��ĵ���
    public GameObject[] enemies;
    // ��ɫ���ƽű�
    private PlayerController playerController;
    private EnemyController enemyController;

    // Alphabet�붯����������Ķ�Ӧ��ϵ
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
        // ��ʼ������
        foreach (AudioSource audio in sound.GetComponentsInChildren<AudioSource>())
        {
            audio.volume = eventSO.gameVolume;
        }
        // �������
        int random_enemy_index = UnityEngine.Random.Range(0, enemies.Length);
        enemies[random_enemy_index].SetActive(true);
        // �������
        int random_bg_index = UnityEngine.Random.Range(0, background_sprites.Length);
        background.sprite = background_sprites[random_bg_index];
        // ��ʼ��˫������/�����������
        playerAttackIcon.sizeDelta = new Vector2(0, playerAttackIcon.sizeDelta.y);
        enemyAttackIcon.sizeDelta = new Vector2(0, enemyAttackIcon.sizeDelta.y);
        playerExecutionIconUnits = playerExecutionIcon.GetComponentsInChildren<Image>();

        // ��ʼ������
        enemyData = enemyDatas[playerData.�ȼ� - 1];
        if (eventSO.currentevent == 11) // ����boss��
        {
            enemyData = bossData;
        }
        playerData.��ǰ����ֵ = playerData.�������ֵ;
        enemyData.��ǰ����ֵ = enemyData.�������ֵ;
        playerData.��ǰ������ = 0;
        enemyData.��ǰ������ = 0;
        playerController = PlayerController.Instance;
        enemyController = EnemyController.Instance;
        skillData = playerBag.currentJianJi;
        enemy_move_speed = enemyData.�������ٶ�;

        // ��ʼ��˫���ȼ�չʾ
        playerLevel.text = "Lv " + playerData.�ȼ�.ToString();
        enemyLevel.text = "Lv " + enemyData.�ȼ�.ToString();

        // ��ʼ��һغ�
        TurnToPlayer();
        canEnterNextStep = 2;
    }

    private void Update()
    {
        // ����Ա�ؼ�
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.G))
        {
            enemyData.��ǰ����ֵ = 0;
        }

        if (Round.PlayerExecution == round) // ��Ҵ���ʣ��ʱ���¼
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

    #region -------��������--------
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

            // ��ȡ����ϵͳ���������ӵ���Ϣ
            int particleCount = leafParticleSystem.GetParticles(particles);

            // �޸��Ѵ��������ٶ�
            for (int i = 0; i < particleCount; i++)
            {
                particles[i].velocity *= multiplier;  // ����ǰ�ٶȳ��Ա���
            }

            leafParticleSystem.SetParticles(particles, particleCount);

            // �޸�����ϵͳ����ʹ���������ٶȳ��Ա���
            ParticleSystem.MainModule mainModule = leafParticleSystem.main;
            ParticleSystem.EmissionModule emissionModule = leafParticleSystem.emission;
            ParticleSystem.RotationOverLifetimeModule rotationModule = leafParticleSystem.rotationOverLifetime;
            ParticleSystem.ForceOverLifetimeModule forceModule = leafParticleSystem.forceOverLifetime;
            ParticleSystem.MinMaxCurve temp;
            // ��ʼ�ٶ�
            temp = mainModule.startSpeed;
            temp.constantMin *= multiplier;
            temp.constantMax *= multiplier;
            mainModule.startSpeed = temp;
            // �����ٶ�
            temp = emissionModule.rateOverTime;
            temp.constant *= multiplier;
            emissionModule.rateOverTime = temp;
            // ��ת�ٶ� x
            temp = rotationModule.x;
            temp.constantMin *= multiplier;
            temp.constantMax *= multiplier;
            rotationModule.x = temp;
            // ��ת�ٶ� y
            temp = rotationModule.y;
            temp.constantMin *= multiplier;
            temp.constantMax *= multiplier;
            rotationModule.y = temp;
            // ��ת�ٶ� z
            temp = rotationModule.z;
            temp.constantMin *= multiplier;
            temp.constantMax *= multiplier;
            rotationModule.z = temp;
            // ˮƽ��
            temp = forceModule.x;
            temp.constantMin *= multiplier * multiplier;
            temp.constantMax *= multiplier * multiplier;
            forceModule.x = temp;
            // ��ֱ��
            temp = forceModule.y;
            temp.constantMin *= multiplier * multiplier;
            temp.constantMax *= multiplier * multiplier;
            forceModule.y = temp;
        }
    }
    #endregion

    #region ------ս�������߼�-------
    void TurnToPlayer()
    {
        InitAttackRecord();
        round = Round.Player;
        playerAttackCount = playerData.�ӵ�����;
        playerController.TurnToStartPos();
        enemyController.TurnToStartPos();
        // ���¹����������
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
        // ��Ч
        exeShotAudio.Play();

        // �򿪴�����¼Icon
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
        enemyAttackCount = enemyData.�ӵ�����;
        playerController.TurnToStartPos();
        enemyController.TurnToStartPos();
        // ���¹����������
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
        // ��Ч
        exeShotAudio.Play();
    }

    // ������д��������ʼִ����Ӧ�߼�
    public void StartExecution()
    {
        round = next_rounds.Pop();
        NextStep();
    }

    bool JudgeHP()  // ������Ϸ�Ƿ����
    {
        if (playerData.��ǰ����ֵ <= EPS)
        {
            FightOver(false);
            return true;
        }
        if (enemyData.��ǰ����ֵ <= EPS)
        {
            FightOver(true);
            return true;
        }
        return false;
    }

    void FightOver(bool result)
    {
        if (result) // Ӯ
        {
            enemyController.Death();
            FightResultUI.Instance.Settle(true);
        }
        else    // ��
        {
            playerController.Death();
            FightResultUI.Instance.Settle(false);
        }
        Debug.Log("ս�����:" +  result);
    }

    // ������ǰС�غϣ�������һС�غ�
    private void NextStep()
    {
        // ������һС�غ�ǰ�����ж�������
        if (Round.Player == round || Round.Enemy == round)
        {
            if (JudgeHP()) { return; }
            JudgeAnger();
        }
        // ��ռ�ʱ��
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

                // �رմ�����¼Icon
                playerExecutionIcon.SetActive(false);
                playerExecutionCountRecord = 0;

                if (enemyExecutedDamage > EPS)   // �����ڼ�������˺�
                {
                    enemyController.Block(false);
                    canEnterNextStep++;
                    // ��Ч����
                    noBlockAudio.Play();
                    StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeFrequency, enemyAttackedMag));
                }
                else
                {
                    canEnterNextStep += 2;
                }
                enemyData.��ǰ������ = 0;
                enemyData.��ǰ����ֵ -= enemyExecutedDamage;
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
                playerData.��ǰ������ = 0;
                playerData.��ǰ����ֵ -= playerExecutedDamage;
                InitExecutionRecord();
                stopExecution = false;
                background.color = new Color(200 / 255f, 200 / 255f, 200 / 255f);
                // ��Ч����
                noBlockAudio.Play();
                StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeFrequency, playerAttackedMag));
            }
            else
            {
                playerController.SetAnimatorSpeed(0.5f);
                // �������ַ��ж�ϵͳ��ֱ�ӵ��õ��˴���
                PlayerExecuted();
                enemyController.Attack(UnityEngine.Random.Range(0, 3), false);
                // ��������
                if (++enemyExecutionRecord >= enemyExecutionCount)
                {
                    stopExecution = true;
                }
            }
        }
    }
    #endregion

    #region------������ϵͳ------
    public void JudgeAnger()
    {
        JudgeEnemyAnger();
        JudgePlayerAnger(); 
    }
    public void JudgeEnemyAnger()
    {
        if (enemyData.��ǰ������ >= enemyData.��������)
        {
            // �ݶ�
            TurnToPlayerExecution();
        }
    }
    public void JudgePlayerAnger()
    {
        if (playerData.��ǰ������ >= playerData.��������)
        {
            // �ݶ�
            TurnToEnemyExecution();
        }
    }
    public bool PlayerAttack()
    {
        bool isEnemyBlock = UnityEngine.Random.Range(0, 1.0f) < enemyData.BlockProb(playerData, enemyData) ? true : false;
        var ��Ҽ����� = playerData.��������;
        var ���˼����� = enemyData.��������;
        var ��ҹ����� = playerData.������;
        var ��ʽ������ = ��ҹ����� * skillData.attackMultiplier[skillIndex];
        var ���˹����� = enemyData.������;
        if (isEnemyBlock)
        {
            enemyData.��ǰ������ = Mathf.Max(enemyData.��ǰ������, enemyData.��ǰ������ + ���˼����� / (10 - (��ʽ������ - ���˹�����)));
            enemyData.��ǰ����ֵ -= ��ʽ������ * 0.1f;
        }
        else
        {
            enemyData.��ǰ������ += ���˼����� / 10 + ��ʽ������;
            enemyData.��ǰ����ֵ -= ��ʽ������;
        }
        // ��Ч����
        if (isEnemyBlock)
        {
            normalBlockAudio.Play();
            StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeFrequency, normalBlockMag));
        }
        else
        {
            // ����δ����ʱ��ִ����ʽ���ܴ��е�Ѫ���ָ�Ч��
            playerData.��ǰ����ֵ += ��ҹ����� * skillData.healMultiplier[skillIndex];
            if (playerData.��ǰ����ֵ >= playerData.�������ֵ) { playerData.��ǰ����ֵ = playerData.�������ֵ; }

            noBlockAudio.Play();
            StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeFrequency, enemyAttackedMag));
        }
        // �������ָ�
        playerData.��ǰ������ -= skillData.angerRecovery;
        if (playerData.��ǰ������ < EPS) { playerData.��ǰ������ = 0; }
        // ������һ��ʽ
        skillIndex = (skillIndex + 1) % skillData.attackMultiplier.Length;
        // ���ص��˸񵲽��
        return isEnemyBlock;
    }
    public void EnemyExecuted()
    {
        var ��ҹ����� = playerData.������;
        enemyExecutedDamage += ��ҹ����� * playerExecutionCoef;

        // ��Ч
        noBlockAudio.Play();
    }
    public void PlayerPerfectBlock()
    {
        var ��Ҽ����� = playerData.��������;
        var ���˼����� = enemyData.��������;
        var ��ҹ����� = playerData.������;
        var ���˹����� = enemyData.������;
        enemyData.��ǰ������ = Mathf.Max(enemyData.��ǰ������, enemyData.��ǰ������ + ���˼����� / (15 - (��ҹ����� - ���˹�����)));
        playerData.��ǰ������ = Mathf.Min(��Ҽ����� - 1, playerData.��ǰ������ + ��Ҽ����� / 20);
        // ��Ч����
        perfectBlockAudio.Play();
        StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeFrequency, perfectBlockMag));
    }
    public void PlayerNormalBlock()
    {
        var ��Ҽ����� = playerData.��������;
        var ���˼����� = enemyData.��������;
        var ��ҹ����� = playerData.������;
        var ���˹����� = enemyData.������;
        playerData.��ǰ������ = Mathf.Max(playerData.��ǰ������, playerData.��ǰ������ + ��Ҽ����� / (10 - (���˹����� - ��ҹ�����)));
        // ��Ч����
        normalBlockAudio.Play();
        StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeFrequency, normalBlockMag));
    }
    public void PlayerNoBlock()
    {
        var ��Ҽ����� = playerData.��������;
        var ���˼����� = enemyData.��������;
        var ��ҹ����� = playerData.������;
        var ���˹����� = enemyData.������;
        playerData.��ǰ������ += (��Ҽ����� / 10) + ���˹�����;
        playerData.��ǰ����ֵ -= ���˹�����;
        // ��Ч����
        noBlockAudio.Play();
        StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeFrequency, playerAttackedMag));
    }
    public void PlayerExecuted()
    {
        var ���˹����� = enemyData.������;
        playerExecutedDamage += ���˹����� * enemyExecutionCoef;
        // ��Ч
        noBlockAudio.Play();
    }
    #endregion

    #region --------�ⲿ�ӿ�---------
    // ��AlphabetController���õĽӿ�
    public void ReceiveResult(float result, Alphabet ab)
    {
        int attackDir = alphabetToAttackDir[ab];
        if (Round.Player == round)
        {
            // ���¹����������
            playerAttackIcon.sizeDelta = new Vector2(playerAttackIcon.sizeDelta.x - attack_icon_width, playerAttackIcon.sizeDelta.y);
            // �����ж��ɹ�
            if (result <= attackSuccessGap && result >= 0)
            {
                playerController.Attack(attackDir, true);
                enemyController.Block(PlayerAttack());
            }
            // �����ж�ʧ��
            else
            {
                StartCoroutine(WaitToNextStep(attackFailWaitDuration));
            }
        }
        else if (Round.Enemy == round)
        {
            // ���¹����������
            enemyAttackIcon.sizeDelta = new Vector2(enemyAttackIcon.sizeDelta.x - attack_icon_width, enemyAttackIcon.sizeDelta.y);
            // ������
            if (result <= blockPerfectGap && result >= 0)
            {
                PlayerPerfectBlock();
                enemyController.Attack(attackDir, true);
                playerController.Block(true);
            }
            // ��ͨ��
            else if (result <= blockSuccessGap && result >= 0)
            {
                PlayerNormalBlock();
                enemyController.Attack(attackDir, true);
                playerController.Block(true);
            }
            // ��ʧ��
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
                // ��Ҵ���ʧ�ܣ�ͣ��һ�½�����һ��
                StartCoroutine(WaitToNextStep(attackFailWaitDuration));
            }
            else
            {
                // ��Ҵ����ɹ�����¼����
                if (playerExecutionCountRecord < playerExecutionIconUnits.Length)
                    playerExecutionIconUnits[playerExecutionCountRecord].color = new Color(140 / 255f, 44 / 255f, 43 / 255f);
                playerExecutionCountRecord++;
                canEnterNextStep += 2;
                // �ж������ɹ�����Ч
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
