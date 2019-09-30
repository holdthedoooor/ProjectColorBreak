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

    void Start()
    {
        StartCoroutine( CharacterImageChangeCoroutine() );
    }

    //스테이지 선택 UI로 가는 버튼
    public void LobbyStartButton()
    {
        if(!StageManager.instance.isMasterMode)
        {
            StageManager.instance.theSaveLoad.LoadData();
            UIManager.instance.chapterSelectUI.ChapterOpen();
        }
        go_LobbyUI.SetActive( false );
        UIManager.instance.chapterSelectUI.go_ChapterSelectUI.SetActive( true );
        isStop = true;
    }

    public IEnumerator CharacterImageChangeCoroutine()
    {
        while(!isStop)
        {
            yield return new WaitForSeconds( 2f );

            num++;

            if (num == character_Sprites.Length)
                num = 0;

            character_Image.sprite = character_Sprites[num];
        }
    }
}
