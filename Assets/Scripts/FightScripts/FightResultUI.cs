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
    string expTextString = "经验值：{0:D}/{1:D}(+{2:D})  等级：{3:D}->{4:D}";

    [SerializeField]
    Text jiadianText;
    string jiadianTextString = "待分配点数：{0:D}\r\n   体质：{1:D}->{2:D}\r\n   力量：{3:D}->{4:D}\r\n   敏捷：{5:D}->{6:D}\r\n   反应：{7:D}->{8:D}";
    [SerializeField]
    Button 体质加;
    [SerializeField]
    Button 体质减;
    [SerializeField]
    Button 力量加;
    [SerializeField]
    Button 力量减;
    [SerializeField]
    Button 敏捷加;
    [SerializeField]
    Button 敏捷减;
    [SerializeField]
    Button 反应加;
    [SerializeField]
    Button 反应减;

    [SerializeField]
    Text attributeText;
    [SerializeField]
    GameObject End;
    string attributeTextString = "生命值：{0}\r\n架势条：{1}\r\n攻击力：{2}\r\n攻击次数：{3}";

    CharacterData_SO playerData;
    CharacterData_SO enemyData;
    private int 原等级;
    private int 原体质;
    private int 原力量;
    private int 原敏捷;
    private int 原反应;

    private float 原生命值;
    private float 原架势条;
    private float 原攻击力;
    private int 原挥刀次数;

    // 战斗结果
    private bool result;

    // Start is called before the first frame update
    void Start()
    {
        playerData = FightManager.Instance.playerData;
        enemyData = FightManager.Instance.enemyData;

        原等级 = playerData.等级;
        原体质 = playerData.体质;
        原力量 = playerData.力量;
        原敏捷 = playerData.敏捷;
        原反应 = playerData.反应;

        原生命值 = playerData.最大生命值;
        原架势条 = playerData.最大架势条;
        原攻击力 = playerData.攻击力;
        原挥刀次数 = playerData.挥刀次数;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region --------按钮调用---------
    public void AttributePlus(string attribute)
    {
        if (attribute == "体质") { playerData.体质++; }
        if (attribute == "力量") { playerData.力量++; }
        if (attribute == "敏捷") { playerData.敏捷++; }
        if (attribute == "反应") { playerData.反应++; }
        playerData.剩余属性点--;
        if (playerData.剩余属性点 == 0) { exitBtn.interactable = true; }
        FreshAttributePanel();
    }
    public void AttributeMinus(string attribute)
    {
        if (attribute == "体质") { playerData.体质--; }
        if (attribute == "力量") { playerData.力量--; }
        if (attribute == "敏捷") { playerData.敏捷--; }
        if (attribute == "反应") { playerData.反应--; }
        playerData.剩余属性点++;
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
            // 记录事件完成
            eventSO.events[eventSO.currentevent] = true;
            // 获得剑技
            if (eventSO.accessibleSkill != null)
            {
                inventoryDataSO.AddItem(eventSO.accessibleSkill, 1);
            }
        }
        SaveManager.Instance.SavePlayerData();
        SceneManager.LoadSceneAsync(1);
    }
    #endregion

    // 内部调用
    void FreshAttributePanel()  // 更新属性加点面板
    {
        jiadianText.text = string.Format(jiadianTextString, playerData.剩余属性点, 原体质, playerData.体质, 原力量, playerData.力量, 原敏捷, playerData.敏捷, 原反应, playerData.反应);
        // 加点带来的属性变化，采用富文字标记变化的属性
        string 生命值 = string.Format("<color={0}>{1}</color>", (原生命值 == playerData.最大生命值 ? "black" : "green"), playerData.最大生命值);
        string 架势条 = string.Format("<color={0}>{1}</color>", (原架势条 == playerData.最大架势条 ? "black" : "green"), playerData.最大架势条);
        string 攻击力 = string.Format("<color={0}>{1}</color>", (原攻击力 == playerData.攻击力 ? "black" : "green"), playerData.攻击力);
        string 挥刀次数 = string.Format("<color={0}>{1}</color>", (原挥刀次数 == playerData.挥刀次数 ? "black" : "green"), playerData.挥刀次数);
        attributeText.text = string.Format(attributeTextString, 生命值, 架势条, 攻击力, 挥刀次数);

        // 按钮开关
        if (playerData.剩余属性点 > 0)
        {
            体质加.interactable = true;
            力量加.interactable = true;
            敏捷加.interactable = true;
            反应加.interactable = true;
        }
        else
        {
            体质加.interactable = false;
            力量加.interactable = false;
            敏捷加.interactable = false;
            反应加.interactable = false;
        }
        if (原体质 < playerData.体质) { 体质减.interactable = true; }
        else { 体质减.interactable = false; }
        if (原力量 < playerData.力量) { 力量减.interactable = true; }
        else { 力量减.interactable = false; }
        if (原敏捷 < playerData.敏捷) { 敏捷减.interactable = true; }
        else { 敏捷减.interactable = false; }
        if (原反应 < playerData.反应) { 反应减.interactable = true; }
        else { 反应减.interactable = false; }

        if (playerData.剩余属性点 > 0) { exitBtn.interactable = false; }
        else { exitBtn.interactable = true; }
    }

    // 外部接口
    public void Settle(bool result_)
    {
        result = result_;
        if (result && eventSO.currentevent == 11)   // 击败boss
        {
            End.SetActive(true);
            // 清除数据
            PlayerPrefs.DeleteAll();
            return;
        }

        // 打开面板
        settlementUI.SetActive(true);

        // 根据战斗结果执行逻辑
        if (result) // 赢
        {
            winIcon.SetActive(true);

            // 获得经验值
            playerData.获得经验值(enemyData.经验值);
            expText.text = string.Format(expTextString, playerData.经验值, playerData.升级所需经验值, enemyData.经验值, 原等级, playerData.等级);

            FreshAttributePanel();

            // 可获得剑技
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
