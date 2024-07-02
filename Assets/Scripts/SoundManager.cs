using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    #region Singleton
    public static SoundManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("More than one Sound Manager!");
        else instance = this;
    }
    #endregion
    public AudioSource soundFXAudio, musicAudio, randomAudio;
    public AudioClip clickAudio;
    static bool soundOff = false, musicOff = false, soundOn = true, musicOn = true;


    private void Start()
    {
        Toggle[] toggles = SceneChanger.instance.OptionsMenu.transform.GetComponentsInChildren<Toggle>();
        if (soundOff) { 
            //MuteS();           
            for(int i = 0; i < toggles.Length; i++)
                if (toggles[i].gameObject.name == "SoundFX")
                    toggles[i].isOn = false;
        }

        if (musicOff)
        {
            //musicAudio.enabled = !musicAudio.isActiveAndEnabled;
            for (int i = 0; i < toggles.Length; i++)
                if (toggles[i].gameObject.name == "Music")
                    toggles[i].isOn = false;
        }
    }


    public void PlayOneShot(AudioClip clip)
    {
        soundFXAudio.PlayOneShot(clip);
    }

    public void RandomClip(AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        randomAudio.pitch = Random.Range(0.95f, 1.05f);
        randomAudio.PlayOneShot(clips[randomIndex]);
    }

    public void ClickButton()
    {
        PlayOneShot(clickAudio);
    }

    public void MuteMusic()
    {
        musicAudio.enabled = !musicAudio.isActiveAndEnabled;
        ClickButton();
        if (musicOn)
        {
            musicOn = false;
            musicOff = true;
        }
        else
        {
            musicOn = true;
            musicOff = false;
        }
        //musicOff = !musicOff;

    }
    public void MuteSound()
    {
        soundFXAudio.enabled = !soundFXAudio.isActiveAndEnabled;
        if(randomAudio != null)
          randomAudio.enabled = !randomAudio.isActiveAndEnabled;
        if (soundOn)
        {
            soundOn = false;
            soundOff = true;
        }
        else
        {
            soundOn = true;
            soundOff = false;
        }
        ClickButton();
    }

    void MuteS()
    {
        soundFXAudio.enabled = !soundFXAudio.isActiveAndEnabled;
        if (randomAudio != null)
            randomAudio.enabled = !randomAudio.isActiveAndEnabled;
    }
}
