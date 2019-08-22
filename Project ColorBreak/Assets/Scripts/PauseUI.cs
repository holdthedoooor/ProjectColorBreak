using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public GameObject go_PauseUI;

    public void ContinueButton()
    {
        go_PauseUI.SetActive( false );
        Time.timeScale = 1;
        StageManager.instance.isPause = false;
    }

    public void HomeButton()
    {
        UIManager.instance.HomeButton();
        go_PauseUI.SetActive( false );
    }
}
