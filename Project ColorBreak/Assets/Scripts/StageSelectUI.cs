using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StageSelectUI : MonoBehaviour
{
    public void Chapter1_Back()
    {
        UIManager.instance.chapterSelectUI.go_Chapters[0].SetActive( false );
        UIManager.instance.chapterSelectUI.go_ChapterSelectUI.SetActive( true );
    }

    public void Chapter2_Back()
    {
        UIManager.instance.chapterSelectUI.go_Chapters[1].SetActive( false );
        UIManager.instance.chapterSelectUI.go_ChapterSelectUI.SetActive( true );
    }

    public void Chapter3_Back()
    {
        UIManager.instance.chapterSelectUI.go_Chapters[2].SetActive( false );
        UIManager.instance.chapterSelectUI.go_ChapterSelectUI.SetActive( true );
    }

    public void Chapter4_Back()
    {
        UIManager.instance.chapterSelectUI.go_Chapters[3].SetActive( false );
        UIManager.instance.chapterSelectUI.go_ChapterSelectUI.SetActive( true );
    }

    public void Chapter5_Back()
    {
        UIManager.instance.chapterSelectUI.go_Chapters[4].SetActive( false );
        UIManager.instance.chapterSelectUI.go_ChapterSelectUI.SetActive( true );
    }
}
