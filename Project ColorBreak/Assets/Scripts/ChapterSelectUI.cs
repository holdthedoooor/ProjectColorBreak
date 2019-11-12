using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AllStageSlot
{
    public StageSlot[] stageSlots = new StageSlot[9];
    public BossStageSlot bossStageSlot;
}

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

    public AllStageSlot[] allStageSlot = new AllStageSlot[5];

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

    //별 개수 확인
    public GameObject go_StarCountCheckUI;
    public Text totalStartCount_Text;
    public Text[] chaptersStarCount_Text;

    public void Chapter1_Button()
    {
        StopCoroutine( coroutine );

        go_ChapterSelectUI.SetActive( false );
        go_Chapters[0].SetActive( true );
        go_CurrentChapterUI = go_Chapters[0];

        StageManager.instance.currentChapter = 1;

        for (int i = 0; i < allStageSlot[0].stageSlots.Length; i++)
            UIManager.instance.stageSlots[i] = allStageSlot[0].stageSlots[i];

        UIManager.instance.bossStageSlot = allStageSlot[0].bossStageSlot;

        Quit.instance.quitStatus = Quit.QuitStatus.StageSelect;
    }

    public void Chapter2_Button()
    {
        StopCoroutine( coroutine );

        go_ChapterSelectUI.SetActive( false );
        go_Chapters[1].SetActive( true );
        go_CurrentChapterUI = go_Chapters[1];

        StageManager.instance.currentChapter = 2;

        for (int i = 0; i < allStageSlot[1].stageSlots.Length; i++)
            UIManager.instance.stageSlots[i] = allStageSlot[1].stageSlots[i];

        UIManager.instance.bossStageSlot = allStageSlot[1].bossStageSlot;

        Quit.instance.quitStatus = Quit.QuitStatus.StageSelect;
    }

    public void Chapter3_Button()
    {
        StopCoroutine( coroutine );

        go_ChapterSelectUI.SetActive( false );
        go_Chapters[2].SetActive( true );
        go_CurrentChapterUI = go_Chapters[2];

        StageManager.instance.currentChapter = 3;

        for (int i = 0; i < allStageSlot[2].stageSlots.Length; i++)
            UIManager.instance.stageSlots[i] = allStageSlot[2].stageSlots[i];

        UIManager.instance.bossStageSlot = allStageSlot[2].bossStageSlot;

        Quit.instance.quitStatus = Quit.QuitStatus.StageSelect;
    }

    public void Chapter4_Button()
    {
        StopCoroutine( coroutine );

        go_ChapterSelectUI.SetActive( false );
        go_Chapters[3].SetActive( true );
        go_CurrentChapterUI = go_Chapters[3];

        StageManager.instance.currentChapter = 4;

        for (int i = 0; i < allStageSlot[3].stageSlots.Length; i++)
            UIManager.instance.stageSlots[i] = allStageSlot[3].stageSlots[i];

        //UIManager.instance.bossStageSlot = allStageSlot[3].bossStageSlot;

        Quit.instance.quitStatus = Quit.QuitStatus.StageSelect;
    }

    public void Chapter5_Button()
    {
        StopCoroutine( coroutine );

        go_ChapterSelectUI.SetActive( false );
        go_Chapters[4].SetActive( true );
        go_CurrentChapterUI = go_Chapters[4];

        StageManager.instance.currentChapter = 5;

        for (int i = 0; i < allStageSlot[4].stageSlots.Length; i++)
            UIManager.instance.stageSlots[i] = allStageSlot[4].stageSlots[i];

        //UIManager.instance.bossStageSlot = allStageSlot[4].bossStageSlot;

        Quit.instance.quitStatus = Quit.QuitStatus.StageSelect;
    }

    public void BackButton()
    {
        go_CurrentChapterUI.SetActive( false );
        go_ChapterSelectUI.SetActive( true );
        TICStartCoroutine();

        Quit.instance.quitStatus = Quit.QuitStatus.ChapterSelect;
    }

    public void LoadChapterOpen()
    {
        for (int i = 0; i < chapterUnlock - 1; i++)
        {
            chapters_Button[i + 1].interactable = true;
            chapters_Button[i + 1].GetComponent<Image>().sprite = chapterOpen_Sprites[i];
            chapterText_Images[i].sprite = chapterOpenText_Sprites[i];
        }
    }

    public void NextChapterOpen()
    {
        chapters_Button[StageManager.instance.currentChapter].interactable = true;
        chapters_Button[StageManager.instance.currentChapter].GetComponent<Image>().sprite = chapterOpen_Sprites[StageManager.instance.currentChapter - 1];
        chapterText_Images[StageManager.instance.currentChapter - 1].sprite = chapterOpenText_Sprites[StageManager.instance.currentChapter - 1];
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

    public void TICStartCoroutine()
    {
        coroutine = StartCoroutine( TextImageChangeCoroutine() );
    }

    public void SetStarCountText()
    {
        int totalStarCount = 0;

        for (int i = 0; i < chapterUnlock; i++)
        {
            totalStarCount += StageManager.instance.chaptersStarCount[i];
        }

        chaptersStarCount_Text[StageManager.instance.currentChapter - 1].text = StageManager.instance.chaptersStarCount[StageManager.instance.currentChapter - 1].ToString();
        totalStartCount_Text.text = totalStarCount.ToString();
    }

    public void LoadStarCountText()
    {
        int totalStarCount = 0;

        for (int i = 0; i < chapterUnlock; i++)
        {
            totalStarCount += StageManager.instance.chaptersStarCount[i];
            chaptersStarCount_Text[i].text = StageManager.instance.chaptersStarCount[i].ToString();
        }
        totalStartCount_Text.text = totalStarCount.ToString();
    }

    public void StarCountCheck_Button()
    {
        Quit.instance.ActivePopUp( go_StarCountCheckUI, Quit.instance.quitStatus );
    }

    public void StarCountCheckCancel_Button()
    {
        Quit.instance.DeactivePopUP();
    }
}
