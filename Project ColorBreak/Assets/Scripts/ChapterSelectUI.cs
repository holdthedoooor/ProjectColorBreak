using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelectUI : MonoBehaviour
{
    public GameObject go_ChapterSelectUI;
    public GameObject go_CurrentChapterUI;
    [Header("각각의 Chapter UI")]
    public GameObject[] go_Chapters;
    [Header("Slot들을 할당하기 위한 Slot들의 부모 게임오브젝트")]
    public GameObject[] go_SlotParents;
    //챕터 선택 버튼들
    public Button[] chapters_Button;

    public StageSlot[] chapter1_StageSlots;
    public StageSlot[] chapter2_StageSlots;
    public StageSlot[] chapter3_StageSlots;
    public StageSlot[] chapter4_StageSlots;
    public StageSlot[] chapter5_StageSlots;
    //1~5 챕터의 보스 스테이지 슬롯들이 할당됨
    public BossStageSlot[] BossStageSlots;

    public int chapterUnlock;

    public void Chapter1_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        go_Chapters[0].SetActive( true );
        go_CurrentChapterUI = go_Chapters[0];

        UIManager.instance.currentChapter = 1;

        for (int i = 0; i < chapter1_StageSlots.Length; i++)
            UIManager.instance.stageSlots[i] = chapter1_StageSlots[i];
        
        if(BossStageSlots.Length > 0)
            UIManager.instance.bossStageSlot = BossStageSlots[0];
    }

    public void Chapter2_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        go_Chapters[1].SetActive( true );
        go_CurrentChapterUI = go_Chapters[1];

        UIManager.instance.currentChapter = 2;

        for (int i = 0; i < chapter2_StageSlots.Length; i++)
            UIManager.instance.stageSlots[i] = chapter2_StageSlots[i];

        if (BossStageSlots.Length > 1)
            UIManager.instance.bossStageSlot = BossStageSlots[1];
    }

    public void Chapter3_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        go_Chapters[2].SetActive( true );
        go_CurrentChapterUI = go_Chapters[2];

        UIManager.instance.currentChapter = 3;

        for (int i = 0; i < chapter3_StageSlots.Length; i++)
            UIManager.instance.stageSlots[i] = chapter3_StageSlots[i];

        if (BossStageSlots.Length > 2)
            UIManager.instance.bossStageSlot = BossStageSlots[2];
    }

    public void Chapter4_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        go_Chapters[3].SetActive( true );
        go_CurrentChapterUI = go_Chapters[3];

        UIManager.instance.currentChapter = 4;

        for (int i = 0; i < chapter4_StageSlots.Length; i++)
            UIManager.instance.stageSlots[i] = chapter4_StageSlots[i];

        if (BossStageSlots.Length > 3)
            UIManager.instance.bossStageSlot = BossStageSlots[3];

    }

    public void Chapter5_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        go_Chapters[4].SetActive( true );
        go_CurrentChapterUI = go_Chapters[4];

        UIManager.instance.currentChapter = 5;

        for (int i = 0; i < chapter5_StageSlots.Length; i++)
            UIManager.instance.stageSlots[i] = chapter5_StageSlots[i];

        if (BossStageSlots.Length > 4)
            UIManager.instance.bossStageSlot = BossStageSlots[4];
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
