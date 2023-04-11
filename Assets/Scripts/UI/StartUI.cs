using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{


    public GameObject buttonpanel;
    public Text loadtext;


   public void StartGame()
    {
        StartCoroutine(LoadScene());
    }

    public void EndGame()
    {
        Application.Quit();
    }

    IEnumerator LoadScene()
    {
        buttonpanel.SetActive(false);
        loadtext.gameObject.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //���ر�������ֵ +1�ĳ���SampleScene
        operation.allowSceneActivation = false;
        //�Ȳ�������һ����
        while (!operation.isDone)
        {
            loadtext.text = "���ڼ���" + operation.progress * 100 + "%";

            if (operation.progress >= 0.9f)
            //���ڸ÷������������������Ҫ�ֶ�����ֵ��Ϊ100%
            {
                loadtext.text = "������ⰴť����";
                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                    //�������ⰴť��ʼ������һ����
                }
            }
            yield return null;
        }
    }
}
