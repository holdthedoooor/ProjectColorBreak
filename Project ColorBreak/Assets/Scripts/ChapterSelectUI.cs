using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterSelectUI : MonoBehaviour
{
    public GameObject go_ChapterSelectUI;
    public GameObject go_CurrentChapterUI { get; private set; }
    public GameObject[] go_Chapters;

    //슬롯들의 부모 오브젝트
    public GameObject[] go_StageSlotParents;

    public StageSlot[] chapter1_StageSlots;
    public StageSlot[] chapter2_StageSlots;
    public StageSlot[] chapter3_StageSlots;
    public StageSlot[] chapter4_StageSlots;
    public StageSlot[] chapter5_StageSlots;

    private void Awake()
    {
        chapter1_StageSlots = go_StageSlotParents[0].GetComponentsInChildren<StageSlot>();
        chapter2_StageSlots = go_StageSlotParents[1].GetComponentsInChildren<StageSlot>();
        chapter3_StageSlots = go_StageSlotParents[2].GetComponentsInChildren<StageSlot>();
        chapter4_StageSlots = go_StageSlotParents[3].GetComponentsInChildren<StageSlot>();
        chapter5_StageSlots = go_StageSlotParents[4].GetComponentsInChildren<StageSlot>();
    }

    public void Chapter1_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        go_Chapters[0].SetActive( true );
        go_CurrentChapterUI = go_Chapters[0];

        for (int i = 0; i < UIManager.instance.stageSlots.Length; i++)
        {
            UIManager.instance.stageSlots[i] = null;
        }

        for (int i = 0; i < chapter1_StageSlots.Length; i++)
        {
            Debug.Log( UIManager.instance.stageSlots.Length );
            UIManager.instance.stageSlots[i] = chapter1_StageSlots[i];
        }
    }

    public void Chapter2_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        go_Chapters[1].SetActive( true );
        go_CurrentChapterUI = go_Chapters[1];

        for (int i = 0; i < UIManager.instance.stageSlots.Length; i++)
        {
            UIManager.instance.stageSlots[i] = null;
        }

        for (int i = 0; i < chapter2_StageSlots.Length; i++)
        {
            UIManager.instance.stageSlots[i] = chapter2_StageSlots[i];
        }
    }

    public void Chapter3_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        go_Chapters[2].SetActive( true );
        go_CurrentChapterUI = go_Chapters[2];

        for (int i = 0; i < UIManager.instance.stageSlots.Length; i++)
        {
            UIManager.instance.stageSlots[i] = null;
        }

        for (int i = 0; i < chapter3_StageSlots.Length; i++)
        {
            UIManager.instance.stageSlots[i] = chapter3_StageSlots[i];
        }
    }

    public void Chapter4_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        go_Chapters[3].SetActive( true );
        go_CurrentChapterUI = go_Chapters[3];

        for (int i = 0; i < UIManager.instance.stageSlots.Length; i++)
        {
            UIManager.instance.stageSlots[i] = null;
        }

        for (int i = 0; i < chapter4_StageSlots.Length; i++)
        {
            UIManager.instance.stageSlots[i] = chapter4_StageSlots[i];
        }
    }

    public void Chapter5_Button()
    {
        go_ChapterSelectUI.SetActive( false );
        go_Chapters[4].SetActive( true );
        go_CurrentChapterUI = go_Chapters[4];

        for (int i = 0; i < UIManager.instance.stageSlots.Length; i++)
        {
            UIManager.instance.stageSlots[i] = null;
        }

        for (int i = 0; i < chapter5_StageSlots.Length; i++)
        {
            UIManager.instance.stageSlots[i] = chapter5_StageSlots[i];
        }
    }
}
