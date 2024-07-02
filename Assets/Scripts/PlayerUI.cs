using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    #region Singleton
    public static PlayerUI instance;
    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("More than one PlayerUI!");
        else instance = this;
    }
    #endregion
    public bool singlePlayer = false;
    public PlayerTag player;
    public Image[] hearts = new Image[3];
    public int greenKeysCount = 3;
    public Image[] greenKeys = new Image[3];
    public int yellowKeysCount = 3;
    public Image[] yellowKeys = new Image[3];
    public HealthBar[] healthBars = new HealthBar[2];
    public int mojoPower = 10;
    public float mojoBoostTime = 3f;
    public HealthBar greenMojoBar, yellowMojoBar;
    public Sprite fullHeart, emptyHeart, halfHeart, gotKeyGreen, gotKeyYellow;
    public Text coins;
    public Text[] scoreTexts = new Text[2];
    public Text[] highScoreTexts = new Text[2];
    public int gemsCount = 3;
    public Text[] gemText = new Text[1];
    public Sprite check, x;
    public Image greenGems, yellowGems;
    public GameObject gameOver, finishedWindow;
    [HideInInspector]
    public bool powerOn = false, greenDone = false, yellowDone = false, greenDoorOpen = false, yellowDoorOpen = false;
    [HideInInspector]
    public int gGems = 0, yGems = 0;
    public AudioClip winAudio, loseAudio, midLoseAudio;

    int heartsCount = 3;
    //float powerUpCount;
    [HideInInspector]
    public int gKeys = 0, yKeys = 0, gHealth = 100, yHealth = 100, score = 0;
    static int highScore = 0;
    [HideInInspector]
    public float greenMojo = 0, yellowMojo = 0;

    Sprite greenKeyEmpty, yellowKeyEmpty;

    private void Start()
    {
        if(greenKeysCount > 0)
         greenKeyEmpty = greenKeys[0].sprite;
        if (yellowKeysCount > 0)
            yellowKeyEmpty = yellowKeys[0].sprite;
        if (singlePlayer)
        {
            if(player == PlayerTag.Player1)
            {
                //yellowDone = true;
                yellowDoorOpen = true;
            }
            else
            {
               // greenDone = true;
                greenDoorOpen = true;
            }
        }
        Time.timeScale = 1;
        //powerUpCount = mojoBoostTime;
        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].SetHealth(100);
        }
        ChangeCoins(0);
        if (singlePlayer)
        {
            if (player == PlayerTag.Player1)
                greenMojoBar.SetHealth(greenMojo);
            else yellowMojoBar.SetHealth(yellowMojo);
        }
        else
        {
            greenMojoBar.SetHealth(greenMojo);
            yellowMojoBar.SetHealth(yellowMojo);
        }
        score = 0;
        coins.text = "0";
        for (int i = 0; i < gemText.Length; i++)
            gemText[i].text = "0 / " + gemsCount.ToString();
    }
    private void Update()
    {
        if (powerOn)
        {
            if (!singlePlayer)
            {
                greenMojo -= 0.2f;
                yellowMojo -= 0.2f;
                greenMojoBar.SetHealth(greenMojo);
                yellowMojoBar.SetHealth(yellowMojo);
                Debug.Log("mojo = " + greenMojo);
            }
            else 
            { 
                if (player == PlayerTag.Player1)
                {
                    greenMojo -= 0.2f;
                    greenMojoBar.SetHealth(greenMojo);
                }
                else
                {
                    yellowMojo -= 0.2f;
                    yellowMojoBar.SetHealth(yellowMojo);
                }
             }

        }

        if (!singlePlayer && (greenMojo <= 0 || yellowMojo <= 0))
        {
            powerOn = false;
            Debug.Log("power offf!");
        }
        else if (singlePlayer)
        {
            if (player == PlayerTag.Player1 && greenMojo <= 0)
                powerOn = false;
            else if (player == PlayerTag.Player2 && yellowMojo <= 0)
                powerOn = false;
        }

        if(greenDone || yellowDone)
        {
            if (singlePlayer || (greenDone && yellowDone))
            {
                Debug.Log("Done!!");
                greenDone = false;
                yellowDone = false;
                SoundManager.instance.PlayOneShot(winAudio);

                scoreTexts[0].text = coins.text;
                highScoreTexts[0].text = (score > highScore) ? score.ToString() : highScore.ToString();

                if (greenGems != null)
                {
                    if (gGems >= gemsCount) greenGems.sprite = check;
                    else greenGems.sprite = x;
                }

                if (yellowGems != null)
                {
                    if (yGems >= gemsCount) yellowGems.sprite = check;
                    else yellowGems.sprite = x;
                }

                finishedWindow.SetActive(true);
            }
        }
    }

    public void ChangeHealth(int h, int player)
    {         
        if (!singlePlayer)
        {
            if (player == 1) gHealth = h;
              else yHealth = h;
            healthBars[player - 1].SetHealth(h);
            if (gHealth + yHealth <= 120)
                hearts[heartsCount - 1].sprite = halfHeart;
        }
        else
        {
            Debug.Log("health = " + h);
            healthBars[0].SetHealth(h);
            if(this.player == PlayerTag.Player1) {
                gHealth = h;
                if(gHealth < 50)
                    hearts[heartsCount - 1].sprite = halfHeart;
            }
            else
            {
                yHealth = h;
                if (yHealth < 50)
                    hearts[heartsCount - 1].sprite = halfHeart;
            }
        }

    }

    public void Die(int lives)
    {
        Debug.Log(lives + " lives left");
        heartsCount--;
        if(heartsCount <= 0)
        {

            scoreTexts[1].text = coins.text;
            highScoreTexts[1].text = highScore.ToString();
            gameOver.SetActive(true);
            SoundManager.instance.PlayOneShot(loseAudio);
            if (gameOver.activeSelf)
                Time.timeScale = 0;
            else Time.timeScale = 1;
        }
        else
        {
            for(int i = 2, j = 0; j < 3 - heartsCount ;i--, j++)
                hearts[i].sprite = emptyHeart;

            greenDone = false;
            yellowDone = false;
            SoundManager.instance.PlayOneShot(midLoseAudio);
        }
    }

    public void gotKey(int player)
    {
        if((!singlePlayer && player == 1) || (singlePlayer && this.player == PlayerTag.Player1))
        {
            if (gKeys >= greenKeysCount) greenDoorOpen = true;
            else greenKeys[gKeys++].sprite = gotKeyGreen;
            Debug.Log("Got " + gKeys + " green keys");
        }
        else if((!singlePlayer && player == 2) || (singlePlayer && this.player == PlayerTag.Player2))
        {
            if (yKeys >= yellowKeysCount) yellowDoorOpen = true;
            yellowKeys[yKeys++].sprite = gotKeyYellow;
            Debug.Log("Got " + yKeys + " yellow keys");
        }
    }

    public void ChangeCoins(int c)
    {
        score = c;
        coins.text = c.ToString();
    }

    public void ChangeMojo(int player, int symbol)
    {
        if ((!singlePlayer && player == 1) || (singlePlayer && this.player == PlayerTag.Player1))
        {
            greenMojo += (mojoPower * symbol);
            greenMojoBar.SetHealth(greenMojo);
        }else if ((!singlePlayer && player == 2) || (singlePlayer && this.player == PlayerTag.Player2))
        {
            yellowMojo += (mojoPower * symbol);
            yellowMojoBar.SetHealth(yellowMojo);
        }

        if(!singlePlayer && greenMojo >= 100 && yellowMojo >= 100)
            powerOn = true;
        else if(singlePlayer && (greenMojo >= 100 || yellowMojo >= 100))
            powerOn = true;

        if (powerOn) Debug.Log("power on");
    }

    public void UpdateKeys(int player)
    {
        if(player == 1)
        {
            for(int i = 0; i < gKeys; i++)
                greenKeys[i].sprite = gotKeyGreen;
            for (int i = gKeys; i < greenKeysCount; i++)
                greenKeys[i].sprite = greenKeyEmpty;
        }
        else for (int i = 0; i < yKeys; i++)
                yellowKeys[i].sprite = gotKeyYellow;
        for (int i = yKeys; i < yellowKeysCount; i++)
            yellowKeys[i].sprite = yellowKeyEmpty;
    }

    public void UpdateGems(int gems, int player)
    {
        if(!singlePlayer)
           gemText[player - 1].text = gems.ToString() + " / " + gemsCount.ToString();
        else gemText[0].text = gems.ToString() + " / " + gemsCount.ToString();
    }
}


//if (gHealth + yHealth <= 50)
        //{
        //    --heartsCount;
        //    if ( heartsCount <= 0)
        //    {
        //        gameOver.SetActive(true);
        //    }

        //    for (int i = 2; i > 3 - heartsCount; i--)
        //    {
        //        hearts[i].sprite = emptyHeart;
        //    }
        //    gHealth = 100;
        //    yHealth = 100;
        //    for (int i = 0; i < 2; i++)
        //        healthBars[i].SetHealth(100);

        //    Debug.Log("Got " + heartsCount + " hearts");

        //}
        //else 