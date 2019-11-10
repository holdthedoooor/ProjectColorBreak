using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public GameObject go_LobbyUI;
    public GameObject go_Slime;
    public GameObject go_TitleText;
    private Vector3 originPosition;

    private float time = 0f;
    
    void Start()
    {
        //현재 화면 상태를 Lobby로 변경
        Quit.instance.quitStatus = Quit.QuitStatus.Lobby;

        originPosition = go_TitleText.transform.position;
    }

    void Update()
    {
        if (!go_Slime.activeInHierarchy)
            return;

        go_Slime.transform.Rotate( new Vector3( 0, 0, 1 ) * Time.deltaTime * 120 );

        if (time <= 3f)
        {
            time += Time.deltaTime;
            go_TitleText.transform.position = originPosition + Random.insideUnitSphere * 10;
            go_TitleText.transform.position = new Vector3( go_TitleText.transform.position.x, go_TitleText.transform.position.y, 0 );
        }
        else
        {
            if(time <= 6f)
                time += Time.deltaTime;
            else
                time = 0;
        }
           
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
