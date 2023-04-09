using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [Header("设置菜单")]
    public GameObject SetPanel;
    [Header("音量")]
    public Slider musicSlider;
    private float lastSlider;
    public AudioSource[] audioSource;
    // Start is called before the first frame update
    private void Update()
    {
              ChangeMusic();
    }
    public void ChangeMusic()
    {
        if (lastSlider != musicSlider.value)
        {
            foreach (AudioSource source in audioSource)
            {
                source.volume = musicSlider.value;
            }
            lastSlider = musicSlider.value;
        }
    }   //调音量
    public void OpenSetting()
    {
        SetPanel.SetActive(true);
    }
    public void ReturnGame()
    {
        SetPanel.SetActive(false);
    }
    public void ReturnMain()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
