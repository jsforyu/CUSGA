using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BasicController
{
    public CharacterData_SO characterData;
    public PlayerStats playerStats=PlayerStats.Defence;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        FightManager.Instance.playerData = characterData;
        FightManager.Instance.playerController = this;
        FightManager.Instance.playerAni = ani;
    }
    private void Update()
    {
        switch (playerStats)
        {
            case PlayerStats.Attack:
                ReturnToAttackPos(FightManager.Instance.enemyContorller.startPos);
                break;  
            case PlayerStats.Defence:
                ReturnToStartPos();
                break;
        }
    }
}
