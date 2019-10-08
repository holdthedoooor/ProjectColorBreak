using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitUI : MonoBehaviour
{
    public GameObject go_QuitUI;

    public void YesButton()
    {
        Application.Quit();
    }

    public void NoButton()
    {
        go_QuitUI.SetActive( false );
        Quit.instance.num++;
    }
}
