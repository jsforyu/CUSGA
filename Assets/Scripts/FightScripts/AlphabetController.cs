using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphabetController : Singleton<AlphabetController>
{
    [SerializeField]
    [Tooltip("字符对象")]
    GameObject alphabet_obj;
    [SerializeField]
    [Tooltip("字符文本显示")]
    private Text alphabet_text;
    [SerializeField]
    [Tooltip("判定区域")]
    private GameObject active_area;
    [SerializeField]
    [Tooltip("字符和判定区域的活动范围[-move_range, move_range]")]
    private float move_range = 330;
    [SerializeField]
    [Tooltip("角色攻击判定持续时间")]
    private float player_attack_time;
    [SerializeField]
    [Tooltip("角色攻击判定时间内判定区域的可能转向次数")]
    private int total_reverse_count;
    [SerializeField]
    [Tooltip("玩家格挡判定时的判定区域随机位置范围(0-1)")]
    private float[] active_area_range;

    // 字符移动速度和方向
    private float ab_move_speed;
    private int ab_move_dir;
    // 判定区域移动速度和方向
    private float area_move_speed;
    private int area_move_dir;
    // 字符工作控制参数
    private bool is_working = false;
    private Round round;
    private Alphabet alphabet;
    private float time_record = 0;
    private int reverse_record = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_working) { return; }
        // 字符和判定区域按照设定速度移动
        alphabet_obj.transform.localPosition += new Vector3((float)(ab_move_speed * ab_move_dir * Time.deltaTime), 0, 0);
        active_area.transform.localPosition += new Vector3((float)(area_move_speed * area_move_dir * Time.deltaTime), 0, 0);
        // 玩家操作判定
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Alphabet.D == alphabet) { RespondResult(Mathf.Abs(alphabet_obj.transform.localPosition.x - active_area.transform.localPosition.x)); }
            else { RespondResult(-1); }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (Alphabet.W == alphabet) { RespondResult(Mathf.Abs(alphabet_obj.transform.localPosition.x - active_area.transform.localPosition.x)); }
            else { RespondResult(-1); }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (Alphabet.A == alphabet) { RespondResult(Mathf.Abs(alphabet_obj.transform.localPosition.x - active_area.transform.localPosition.x)); }
            else { RespondResult(-1); }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (Alphabet.S == alphabet) { RespondResult(Mathf.Abs(alphabet_obj.transform.localPosition.x - active_area.transform.localPosition.x)); }
            else { RespondResult(-1); }
        }

        // 字符在不同回合的策略
        // 玩家回合
        if (Round.Player == round)
        {
            // 到达边界回弹
            if (alphabet_obj.transform.localPosition.x < -move_range) { ab_move_dir = 1; }
            else if (alphabet_obj.transform.localPosition.x > move_range) { ab_move_dir = -1;}
            // 判定区域若回弹，则转向记录要加1
            if (active_area.transform.localPosition.x < -move_range) {
                area_move_dir = 1;
                reverse_record += 1;
            }
            else if (active_area.transform.localPosition.x > move_range) {
                area_move_dir = -1;
                reverse_record += 1;
            }
            // 判定区域的随机反向
            float area_reverse_interval = player_attack_time / (total_reverse_count + 1);
            time_record += Time.deltaTime;
            Countdown.Instance.FreshCountDown(time_record, player_attack_time);
            if (time_record > area_reverse_interval * reverse_record)
            {
                area_move_dir = Random.Range(0, 2) * 2 - 1;
                reverse_record += 1;
            }
            // 超过时间，判定失败
            if (time_record > player_attack_time) { RespondResult(-1); }
        }

        // 敌人回合
        // 字符到达最左边，判定失败
        else if (Round.Enemy == round && alphabet_obj.transform.localPosition.x < -move_range) { RespondResult(-1); }

        // 玩家处决期间
        // 字符到达最右边，判定失败
        else if (Round.PlayerExecution == round && alphabet_obj.transform.localPosition.x > move_range) { RespondResult(-1); }
    }

    // 由FightManager调用的接口
    public void StartWorking(Round _round, float _ab_move_speed, float _area_move_speed = 0)
    {
        // 设置字符和区域的移动速度
        ab_move_speed = _ab_move_speed;
        area_move_speed = _area_move_speed;
        // 生成随机字母
        alphabet = (Alphabet)Random.Range(0, 4);
        alphabet_text.text = alphabet.ToString();
        // 根据当前属于哪方的回合，调用不同的字符移动规则
        round = _round;
        if (Round.Player == round) { PlayerRound(); }
        else if (Round.Enemy == round) { EnemyRound(); }
        else if (Round.PlayerExecution == round) { PlayerExecution(); }
        // 字符开始移动
        is_working = true;
        alphabet_obj.SetActive(true);
        if (Round.PlayerExecution != round) { active_area.SetActive(true); }
    }

    private void PlayerRound()
    {
        // 字符随机起点随机方向
        alphabet_obj.transform.localPosition = new Vector3(Random.Range(-move_range, move_range), 0, 0);
        ab_move_dir = Random.Range(0, 2) * 2 - 1;
        // 判定区也要随机移动
        active_area.transform.localPosition = new Vector3(Random.Range(-move_range, move_range), 0, 0);
        area_move_dir = Random.Range(0, 2) * 2 - 1;
        time_record = 0;
        reverse_record = 1;
    }

    private void EnemyRound()
    {
        // 字符从右向左移动
        alphabet_obj.transform.localPosition = new Vector3(move_range, 0, 0);
        ab_move_dir = -1;
        // 判定区固定在某个位置
        active_area.transform.localPosition = new Vector3(Random.Range(move_range * (active_area_range[0] - 0.5f) * 2, move_range * (active_area_range[1] - 0.5f) * 2), 0, 0);
    }

    private void PlayerExecution()
    {
        // 字符从左向右移动
        alphabet_obj.transform.localPosition = new Vector3(-move_range, 0, 0);
        ab_move_dir = 1;
    }

    // 响应按键，向FightManager反馈结果（result即字符和判定区的距离）
    private void RespondResult(float result)
    {
        FightManager.Instance.ReceiveResult(result, alphabet);
        // 字符移动过程结束
        is_working = false;
        alphabet_obj.SetActive(false);
        active_area.SetActive(false);
        // 非玩家处决阶段，清空计时条
    }
}
