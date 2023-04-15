using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FightResultUI : Singleton<FightResultUI>
{
    [SerializeField]
    EventSO eventSO;
    [SerializeField]
    InventoryData_SO inventoryDataSO;
    [SerializeField]
    GameObject settlementUI;
    [SerializeField]
    GameObject winIcon;
    [SerializeField]
    GameObject loseIcon;
    [SerializeField]
    Button exitBtn;
    [SerializeField]
    AudioSource btnClickSE;
    [SerializeField]
    GameObject accessibleSkill;
    [SerializeField]
    GameObject gottenObject_1;

    [SerializeField]
    Text expText;
    string expTextString = "����ֵ��{0:D}/{1:D}(+{2:D})  �ȼ���{3:D}->{4:D}";

    [SerializeField]
    Text jiadianText;
    string jiadianTextString = "�����������{0:D}\r\n   ���ʣ�{1:D}->{2:D}\r\n   ������{3:D}->{4:D}\r\n   ���ݣ�{5:D}->{6:D}\r\n   ��Ӧ��{7:D}->{8:D}";
    [SerializeField]
    Button ���ʼ�;
    [SerializeField]
    Button ���ʼ�;
    [SerializeField]
    Button ������;
    [SerializeField]
    Button ������;
    [SerializeField]
    Button ���ݼ�;
    [SerializeField]
    Button ���ݼ�;
    [SerializeField]
    Button ��Ӧ��;
    [SerializeField]
    Button ��Ӧ��;

    [SerializeField]
    Text attributeText;
    [SerializeField]
    GameObject End;
    string attributeTextString = "����ֵ��{0}\r\n��������{1}\r\n��������{2}\r\n����������{3}";

    CharacterData_SO playerData;
    CharacterData_SO enemyData;
    private int ԭ�ȼ�;
    private int ԭ����;
    private int ԭ����;
    private int ԭ����;
    private int ԭ��Ӧ;

    private float ԭ����ֵ;
    private float ԭ������;
    private float ԭ������;
    private int ԭ�ӵ�����;

    // ս�����
    private bool result;

    // Start is called before the first frame update
    void Start()
    {
        playerData = FightManager.Instance.playerData;
        enemyData = FightManager.Instance.enemyData;

        ԭ�ȼ� = playerData.�ȼ�;
        ԭ���� = playerData.����;
        ԭ���� = playerData.����;
        ԭ���� = playerData.����;
        ԭ��Ӧ = playerData.��Ӧ;

        ԭ����ֵ = playerData.�������ֵ;
        ԭ������ = playerData.��������;
        ԭ������ = playerData.������;
        ԭ�ӵ����� = playerData.�ӵ�����;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region --------��ť����---------
    public void AttributePlus(string attribute)
    {
        if (attribute == "����") { playerData.����++; }
        if (attribute == "����") { playerData.����++; }
        if (attribute == "����") { playerData.����++; }
        if (attribute == "��Ӧ") { playerData.��Ӧ++; }
        playerData.ʣ�����Ե�--;
        if (playerData.ʣ�����Ե� == 0) { exitBtn.interactable = true; }
        FreshAttributePanel();
    }
    public void AttributeMinus(string attribute)
    {
        if (attribute == "����") { playerData.����--; }
        if (attribute == "����") { playerData.����--; }
        if (attribute == "����") { playerData.����--; }
        if (attribute == "��Ӧ") { playerData.��Ӧ--; }
        playerData.ʣ�����Ե�++;
        exitBtn.interactable = false;
        FreshAttributePanel();
    }

    public void Exit()
    {
        StartCoroutine(loadSceneWithSound());
    }

    IEnumerator loadSceneWithSound()
    {
        btnClickSE.Play();
        yield return new WaitForSeconds(btnClickSE.clip.length);
        if (result)
        {
            // ��¼�¼����
            eventSO.events[eventSO.currentevent] = true;
            // ��ý���
            if (eventSO.accessibleSkill != null)
            {
                inventoryDataSO.AddItem(eventSO.accessibleSkill, 1);
            }
        }
        SaveManager.Instance.SavePlayerData();
        SceneManager.LoadSceneAsync(1);
    }
    #endregion

    // �ڲ�����
    void FreshAttributePanel()  // �������Լӵ����
    {
        jiadianText.text = string.Format(jiadianTextString, playerData.ʣ�����Ե�, ԭ����, playerData.����, ԭ����, playerData.����, ԭ����, playerData.����, ԭ��Ӧ, playerData.��Ӧ);
        // �ӵ���������Ա仯�����ø����ֱ�Ǳ仯������
        string ����ֵ = string.Format("<color={0}>{1}</color>", (ԭ����ֵ == playerData.�������ֵ ? "black" : "green"), playerData.�������ֵ);
        string ������ = string.Format("<color={0}>{1}</color>", (ԭ������ == playerData.�������� ? "black" : "green"), playerData.��������);
        string ������ = string.Format("<color={0}>{1}</color>", (ԭ������ == playerData.������ ? "black" : "green"), playerData.������);
        string �ӵ����� = string.Format("<color={0}>{1}</color>", (ԭ�ӵ����� == playerData.�ӵ����� ? "black" : "green"), playerData.�ӵ�����);
        attributeText.text = string.Format(attributeTextString, ����ֵ, ������, ������, �ӵ�����);

        // ��ť����
        if (playerData.ʣ�����Ե� > 0)
        {
            ���ʼ�.interactable = true;
            ������.interactable = true;
            ���ݼ�.interactable = true;
            ��Ӧ��.interactable = true;
        }
        else
        {
            ���ʼ�.interactable = false;
            ������.interactable = false;
            ���ݼ�.interactable = false;
            ��Ӧ��.interactable = false;
        }
        if (ԭ���� < playerData.����) { ���ʼ�.interactable = true; }
        else { ���ʼ�.interactable = false; }
        if (ԭ���� < playerData.����) { ������.interactable = true; }
        else { ������.interactable = false; }
        if (ԭ���� < playerData.����) { ���ݼ�.interactable = true; }
        else { ���ݼ�.interactable = false; }
        if (ԭ��Ӧ < playerData.��Ӧ) { ��Ӧ��.interactable = true; }
        else { ��Ӧ��.interactable = false; }

        if (playerData.ʣ�����Ե� > 0) { exitBtn.interactable = false; }
        else { exitBtn.interactable = true; }
    }

    // �ⲿ�ӿ�
    public void Settle(bool result_)
    {
        result = result_;
        if (result && eventSO.currentevent == 11)   // ����boss
        {
            End.SetActive(true);
            // �������
            PlayerPrefs.DeleteAll();
            return;
        }

        // �����
        settlementUI.SetActive(true);

        // ����ս�����ִ���߼�
        if (result) // Ӯ
        {
            winIcon.SetActive(true);

            // ��þ���ֵ
            playerData.��þ���ֵ(enemyData.����ֵ);
            expText.text = string.Format(expTextString, playerData.����ֵ, playerData.�������辭��ֵ, enemyData.����ֵ, ԭ�ȼ�, playerData.�ȼ�);

            FreshAttributePanel();

            // �ɻ�ý���
            if (eventSO.accessibleSkill != null)
            {
                GameObject temp = Instantiate(accessibleSkill);
                temp.transform.parent = gottenObject_1.transform;
                temp.transform.localPosition = new Vector3(0, 1.4f, 0);
                temp.transform.localScale = new Vector3(6.043f, 6.043f, 0);

                temp.GetComponent<AccessibleSkillUI>().InitItem(eventSO.accessibleSkill);
            }
        }
        else
        {
            loseIcon.SetActive(true);

            expText.text = "";

            jiadianText.gameObject.SetActive(false);
            attributeText.text = "";

            exitBtn.interactable = true;
        }
    }
}
