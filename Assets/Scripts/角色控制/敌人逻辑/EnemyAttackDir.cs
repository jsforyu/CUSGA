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
            //TODO:��Ѫ
            gameObject.SetActive(false);
            Debug.Log("δ�ɹ�����");
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
                        //TODO:�����������Ǽ����������ӵ��˼�����
                        FightManager.Instance.PlayerPerfectBlock();
                        Debug.Log("��������");
                    }
                    else
                    {
                        //TODO:�������Ǽ�����
                        FightManager.Instance.PlayerNormalBlock();
                        Debug.Log("��ͨ����");
                    }
                    this.gameObject.SetActive(false);
                }
                else
                {
                    //TODO:��Ѫ ���Ӽ�����
                    FightManager.Instance.PlayerNoBlock();
                    Debug.Log("����ƫ�����");
                    gameObject.SetActive(false);
                }
            }
            else
            {
                //TODO:��Ѫ ���Ӽ�����
                FightManager.Instance.PlayerNoBlock();
                Debug.Log("����������");
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
