using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterSelectUI : MonoBehaviour
{
    public GameObject go_ChapterSelectUI;
    public GameObject[] go_Chapters;


    public void Chapter1_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        UIManager.instance.stageSelectUI.go_StageSelectUI.SetActive( true );
        go_Chapters[0].SetActive( true );
    }

    public void Chapter2_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        UIManager.instance.stageSelectUI.go_StageSelectUI.SetActive( true );
        go_Chapters[1].SetActive( true );
    }

    public void Chapter3_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        UIManager.instance.stageSelectUI.go_StageSelectUI.SetActive( true );
        go_Chapters[2].SetActive( true );
    }

    public void Chapter4_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        UIManager.instance.stageSelectUI.go_StageSelectUI.SetActive( true );
        go_Chapters[3].SetActive( true );
    }

    public void Chapter5_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        UIManager.instance.stageSelectUI.go_StageSelectUI.SetActive( true );
        go_Chapters[4].SetActive( true );
    }
}
