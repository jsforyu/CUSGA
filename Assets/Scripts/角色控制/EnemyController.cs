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
    }
    private void OnEnable()
    {
        FightManager.Instance.enemyData = characterData;
        FightManager.Instance.enemyContorller = this;
    }
    private void Update()
    {
        switch (enemyStats)
        {
            case EnemyStats.Attack:
                ReturnToAttackPos(FightManager.Instance.playerController.gameObject.transform.position);
                break;
            case EnemyStats.Defence:
                ReturnToStartPos();
                break;
        }
    }
}
