using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewORLoadUI : MonoBehaviour
{
    public GameObject go_NewORLoadUI;

    public void NewGameButton()
    {
        go_NewORLoadUI.SetActive( false );
        UIManager.instance.lobbyUI.go_LobbyUI.SetActive( true );
    }

    /*
    public void LoadGameButton()
    {
        if(StageManager.instance.theSaveLoad.LoadData())
        {
            go_NewORLoadUI.SetActive( false );
            UIManager.instance.lobbyUI.go_LobbyUI.SetActive( true );
        }
    }*/
}
