using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public List<int> stageBestScore = new List<int>();
    public List<int> stageStarCount = new List<int>();
    public List<int> stageStatusNumber = new List<int>();
    public int bossStageStatusNumber;
    public int bossStageMinPanaltyPoint;
    public int bossStageStarCount;
}

public class SaveLoad : MonoBehaviour
{
    public SaveData[] saveData = new SaveData[5];

    private string json;
    private string[] saveChapters = new string[] { "Chapter1", "Chapter2", "Chapter3", "Chapter4", "Chapter5"};

    public void SaveData()
    {
        saveData[StageManager.instance.currentChapter - 1].stageBestScore.Clear();
        saveData[StageManager.instance.currentChapter - 1].stageStarCount.Clear();
        saveData[StageManager.instance.currentChapter - 1].stageStatusNumber.Clear();

        for (int i = 0; i < UIManager.instance.stageSlots.Length; i++)
        {
            /*StageSlot stageSlot = UIManager.instance.stageSlots[ i ];
            if ( stageSlot == null )
            {
                Debug.Log( $"다음 스테이지 슬롯({i})이 비어있습니다. 스테이지를 추가해주세요." );
                continue;
            }*/

            if (UIManager.instance.stageSlots[i].stageStatus == StageSlot.StageStatus.Rock)
                break;
            /*
            if (UIManager.instance.stageSlots[i].stageStatus == StageSlot.StageStatus.Clear)
                clearStageCount++;*/

            saveData[StageManager.instance.currentChapter - 1].stageBestScore.Add( UIManager.instance.stageSlots[i].bestScore );
            saveData[StageManager.instance.currentChapter - 1].stageStarCount.Add( UIManager.instance.stageSlots[i].starCount );
            saveData[StageManager.instance.currentChapter - 1].stageStatusNumber.Add( (int)UIManager.instance.stageSlots[i].stageStatus );
        }

        //해당 챕터의 보스 스테이지가 Open, Clear면 
        if(UIManager.instance.bossStageSlot.bossStageStatus != BossStageSlot.BossStageStatus.Rock)
        {
            saveData[StageManager.instance.currentChapter - 1].bossStageStatusNumber = (int)UIManager.instance.bossStageSlot.bossStageStatus;
            saveData[StageManager.instance.currentChapter - 1].bossStageMinPanaltyPoint = UIManager.instance.bossStageSlot.minPanaltyPoint;
            saveData[StageManager.instance.currentChapter - 1].bossStageStarCount = UIManager.instance.bossStageSlot.starCount;
        }

        //현재 챕터의 보스스테이지를 클리어하고 별 개수가 현재 챕터의 목표 별 개수보다 크거나 같으면 다음챕터 Open
        if (StageManager.instance.beforeChapterUnlock != UIManager.instance.chapterSelectUI.chapterUnlock)
        {
            Debug.Log( "다음 챕터 1스테이지 오픈" );
            saveData[UIManager.instance.chapterSelectUI.chapterUnlock - 1].stageBestScore.Add( 0 );
            saveData[UIManager.instance.chapterSelectUI.chapterUnlock - 1].stageStarCount.Add( 0 );
            saveData[UIManager.instance.chapterSelectUI.chapterUnlock - 1].stageStatusNumber.Add( 1 );

            json = JsonUtility.ToJson( saveData[UIManager.instance.chapterSelectUI.chapterUnlock - 1], true );
            PlayerPrefs.SetString( saveChapters[UIManager.instance.chapterSelectUI.chapterUnlock - 1], json );
        }
           
        PlayerPrefs.SetInt( "ChapterUnlock", UIManager.instance.chapterSelectUI.chapterUnlock );

        switch (StageManager.instance.currentChapter)
        {
            case 1:
                json = JsonUtility.ToJson( saveData[0], true );
                if (PlayerPrefs.HasKey( saveChapters[0] ))
                    PlayerPrefs.DeleteKey( saveChapters[0] );
                PlayerPrefs.SetString( saveChapters[0], json );
                break;
            case 2:
                json = JsonUtility.ToJson( saveData[1], true );
                if (PlayerPrefs.HasKey( saveChapters[1] ))
                    PlayerPrefs.DeleteKey( saveChapters[1] );
                PlayerPrefs.SetString( saveChapters[1], json );
                break;
            case 3:
                json = JsonUtility.ToJson( saveData[2], true );
                if (PlayerPrefs.HasKey( saveChapters[2] ))
                    PlayerPrefs.DeleteKey( saveChapters[2] );
                PlayerPrefs.SetString( saveChapters[2], json );
                break;
            case 4:
                json = JsonUtility.ToJson( saveData[3], true );
                if (PlayerPrefs.HasKey( saveChapters[3] ))
                    PlayerPrefs.DeleteKey( saveChapters[3] );
                PlayerPrefs.SetString( saveChapters[3], json );
                break;
            case 5:
                json = JsonUtility.ToJson( saveData[4], true );
                if (PlayerPrefs.HasKey( saveChapters[4] ))
                    PlayerPrefs.DeleteKey( saveChapters[4] );
                PlayerPrefs.SetString( saveChapters[4], json );
                break;
        }
    }  

    public void LoadData()
    {
        if (PlayerPrefs.HasKey( "ChapterUnlock" ))
            UIManager.instance.chapterSelectUI.chapterUnlock = PlayerPrefs.GetInt( "ChapterUnlock" );
        else
            return;

        for (int i = 0; i < UIManager.instance.chapterSelectUI.chapterUnlock; i++)
        {
            json = PlayerPrefs.GetString( saveChapters[i] );
            saveData[i] = JsonUtility.FromJson<SaveData>( json );

            if (saveData[0].stageStatusNumber.Count == 0)
                return;

            /*
            if (saveData[UIManager.instance.chapterSelectUI.chapterUnlock - 1].stageBestScore.Count == 0)
            {
                saveData[UIManager.instance.chapterSelectUI.chapterUnlock - 1].stageStarCount.Add( 0 );
                saveData[UIManager.instance.chapterSelectUI.chapterUnlock - 1].stageBestScore.Add( 0 );
                saveData[UIManager.instance.chapterSelectUI.chapterUnlock - 1].stageStatusNumber.Add(1);
            }*/

            for (int j = 0; j < saveData[i].stageBestScore.Count; j++)
            {
                UIManager.instance.chapterSelectUI.allStageSlot[i].stageSlots[j].SetStageSlot( saveData[i].stageBestScore[j], saveData[i].stageStarCount[j], saveData[i].stageStatusNumber[j] );
                StageManager.instance.chaptersStarCount[i] += UIManager.instance.chapterSelectUI.allStageSlot[i].stageSlots[j].starCount;

                if (saveData[i].stageStatusNumber[j] == 0)
                    break;
            }
            if (saveData[i].stageBestScore.Count == 9)
            {
                if (UIManager.instance.chapterSelectUI.allStageSlot[i].bossStageSlot != null)
                {
                    UIManager.instance.chapterSelectUI.allStageSlot[i].bossStageSlot.SetBossStageSlot( saveData[i].bossStageStatusNumber, saveData[i].bossStageMinPanaltyPoint, saveData[i].bossStageStarCount );
                    StageManager.instance.chaptersStarCount[i] += UIManager.instance.chapterSelectUI.allStageSlot[i].bossStageSlot.starCount;
                }
            }
        }

        if (UIManager.instance.chapterSelectUI.chapterUnlock >= 3)
            UIManager.instance.chapterSelectUI.endingScene_Btn.SetActive( true );

        UIManager.instance.chapterSelectUI.LoadChapterOpen();
    }

    public void DataReset()
    {
        PlayerPrefs.DeleteAll();
    }
}
