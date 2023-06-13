using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : Singleton<UIManager>
{
    [Header("设置菜单")]
    public GameObject SetPanel;
    [Header("音量")]
    public Slider musicSlider;
    private float lastSlider;
    [Tooltip("事件数据汇总存储有音量信息")]
    public EventSO eventSO;
    [Tooltip("教程面板")]
    public GameObject jiaocheng;

    public AudioSource walkAudioSource;
    public AudioSource btnAudioSource;
    public AudioSource bgAudioSource;
    // Start is called before the first frame update

    private void Start()
    {
        InitVolume();
    }

    void InitVolume()
    {
        // 初始化游戏音量
        if (eventSO != null)
        {
            musicSlider.value = lastSlider = eventSO.gameVolume;
            walkAudioSource.volume = musicSlider.value;
            btnAudioSource.volume = musicSlider.value;
            bgAudioSource.volume = musicSlider.value;
        }
    }

    private void Update()
    {
        ChangeMusic();
    }
    public void ChangeMusic()
    {
        if (lastSlider != musicSlider.value)
        {
            walkAudioSource.volume = musicSlider.value;
            btnAudioSource.volume = musicSlider.value;
            bgAudioSource.volume = musicSlider.value;
            lastSlider = musicSlider.value;
            eventSO.gameVolume = musicSlider.value;
            SaveManager.Instance.SavePlayerData();
        }
    }   //调音量
    public void OpenSetting()
    {
        SetPanel.SetActive(true);
        btnAudioSource.Play();
    }
    public void ReturnGame()
    {
        SetPanel.SetActive(false);
        btnAudioSource.Play();
    }
    public void ReturnMain()
    {
        StartCoroutine(ReturnMainEnum());
    }
    IEnumerator ReturnMainEnum()
    {
        btnAudioSource.Play();
        yield return new WaitForSeconds(btnAudioSource.clip.length);
        SceneManager.LoadSceneAsync(0);
    }

    public void OpenJiaocheng()
    {

        jiaocheng.SetActive(true);
        btnAudioSource.Play();
    }

    public void CloseJiaocheng()
    {
        jiaocheng.SetActive(false);
        btnAudioSource.Play();
    }
}
