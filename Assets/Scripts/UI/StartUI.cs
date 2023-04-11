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
        //加载本场景数值 +1的场景SampleScene
        operation.allowSceneActivation = false;
        //先不加载下一场景
        while (!operation.isDone)
        {
            loadtext.text = "正在加载" + operation.progress * 100 + "%";

            if (operation.progress >= 0.9f)
            //由于该方法本身的问题所以需要手动把数值改为100%
            {
                loadtext.text = "点击任意按钮进入";
                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                    //按下任意按钮开始加载下一场景
                }
            }
            yield return null;
        }
    }
}
