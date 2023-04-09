using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRip : Singleton<EnemyRip>
{
    public GameObject 判定点;
    public float existTime;
    public float currentTime;
    public GameObject[] 攻击方向;
    public bool canAttack=true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.unscaledDeltaTime;
        if(currentTime <= existTime&&canAttack) 
        {
                canAttack= false;
            int temp = Random.Range(0, 5);
            攻击方向[temp].SetActive(true); 
        }
    }
    private void OnEnable()
    {
        currentTime = 0;
        判定点.SetActive(false);
    }
    private void OnDisable()
    {
        判定点.SetActive(true);
    }
}
