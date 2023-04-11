using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphabetController : Singleton<AlphabetController>
{
    [SerializeField]
    [Tooltip("�ַ�����")]
    GameObject alphabet_obj;
    [SerializeField]
    [Tooltip("�ַ��ı���ʾ")]
    private Text alphabet_text;
    [SerializeField]
    [Tooltip("�ж�����")]
    private GameObject active_area;
    [SerializeField]
    [Tooltip("�ַ����ж�����Ļ��Χ[-move_range, move_range]")]
    private float move_range = 330;
    [SerializeField]
    [Tooltip("��ɫ�����ж�����ʱ��")]
    private float player_attack_time;
    [SerializeField]
    [Tooltip("��ɫ�����ж�ʱ�����ж�����Ŀ���ת�����")]
    private int total_reverse_count;
    [SerializeField]
    [Tooltip("��Ҹ��ж�ʱ���ж��������λ�÷�Χ(0-1)")]
    private float[] active_area_range;

    // �ַ��ƶ��ٶȺͷ���
    private float ab_move_speed;
    private int ab_move_dir;
    // �ж������ƶ��ٶȺͷ���
    private float area_move_speed;
    private int area_move_dir;
    // �ַ��������Ʋ���
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
        // �ַ����ж��������趨�ٶ��ƶ�
        alphabet_obj.transform.localPosition += new Vector3((float)(ab_move_speed * ab_move_dir * Time.deltaTime), 0, 0);
        active_area.transform.localPosition += new Vector3((float)(area_move_speed * area_move_dir * Time.deltaTime), 0, 0);
        // ��Ҳ����ж�
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

        // �ַ��ڲ�ͬ�غϵĲ���
        // ��һغ�
        if (Round.Player == round)
        {
            // ����߽�ص�
            if (alphabet_obj.transform.localPosition.x < -move_range) { ab_move_dir = 1; }
            else if (alphabet_obj.transform.localPosition.x > move_range) { ab_move_dir = -1;}
            // �ж��������ص�����ת���¼Ҫ��1
            if (active_area.transform.localPosition.x < -move_range) {
                area_move_dir = 1;
                reverse_record += 1;
            }
            else if (active_area.transform.localPosition.x > move_range) {
                area_move_dir = -1;
                reverse_record += 1;
            }
            // �ж�������������
            float area_reverse_interval = player_attack_time / (total_reverse_count + 1);
            time_record += Time.deltaTime;
            Countdown.Instance.FreshCountDown(time_record, player_attack_time);
            if (time_record > area_reverse_interval * reverse_record)
            {
                area_move_dir = Random.Range(0, 2) * 2 - 1;
                reverse_record += 1;
            }
            // ����ʱ�䣬�ж�ʧ��
            if (time_record > player_attack_time) { RespondResult(-1); }
        }

        // ���˻غ�
        // �ַ���������ߣ��ж�ʧ��
        else if (Round.Enemy == round && alphabet_obj.transform.localPosition.x < -move_range) { RespondResult(-1); }

        // ��Ҵ����ڼ�
        // �ַ��������ұߣ��ж�ʧ��
        else if (Round.PlayerExecution == round && alphabet_obj.transform.localPosition.x > move_range) { RespondResult(-1); }
    }

    // ��FightManager���õĽӿ�
    public void StartWorking(Round _round, float _ab_move_speed, float _area_move_speed = 0)
    {
        // �����ַ���������ƶ��ٶ�
        ab_move_speed = _ab_move_speed;
        area_move_speed = _area_move_speed;
        // ���������ĸ
        alphabet = (Alphabet)Random.Range(0, 4);
        alphabet_text.text = alphabet.ToString();
        // ���ݵ�ǰ�����ķ��Ļغϣ����ò�ͬ���ַ��ƶ�����
        round = _round;
        if (Round.Player == round) { PlayerRound(); }
        else if (Round.Enemy == round) { EnemyRound(); }
        else if (Round.PlayerExecution == round) { PlayerExecution(); }
        // �ַ���ʼ�ƶ�
        is_working = true;
        alphabet_obj.SetActive(true);
        if (Round.PlayerExecution != round) { active_area.SetActive(true); }
    }

    private void PlayerRound()
    {
        // �ַ��������������
        alphabet_obj.transform.localPosition = new Vector3(Random.Range(-move_range, move_range), 0, 0);
        ab_move_dir = Random.Range(0, 2) * 2 - 1;
        // �ж���ҲҪ����ƶ�
        active_area.transform.localPosition = new Vector3(Random.Range(-move_range, move_range), 0, 0);
        area_move_dir = Random.Range(0, 2) * 2 - 1;
        time_record = 0;
        reverse_record = 1;
    }

    private void EnemyRound()
    {
        // �ַ����������ƶ�
        alphabet_obj.transform.localPosition = new Vector3(move_range, 0, 0);
        ab_move_dir = -1;
        // �ж����̶���ĳ��λ��
        active_area.transform.localPosition = new Vector3(Random.Range(move_range * (active_area_range[0] - 0.5f) * 2, move_range * (active_area_range[1] - 0.5f) * 2), 0, 0);
    }

    private void PlayerExecution()
    {
        // �ַ����������ƶ�
        alphabet_obj.transform.localPosition = new Vector3(-move_range, 0, 0);
        ab_move_dir = 1;
    }

    // ��Ӧ��������FightManager���������result���ַ����ж����ľ��룩
    private void RespondResult(float result)
    {
        FightManager.Instance.ReceiveResult(result, alphabet);
        // �ַ��ƶ����̽���
        is_working = false;
        alphabet_obj.SetActive(false);
        active_area.SetActive(false);
        // ����Ҵ����׶Σ���ռ�ʱ��
    }
}
