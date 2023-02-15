using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BasicController
{
    public CharacterData_SO tempData;
    public CharacterData_SO characterData;
    public EnemyStats enemyStats=EnemyStats.Defence;
    protected override void Awake()
    {
        base.Awake();
        characterData = Instantiate(tempData);
        characterData.敏捷 = 10;
        SetAnimatorSpeed(0, "Enemy_Attack", 1 / characterData.攻击速度);
        SetAnimatorSpeed(0, "Enemy_Defence", 1 / characterData.攻击速度);
    }
    private void Start()
    {
        FightManager.Instance.enemyData = characterData;
        FightManager.Instance.enemyContorller = this;
        FightManager.Instance.enemyAni = ani;
    }
    private void Update()
    {
        switch (enemyStats)
        {
            case EnemyStats.Attack:
                ReturnToAttackPos(FightManager.Instance.playerController.startPos);
                break;
            case EnemyStats.Defence:
                ReturnToStartPos();
                break;
        }
    }
}
