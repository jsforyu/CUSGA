using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BasicController
{
    public CharacterData_SO characterData;
    public PlayerStats playerStats=PlayerStats.Defence;
    private void Start()
    { 
    }
    protected override void Awake()
    {
        base.Awake();
    }
    private void Update()
    {
          switch(playerStats)
        {
            case PlayerStats.Attack:
                ReturnToAttackPos(FightManager.Instance.enemyContorller.gameObject.transform.position);
                break;  
            case PlayerStats.Defence:
                ReturnToStartPos();
                break;
        }
    }

    private void OnEnable()
    {
        FightManager.Instance.playerData = characterData;
        FightManager.Instance.playerController = this;
    }
}
