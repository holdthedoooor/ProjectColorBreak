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
    private int clearStageCount; //현재 챕터에서 클리어한 스테이지가 몇개인지


    public void SaveData()
    {
        saveData[UIManager.instance.currentChapter - 1].stageBestScore.Clear();
        saveData[UIManager.instance.currentChapter - 1].stageStarCount.Clear();
        saveData[UIManager.instance.currentChapter - 1].stageStatusNumber.Clear();

        clearStageCount = 0;

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

            saveData[UIManager.instance.currentChapter - 1].stageBestScore.Add( UIManager.instance.stageSlots[i].bestScore );
            saveData[UIManager.instance.currentChapter - 1].stageStarCount.Add( UIManager.instance.stageSlots[i].starCount );
            saveData[UIManager.instance.currentChapter - 1].stageStatusNumber.Add( (int)UIManager.instance.stageSlots[i].stageStatus );
        }

        saveData[UIManager.instance.currentChapter - 1].bossStageStatusNumber = (int)UIManager.instance.bossStageSlot.bossStageStatus;
        saveData[UIManager.instance.currentChapter - 1].bossStageMinPanaltyPoint = UIManager.instance.bossStageSlot.minPanaltyPoint;
        saveData[UIManager.instance.currentChapter - 1].bossStageStarCount = UIManager.instance.bossStageSlot.starCount;

        /*//현재 챕터의 1~9 스테이지를 클리어 했을 경우에만 보스스테이지의 정보를 저장한다.
        if (clearStageCount == 9)
        {
           
        }*/

        //현재 챕터의 보스스테이지를 클리어했으면 다음 챕터의 1스테이지를 Open으로 바꿔준다.
        if (saveData[UIManager.instance.currentChapter - 1].bossStageStatusNumber == 2)
        {
            saveData[UIManager.instance.currentChapter].stageBestScore.Add( 0 );
            saveData[UIManager.instance.currentChapter].stageStarCount.Add( 0 );
            saveData[UIManager.instance.currentChapter].stageStatusNumber.Add( 1 );

            json = JsonUtility.ToJson( saveData[UIManager.instance.currentChapter], true );
            PlayerPrefs.SetString( saveChapters[UIManager.instance.currentChapter], json );
        }
           
        PlayerPrefs.SetInt( "ChapterUnlock", UIManager.instance.chapterSelectUI.chapterUnlock );

        switch (UIManager.instance.currentChapter)
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

        Debug.Log( json );
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey( "ChapterUnlock" ))
            UIManager.instance.chapterSelectUI.chapterUnlock = PlayerPrefs.GetInt( "ChapterUnlock" );

        else
            return;

        switch(UIManager.instance.chapterSelectUI.chapterUnlock)
        {
            case 1:
                json = PlayerPrefs.GetString( "Chapter1" );
                saveData[0] = JsonUtility.FromJson<SaveData>( json );

                for (int i = 0; i < saveData[0].stageBestScore.Count; i++)
                    UIManager.instance.chapterSelectUI.chapter1_StageSlots[i].GetComponent<StageSlot>().SetStageSlot( saveData[0].stageBestScore[i], saveData[0].stageStarCount[i], saveData[0].stageStatusNumber[i] );

                UIManager.instance.chapterSelectUI.chapter1_StageSlots[9].GetComponent<BossStageSlot>().SetBossStageSlot( saveData[0].bossStageStatusNumber, saveData[0].bossStageMinPanaltyPoint, saveData[0].bossStageStarCount );
                break;

            case 2:
                for (int i = 0; i < 2; i++)
                {
                    json = PlayerPrefs.GetString( saveChapters[i] );
                    saveData[i] = JsonUtility.FromJson<SaveData>( json );
                }

                for (int i = 0; i < saveData[0].stageBestScore.Count; i++)
                    UIManager.instance.chapterSelectUI.chapter1_StageSlots[i].GetComponent<StageSlot>().SetStageSlot( saveData[0].stageBestScore[i], saveData[0].stageStarCount[i], saveData[0].stageStatusNumber[i] );

                for (int i = 0; i < saveData[1].stageBestScore.Count; i++)
                {
                    UIManager.instance.chapterSelectUI.chapter2_StageSlots[i].GetComponent<StageSlot>().SetStageSlot( saveData[1].stageBestScore[i], saveData[1].stageStarCount[i], saveData[1].stageStatusNumber[i] );
                }     

                UIManager.instance.chapterSelectUI.chapter1_StageSlots[9].GetComponent<BossStageSlot>().SetBossStageSlot( saveData[0].bossStageStatusNumber, saveData[0].bossStageMinPanaltyPoint, saveData[0].bossStageStarCount );
                UIManager.instance.chapterSelectUI.chapter2_StageSlots[9].GetComponent<BossStageSlot>().SetBossStageSlot( saveData[1].bossStageStatusNumber, saveData[1].bossStageMinPanaltyPoint, saveData[1].bossStageStarCount );
                break;

            case 3:
                for (int i = 0; i < 3; i++)
                {
                    json = PlayerPrefs.GetString( saveChapters[i] );
                    saveData[i] = JsonUtility.FromJson<SaveData>( json );
                }

                for (int i = 0; i < saveData[0].stageBestScore.Count; i++)
                    UIManager.instance.chapterSelectUI.chapter1_StageSlots[i].GetComponent<StageSlot>().SetStageSlot( saveData[0].stageBestScore[i], saveData[0].stageStarCount[i], saveData[0].stageStatusNumber[i] );

                for (int i = 0; i < saveData[1].stageBestScore.Count; i++)
                    UIManager.instance.chapterSelectUI.chapter2_StageSlots[i].GetComponent<StageSlot>().SetStageSlot( saveData[1].stageBestScore[i], saveData[1].stageStarCount[i], saveData[1].stageStatusNumber[i] );

                for (int i = 0; i < saveData[2].stageBestScore.Count; i++)
                    UIManager.instance.chapterSelectUI.chapter3_StageSlots[i].GetComponent<StageSlot>().SetStageSlot( saveData[2].stageBestScore[i], saveData[2].stageStarCount[i], saveData[2].stageStatusNumber[i] );

                break;

            case 4:
                for (int i = 0; i < 4; i++)
                {
                    json = PlayerPrefs.GetString( saveChapters[i] );
                    saveData[i] = JsonUtility.FromJson<SaveData>( json );
                }

                for (int i = 0; i < saveData[0].stageBestScore.Count; i++)
                    UIManager.instance.chapterSelectUI.chapter1_StageSlots[i].GetComponent<StageSlot>().SetStageSlot( saveData[0].stageBestScore[i], saveData[0].stageStarCount[i], saveData[0].stageStatusNumber[i] );

                for (int i = 0; i < saveData[1].stageBestScore.Count; i++)
                    UIManager.instance.chapterSelectUI.chapter2_StageSlots[i].GetComponent<StageSlot>().SetStageSlot( saveData[1].stageBestScore[i], saveData[1].stageStarCount[i], saveData[1].stageStatusNumber[i] );

                for (int i = 0; i < saveData[2].stageBestScore.Count; i++)
                    UIManager.instance.chapterSelectUI.chapter3_StageSlots[i].GetComponent<StageSlot>().SetStageSlot( saveData[2].stageBestScore[i], saveData[2].stageStarCount[i], saveData[2].stageStatusNumber[i] );

                for (int i = 0; i < saveData[3].stageBestScore.Count; i++)
                    UIManager.instance.chapterSelectUI.chapter4_StageSlots[i].GetComponent<StageSlot>().SetStageSlot( saveData[3].stageBestScore[i], saveData[3].stageStarCount[i], saveData[3].stageStatusNumber[i] );

                break;

            case 5:
                for (int i = 0; i < 5; i++)
                {
                    json = PlayerPrefs.GetString( saveChapters[i] );
                    saveData[i] = JsonUtility.FromJson<SaveData>( json );
                }

                for (int i = 0; i < saveData[0].stageBestScore.Count; i++)
                    UIManager.instance.chapterSelectUI.chapter1_StageSlots[i].GetComponent<StageSlot>().SetStageSlot( saveData[0].stageBestScore[i], saveData[0].stageStarCount[i], saveData[0].stageStatusNumber[i] );

                for (int i = 0; i < saveData[1].stageBestScore.Count; i++)
                    UIManager.instance.chapterSelectUI.chapter2_StageSlots[i].GetComponent<StageSlot>().SetStageSlot( saveData[1].stageBestScore[i], saveData[1].stageStarCount[i], saveData[1].stageStatusNumber[i] );

                for (int i = 0; i < saveData[2].stageBestScore.Count; i++)
                    UIManager.instance.chapterSelectUI.chapter3_StageSlots[i].GetComponent<StageSlot>().SetStageSlot( saveData[2].stageBestScore[i], saveData[2].stageStarCount[i], saveData[2].stageStatusNumber[i] );

                for (int i = 0; i < saveData[3].stageBestScore.Count; i++)
                    UIManager.instance.chapterSelectUI.chapter4_StageSlots[i].GetComponent<StageSlot>().SetStageSlot( saveData[3].stageBestScore[i], saveData[3].stageStarCount[i], saveData[3].stageStatusNumber[i] );

                for (int i = 0; i < saveData[4].stageBestScore.Count; i++)
                    UIManager.instance.chapterSelectUI.chapter5_StageSlots[i].GetComponent<StageSlot>().SetStageSlot( saveData[4].stageBestScore[i], saveData[4].stageStarCount[i], saveData[4].stageStatusNumber[i] );
            
                break;
        }
    }

    public void DataReset()
    {
        PlayerPrefs.DeleteAll();
    }
}
