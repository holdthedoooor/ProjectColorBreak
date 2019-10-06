using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelectUI : MonoBehaviour
{
    public GameObject go_ChapterSelectUI;
    public GameObject go_CurrentChapterUI;
    public GameObject[] go_Chapters;

    //챕터 선택 버튼들
    public Button[] chapters_Button;

    public GameObject[] chapter1_StageSlots;
    public GameObject[] chapter2_StageSlots;
    public GameObject[] chapter3_StageSlots;
    public GameObject[] chapter4_StageSlots;
    public GameObject[] chapter5_StageSlots;

    public int chapterUnlock;

    public void Chapter1_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        go_Chapters[0].SetActive( true );
        go_CurrentChapterUI = go_Chapters[0];

        UIManager.instance.currentChapter = 1;

        for (int i = 0; i < 9; i++)
            UIManager.instance.stageSlots[i] = chapter1_StageSlots[i].GetComponent<StageSlot>();
        
        UIManager.instance.bossStageSlot = chapter1_StageSlots[9].GetComponent<BossStageSlot>();
    }

    public void Chapter2_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        go_Chapters[1].SetActive( true );
        go_CurrentChapterUI = go_Chapters[1];

        UIManager.instance.currentChapter = 2;

        for (int i = 0; i < 9; i++)
            UIManager.instance.stageSlots[i] = chapter2_StageSlots[i].GetComponent<StageSlot>();

        UIManager.instance.bossStageSlot = chapter2_StageSlots[9].GetComponent<BossStageSlot>();
    }

    public void Chapter3_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        go_Chapters[2].SetActive( true );
        go_CurrentChapterUI = go_Chapters[2];

        UIManager.instance.currentChapter = 3;

        for (int i = 0; i < chapter3_StageSlots.Length; i++)
            UIManager.instance.stageSlots[i] = chapter3_StageSlots[i].GetComponent<StageSlot>();
        
    }

    public void Chapter4_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        go_Chapters[3].SetActive( true );
        go_CurrentChapterUI = go_Chapters[3];

        UIManager.instance.currentChapter = 4;

        for (int i = 0; i < chapter4_StageSlots.Length; i++)
            UIManager.instance.stageSlots[i] = chapter4_StageSlots[i].GetComponent<StageSlot>();
        
    }

    public void Chapter5_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        go_Chapters[4].SetActive( true );
        go_CurrentChapterUI = go_Chapters[4];

        UIManager.instance.currentChapter = 5;

        for (int i = 0; i < chapter5_StageSlots.Length; i++)
            UIManager.instance.stageSlots[i] = chapter5_StageSlots[i].GetComponent<StageSlot>();
        
    }

    public void BackButton()
    {
        go_CurrentChapterUI.SetActive( false );
        go_ChapterSelectUI.SetActive( true );
    }

    public void ChapterOpen()
    {
        for (int i = 0; i < chapterUnlock; i++)
        {
            chapters_Button[i].interactable = true;
        }
    }
}
