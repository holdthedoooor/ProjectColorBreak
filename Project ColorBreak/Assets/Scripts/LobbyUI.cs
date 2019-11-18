using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public GameObject go_LobbyUI;
    public GameObject go_Slime;
    public GameObject go_TitleText;

    public Coroutine coroutine1;
    public Coroutine coroutine2;

    //1이면 Logo, 2면 Quit
    public int stopCourotineNum;
    
    void Start()
    {
        //현재 화면 상태를 Lobby로 변경
        Quit.instance.quitStatus = Quit.QuitStatus.Lobby;
        stopCourotineNum = 0;
    }

    //스테이지 선택 UI로 가는 버튼
    public void LobbyStartButton()
    {
        SoundManager.instance.PlaySFX( "Click_1" );

        go_LobbyUI.SetActive( false );

        UIManager.instance.chapterSelectUI.go_ChapterSelectUI.SetActive( true );

        //현재 화면 상태를 ChapterSelect로 변경
        Quit.instance.quitStatus = Quit.QuitStatus.ChapterSelect;

        if(stopCourotineNum == 0)
        {
            UIManager.instance.scenarioUI.StopCoroutine( coroutine1 );
            UIManager.instance.scenarioUI.StopCoroutine( coroutine2 );
            stopCourotineNum++;
        }
        else
        {
            Quit.instance.StopCoroutine( coroutine1 );
            Quit.instance.StopCoroutine( coroutine2 );
        }

        UIManager.instance.chapterSelectUI.coroutine = StartCoroutine( UIManager.instance.chapterSelectUI.TextImageChangeCoroutine() );
        UIManager.instance.chapterSelectUI.coroutineNum = 0;
    }

    public IEnumerator SlimeRotation()
    {
        go_Slime.transform.eulerAngles = new Vector3( 0, 0, 0 );

        while(go_Slime.activeInHierarchy)
        {
            go_Slime.transform.Rotate( new Vector3( 0, 0, 1 ) * Time.deltaTime * 120 );
            yield return null;
        }
    }

    public IEnumerator TextShake()
    {
        float _time = 0;
        go_TitleText.transform.localScale = new Vector3( 1, 1, 1 );

        while (go_Slime.activeInHierarchy)
        {
            go_TitleText.transform.localScale = new Vector3( 1.1f, 1.1f, 1 );

            _time = 0;
            while (_time < 0.3f)
            {
                _time += Time.deltaTime;
                go_TitleText.transform.localScale = Vector3.Lerp( new Vector3( 1.1f, 1.1f, 1 ), new Vector3( 1,1,1 ), _time / 0.3f );
                yield return null;
            }
            yield return null;
        }
    }
}
