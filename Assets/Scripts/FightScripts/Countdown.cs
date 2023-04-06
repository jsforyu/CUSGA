using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : Singleton<Countdown>
{
    [SerializeField]
    private Image image;
    public Sprite[] countdown_sprites;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FreshCountDown(float cur_time, float total_time)
    {
        // cur_time == total_time时为计时结束，即cur_time递增
        int index = (int)(cur_time / total_time * (countdown_sprites.Length - 1));
        if (index >= countdown_sprites.Length) { index = countdown_sprites.Length - 1; }
        image.sprite = countdown_sprites[index];
    }
}
