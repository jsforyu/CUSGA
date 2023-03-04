using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyAttackDir : MonoBehaviour
{
    private Vector3 startPostion;
    public KeyCode key;
    // Start is called before the first frame update
    private void Awake()
    {
        startPostion = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.x > 360)
        {
            //TODO:扣血
            gameObject.SetActive(false);
            Debug.Log("未成功按键");
        }
        transform.Translate(Vector3.right * FightManager.Instance.speed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.X))
        {
            if (Input.GetKeyDown(key))
            {
                if (transform.localPosition.x > FightManager.Instance.normalAlphas[0] && transform.localPosition.x < FightManager.Instance.normalAlphas[1])
                {
                    FightManager.Instance.playerAni.SetTrigger("Defence");
                    if (FightManager.Instance.bestAlphas[0] < transform.localPosition.x && transform.localPosition.x < FightManager.Instance.bestAlphas[1])
                    {
                        //TODO:少量增加主角架势条，增加敌人架势条
                        FightManager.Instance.PlayerPerfectBlock();
                        Debug.Log("完美按键");
                    }
                    else
                    {
                        //TODO:增加主角架势条
                        FightManager.Instance.PlayerNormalBlock();
                        Debug.Log("普通按键");
                    }
                    this.gameObject.SetActive(false);
                }
                else
                {
                    //TODO:扣血 增加架势条
                    FightManager.Instance.PlayerNoBlock();
                    Debug.Log("按键偏差过大");
                    gameObject.SetActive(false);
                }
            }
            else
            {
                //TODO:扣血 增加架势条
                FightManager.Instance.PlayerNoBlock();
                Debug.Log("按键不符合");
                gameObject.SetActive(false);
            }
        }
    }
    private void OnEnable()
    {
        transform.position = startPostion;  
    }
    private void OnDisable()
    {
        FightManager.Instance.canAttack = true;
    }
}
