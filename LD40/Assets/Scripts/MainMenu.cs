using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button ButtonSound;
    private static readonly string OFFLINE_GAME = "OfflineGame";

    private void Start()
    {
    }

    public void LoadGame()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        SceneManager.LoadScene(OFFLINE_GAME);
    }

    public void SwitchSoundOnOff()
    {
        SharedSettings.IsSoundOn = !SharedSettings.IsSoundOn;
        AudioListener.volume = 1 - AudioListener.volume;
        string onOff = (SharedSettings.IsSoundOn) ? "On" : "Off";
        ButtonSound.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Sound" + onOff);

    }

    public void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (Input.GetKeyDown(KeyCode.Return)
            || Input.GetKeyDown(KeyCode.KeypadEnter)
            || Input.GetKeyDown(KeyCode.Space))
        {
            LoadGame();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SwitchSoundOnOff();
        }
#endif
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
