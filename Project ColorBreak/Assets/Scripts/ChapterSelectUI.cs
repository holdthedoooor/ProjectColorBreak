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

    //Chapter Rock 이미지
    public Image[] chapterText_Images;
    public Sprite[] chapterOpen_Sprites;
    public Sprite[] chapterOpenText_Sprites;

    //Chapter Select 텍스트 이미지 변경
    public Coroutine coroutine;
    public Image text_Image;
    public Sprite[] text_Sprites;
    private int num;
    public bool isStop = false;

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

        Quit.instance.quitStatus = Quit.QuitStatus.StageSelect;
        ChapterStopCoroutine();
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

        Quit.instance.quitStatus = Quit.QuitStatus.StageSelect;
        ChapterStopCoroutine();
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

        Quit.instance.quitStatus = Quit.QuitStatus.StageSelect;
        ChapterStopCoroutine();
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

        Quit.instance.quitStatus = Quit.QuitStatus.StageSelect;
        ChapterStopCoroutine();
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

        Quit.instance.quitStatus = Quit.QuitStatus.StageSelect;
        ChapterStopCoroutine();
    }

    public void BackButton()
    {
        go_CurrentChapterUI.SetActive( false );
        go_ChapterSelectUI.SetActive( true );
        Quit.instance.quitStatus = Quit.QuitStatus.ChapterSelect;
        ChapterStartCoroutine();
    }

    public void ChapterOpen()
    {
        for (int i = 0; i < chapterUnlock - 1; i++)
        {
            chapters_Button[i + 1].interactable = true;
            chapters_Button[i + 1].GetComponent<Image>().sprite = chapterOpen_Sprites[i];
            chapterText_Images[i].sprite = chapterOpenText_Sprites[i];
        }
    }

    public IEnumerator TextImageChangeCoroutine()
    {
        num = 0;
        isStop = false;
        text_Image.sprite = text_Sprites[0];
        while (!isStop)
        {
            yield return new WaitForSeconds( 2f );

            num++;

            if (num == text_Sprites.Length)
                num = 0;

            text_Image.sprite = text_Sprites[num];
        }
    }

    public void ChapterStartCoroutine()
    {
        coroutine = StartCoroutine( TextImageChangeCoroutine() );
    }

    public void ChapterStopCoroutine()
    {
        StopCoroutine( coroutine );
    }
}
