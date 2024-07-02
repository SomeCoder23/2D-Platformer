using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Sprite[] numbers = new Sprite[10];
    public Image digit1, digit2, zero;
    public Text dots;
    public AudioClip tikTok;

    public void CountDown(int time)
    {
        digit1.sprite = numbers[time / 10];
        digit2.sprite = numbers[time % 10];
        if(time <= 5)
        {
            SoundManager.instance.PlayOneShot(tikTok);
            digit1.color = Color.red;
            digit2.color = Color.red;
            zero.color = Color.red;
            dots.color = Color.red;
        }
    }
}
