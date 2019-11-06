using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public GameObject go_LobbyUI;

    void Start()
    {
        //현재 화면 상태를 Lobby로 변경
        Quit.instance.quitStatus = Quit.QuitStatus.Lobby;
    }

    //스테이지 선택 UI로 가는 버튼
    public void LobbyStartButton()
    {
        go_LobbyUI.SetActive( false );

        UIManager.instance.chapterSelectUI.go_ChapterSelectUI.SetActive( true );

        //현재 화면 상태를 ChapterSelect로 변경
        Quit.instance.quitStatus = Quit.QuitStatus.ChapterSelect;
    }
}
