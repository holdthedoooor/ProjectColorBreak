using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public static Quit instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<Quit>();
            }
            return m_instance;
        }
    }
    private static Quit m_instance; //싱글톤이 할당될 변수

    //현재 어느 화면인지
    public enum QuitStatus
    {
        Lobby,
        ChapterSelect,
        StageSelect,
        StopGame,
        InGame,
    }
    public QuitStatus quitStatus;

    public int num = 0;

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.Android)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                switch (quitStatus)
                {
                    case QuitStatus.Lobby:
                        //종료하시겠습니까 창 출력
                        num++;
                        if (num == 1)
                        {
                            UIManager.instance.quitUI.go_QuitUI.SetActive( true );
                            num = -1;
                        }
                        else if (num == 0)
                            UIManager.instance.quitUI.go_QuitUI.SetActive( false );

                        break;

                    case QuitStatus.ChapterSelect:
                        UIManager.instance.chapterSelectUI.StopCoroutine( UIManager.instance.chapterSelectUI.coroutine );
                        UIManager.instance.chapterSelectUI.go_ChapterSelectUI.SetActive( false );
                        UIManager.instance.lobbyUI.go_LobbyUI.SetActive( true );
                        UIManager.instance.lobbyUI.LobbyStartCoroutine();
                        quitStatus = QuitStatus.Lobby;
                        break;

                    case QuitStatus.StageSelect:
                        UIManager.instance.chapterSelectUI.BackButton();
                        break;

                    case QuitStatus.StopGame:
                        UIManager.instance.pauseUI.ContinueButton();
                        break;

                    case QuitStatus.InGame:
                        UIManager.instance.PauseButton();
                        break;
                }
            }
        }
    }
}
