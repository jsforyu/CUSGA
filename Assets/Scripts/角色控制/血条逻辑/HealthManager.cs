using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthManager : Singleton<HealthManager>
{
    public Image playerHealth;
    public Image enemyHealth;
    public Image playerAnger;
    public Image enemyAnger;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        ChangeFill();
    }
    public void ChangeFill()
    {
        playerHealth.fillAmount = ReturnPlayerHealth();
        playerAnger.fillAmount = ReturnPlayerAnger();
        enemyHealth.fillAmount = ReturnEnemyHealth();
        enemyAnger.fillAmount = ReturnEnemyAnger();
    }
    float ReturnPlayerHealth()
    {
        return FightManager.Instance.playerData.当前生命值 / FightManager.Instance.playerData.最大生命值;
    }
    float ReturnPlayerAnger()
    {
        return FightManager.Instance.playerData.当前架势条 / FightManager.Instance.playerData.最大架势条;
    }
    float ReturnEnemyHealth()
    {
        return FightManager.Instance.enemyData.当前生命值 / FightManager.Instance.enemyData.最大生命值;
    }
    float ReturnEnemyAnger()
    {
        return FightManager.Instance.enemyData.当前架势条 / FightManager.Instance.playerData.最大架势条;
    }
}
