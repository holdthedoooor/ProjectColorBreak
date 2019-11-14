using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public GameObject go_PauseUI;

    public void ContinueButton()
    {
        SoundManager.instance.PlaySFX( "Click_1" );

        go_PauseUI.SetActive( false );
        Time.timeScale = 1;
        StageManager.instance.isPause = false;
        Quit.instance.quitStatus = Quit.QuitStatus.InGame;
    }

    public void HomeButton()
    {
        UIManager.instance.HomeButton();
        go_PauseUI.SetActive( false );
    }
}
