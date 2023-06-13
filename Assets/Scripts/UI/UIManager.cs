using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : Singleton<UIManager>
{
    [Header("���ò˵�")]
    public GameObject SetPanel;
    [Header("����")]
    public Slider musicSlider;
    private float lastSlider;
    [Tooltip("�¼����ݻ��ܴ洢��������Ϣ")]
    public EventSO eventSO;
    [Tooltip("�̳����")]
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
        // ��ʼ����Ϸ����
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
    }   //������
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
