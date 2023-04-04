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
        return FightManager.Instance.playerData.��ǰ����ֵ / FightManager.Instance.playerData.�������ֵ;
    }
    float ReturnPlayerAnger()
    {
        return FightManager.Instance.playerData.��ǰ������ / FightManager.Instance.playerData.��������;
    }
    float ReturnEnemyHealth()
    {
        return FightManager.Instance.enemyData.��ǰ����ֵ / FightManager.Instance.enemyData.�������ֵ;
    }
    float ReturnEnemyAnger()
    {
        return FightManager.Instance.enemyData.��ǰ������ / FightManager.Instance.playerData.��������;
    }
}
