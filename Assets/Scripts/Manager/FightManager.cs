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
    [Header("��ɫ����")]
    public CharacterData_SO playerData;
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
    [Tooltip("���˴����ܴ���")]
    public int enemyExecutionCount;
    [Tooltip("��Ҵ���ʱ�ַ��ƶ��ٶ�")]
    public float playerExeAlpMoveSpeed;
    [Tooltip("��ҹ���ʧ��ͣ��ʱ��")]
    public float attackFailWaitDuration;

    [Tooltip("������д")]
    public Animator executionShot;

    // ���Ƽ�¼����
    private int playerAttackCount;          // ��ҹ����ܴ���
    private int enemyAttackCount;           // ���˹����ܴ���
    private int playerAttackRecord = 0;     // ����ѹ�������
    private int enemyAttackRecord = 0;      // �����ѹ�������
    private float playerExcutionRecord = 0; // ��Ҵ����Ѿ���ʱ��
    private int enemyExecutionRecord = 0;   // �����Ѵ�������
    private bool stopExecution = false;     // ֹͣ������־λ
    Round round;        // ��¼��ǰ������һ�غϣ���ң����ˣ���Ҵ��������˴�����
    Stack<Round> next_rounds = new Stack<Round>();   // ��ʱ��¼������������Ļغ�
    int canEnterNextStep = 0;   // �ﵽ2ʱ����ҡ����˽�׼�����ʱ����������һС�غ�

    
    private float ab_move_speed = 300;        // �ַ��ƶ��ٶ�
    private float area_move_speed = 50;    // �ж������ƶ��ٶ�

    private float enemyExecutedDamage = 0;
    private float playerExecutedDamage = 0;

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

    private void Start()
    {
        // ��ʼ��
        playerData.��ǰ����ֵ = playerData.�������ֵ;
        enemyData.��ǰ����ֵ = enemyData.�������ֵ;
        playerData.��ǰ������ = 0;
        enemyData.��ǰ������ = 0;
        playerController = PlayerController.Instance;
        enemyController = EnemyController.Instance;

        // ��ʼ��һغ�
        TurnToPlayer();
        canEnterNextStep = 2;
    }

    private void Update()
    {
        if (Round.PlayerExecution == round && !stopExecution)
        {
            playerExcutionRecord += Time.deltaTime;
            // ������Ҵ���
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

    #region ------ս�������߼�-------
    void TurnToPlayer()
    {
        InitAttackRecord();
        round = Round.Player;
        playerAttackCount = playerData.�ӵ�����;
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
        enemyAttackCount = enemyData.�ӵ�����;   // �ݶ�
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

    // ������д��������ʼִ����Ӧ�߼�
    public void StartExecution()
    {
        round = next_rounds.Pop();
        NextStep();
    }

    bool JudgeHP()  // ������Ϸ�Ƿ����
    {
        if (playerData.��ǰ����ֵ <= 0)
        {
            FightOver(false);
            return true;
        }
        if (enemyData.��ǰ����ֵ <= 0)
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
        }
        else    // ��
        {
            playerController.Death();
        }
        Debug.Log("ս�����:" +  result);
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
    public bool PlayerAttackSuccess()
    {
        bool isEnemyBlock = Random.Range(0, 1.0f) < enemyData.BlockProb(playerData, enemyData) ? true : false;
        var ��Ҽ����� = playerData.��������;
        var ���˼����� = enemyData.��������;
        var ��ҹ����� = playerData.������;
        var ���˹����� = enemyData.������;
        if (isEnemyBlock)
        {
            enemyData.��ǰ������ = Mathf.Max(enemyData.��ǰ������, enemyData.��ǰ������ + ���˼����� / (10 - (��ҹ����� - ���˹�����)));
        }
        else
        {
            enemyData.��ǰ������ += ���˼����� / 10 + ��ҹ�����;
            enemyData.��ǰ����ֵ -= ��ҹ�����;
        }
        return isEnemyBlock;
    }
    public void EnemyExecuted()
    {
        var ��ҹ����� = playerData.������;
        enemyExecutedDamage += ��ҹ����� / 2;
    }
    public void PlayerPerfectBlock()
    {
        var ��Ҽ����� = playerData.��������;
        var ���˼����� = enemyData.��������;
        var ��ҹ����� = playerData.������;
        var ���˹����� = enemyData.������;
        enemyData.��ǰ������ = Mathf.Max(enemyData.��ǰ������, enemyData.��ǰ������ + ���˼����� / (15 - (��ҹ����� - ���˹�����)));
        playerData.��ǰ������ = Mathf.Min(��Ҽ����� - 1, playerData.��ǰ������ + ��Ҽ����� / 20);
    }
    public void PlayerNormalBlock()
    {
        var ��Ҽ����� = playerData.��������;
        var ���˼����� = enemyData.��������;
        var ��ҹ����� = playerData.������;
        var ���˹����� = enemyData.������;
        playerData.��ǰ������ = Mathf.Max(playerData.��ǰ������, playerData.��ǰ������ + ��Ҽ����� / (10 - (���˹����� - ��ҹ�����)));
    }
    public void PlayerNoBlock()
    {
        var ��Ҽ����� = playerData.��������;
        var ���˼����� = enemyData.��������;
        var ��ҹ����� = playerData.������;
        var ���˹����� = enemyData.������;
        playerData.��ǰ������ += (��Ҽ����� / 10) + ���˹�����;
        playerData.��ǰ����ֵ -= ���˹�����;
    }
    public void PlayerExecuted()
    {
        var ���˹����� = enemyData.������;
        playerExecutedDamage += ���˹����� * 2;
    }
    #endregion

    // ��AlphabetController���õĽӿ�
    public void ReceiveResult(float result, Alphabet ab)
    {
        int attackDir = alphabetToAttackDir[ab];
        if (Round.Player == round)
        {
            // �����ɹ�
            if (result <= attackSuccessGap)
            {
                playerController.Attack(attackDir, true);
                enemyController.Block(PlayerAttackSuccess());
            }
            // ����ʧ��
            else
            {
                StartCoroutine(WaitToNextStep(attackFailWaitDuration));
            }
        }
        else if (Round.Enemy == round)
        {
            // ������
            if (result < blockPerfectGap)
            {
                PlayerPerfectBlock();
                enemyController.Attack(attackDir, true);
                playerController.Block(true);
            }
            // ��ͨ��
            else if (result < blockSuccessGap)
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
            if (result > float.MaxValue - 10)
            {
                // ��Ҵ���ʧ�ܣ�ͣ��һ�½�����һ��
                WaitToNextStep(attackFailWaitDuration);
            }
            else
            {
                // ��Ҵ����ɹ�
                EnemyExecuted();
                playerController.Attack(attackDir, false);
            }
        }
    }

    // ������ǰС�غϣ�������һС�غ�
    public void NextStep()
    {
        // ������һС�غ�ǰ�����ж�������
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
                enemyData.��ǰ������ = 0;
                enemyData.��ǰ����ֵ -= enemyExecutedDamage;
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
                playerData.��ǰ������ = 0;
                playerData.��ǰ����ֵ -= playerExecutedDamage;
                InitExecutionRecord();
                stopExecution = false;
            }
            else
            {
                //playerController.SetAnimatorSpeed(0, "PlayerIdle", 0);
                // �������ַ��ж�ϵͳ��ֱ�ӵ��õ��˴���
                PlayerExecuted();
                enemyController.Attack(Random.Range(0, 3), false);
                // ��������
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
