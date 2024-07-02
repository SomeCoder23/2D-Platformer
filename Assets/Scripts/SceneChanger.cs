using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    #region Singleton
    public static SceneChanger instance;
    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("More than one Scene Changer!");
        else instance = this;
    }
    #endregion
    //public Sprite sound, mute;
    //public Button icon;
    public GameObject OptionsMenu;

    public void ChangeScene(string scene)
    {
        SoundManager.instance.ClickButton();
        SceneManager.LoadScene(scene);
    }

    public void Exit()
    {
        Debug.Log("exitgame");
        SoundManager.instance.ClickButton();
        Application.Quit();
    }

    //public void Mute()
    //{
    //    AudioSource audio = GetComponent<AudioSource>();
    //    bool activate = !audio.gameObject.activeSelf;
    //    audio.gameObject.SetActive(activate);
    //    if (activate)
    //        icon.image.sprite = sound;
    //    else icon.image.sprite = mute;
    //}

    public void OpenOptions()
    {
        OptionsMenu.SetActive(!OptionsMenu.activeSelf);
        SoundManager.instance.ClickButton();
        if (OptionsMenu.activeSelf)
            Time.timeScale = 0;
        else Time.timeScale = 1;
    }
}
