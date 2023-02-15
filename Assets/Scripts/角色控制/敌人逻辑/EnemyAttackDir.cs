using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackDir : MonoBehaviour
{
    public KeyCode key;
    public float AttackTime;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(key))
        {
            FightManager.Instance.playerAni.SetTrigger("Defence");
            FightManager.Instance.enemyAni.SetTrigger("Defence");
            this.gameObject.SetActive(false);
            FightManager.Instance.canAttack = true;
        }
    }
    private void OnEnable()
    {
        time = 0;
    }
    private void OnDisable()
    {
        time = 0;
    }
}
