using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public GameObject go_LobbyUI;

    public Image character_Image;

    public Sprite[] character_Sprites;

    public bool isStop;
    private int num = 0;
    private Coroutine coroutine;

    void Start()
    {
        LobbyStartCoroutine();

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

        //캐릭터 이미지 변경 코루틴 멈춤
        StopCoroutine( coroutine );
    }

    public IEnumerator CharacterImageChangeCoroutine()
    {
        num = 0;
        isStop = false;
        character_Image.sprite = character_Sprites[0];
        while (!isStop)
        {
            yield return new WaitForSeconds( 2f );

            num++;

            if (num == character_Sprites.Length)
                num = 0;

            character_Image.sprite = character_Sprites[num];
        }
    }

    public void LobbyStartCoroutine()
    {
        coroutine = StartCoroutine( CharacterImageChangeCoroutine() );
    }
}
