using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public KeyCode key;
    public float attackTime=5f;
    private float time;
    private bool isRight;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= attackTime) 
        {
            Debug.Log("连招时间过了");
            gameObject.SetActive(false);
            FightManager.Instance.currentIndex = 0;
        }
        if (transform.localPosition.x > 360) { isRight = false; }
        if (transform.localPosition.x < -326) { isRight = true; }   
        if(isRight)
        {
            transform.Translate(Vector3.right * FightManager.Instance.speed * Time.deltaTime);
        }
        else { transform.Translate(Vector3.left * FightManager.Instance.speed * Time.deltaTime); }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.X))
        {
            if (Input.GetKeyDown(key))
            {
                if (FightManager.Instance.bestAlphas[0] < transform.localPosition.x && transform.localPosition.x < FightManager.Instance.bestAlphas[1])
                {
                    Debug.Log("成功连招");
                    gameObject.SetActive(false);
                }
                else { Debug.Log("连招失败"); FightManager.Instance.currentIndex = 0; gameObject.SetActive(false);  }
            }
        }
    }
    private void OnEnable()
    {
        time = 0;
        int x = Random.Range(-160, 360);
        transform.localPosition=new Vector3(x,-3,0);
        float y = Random.Range(0f, 1f);
        if (y>=0.5)
        {
            isRight = true;
        }
        else isRight= false;
    }
    private void OnDisable()
    {
        FightManager.Instance.canAttack = true;
    }
}
