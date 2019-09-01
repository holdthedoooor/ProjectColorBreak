using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public List<int> stageArrayNumber = new List<int>();
    public List<int> stageBestScore = new List<int>();
    public List<int> stageStarCount = new List<int>();
    public List<int> satgeStatusNumber = new List<int>();
}

public class SaveLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "SaveFile.txt";

    private void Start()
    {
        SAVE_DATA_DIRECTORY = Application.persistentDataPath + "/Saves/";

        if(!Directory.Exists(SAVE_DATA_DIRECTORY))
        {
            Directory.CreateDirectory( SAVE_DATA_DIRECTORY );
        }
    }

    public void SaveData()
    {
        saveData.stageArrayNumber.Clear();
        saveData.stageBestScore.Clear();
        saveData.stageStarCount.Clear();
        saveData.satgeStatusNumber.Clear();

        for (int i = 0; i < UIManager.instance.stageSlots.Length; i++)
        {
            if (UIManager.instance.stageSlots[i].stageStatus != StageSlot.StageStatus.Rock)
            {
                saveData.stageArrayNumber.Add( i );
                saveData.stageBestScore.Add( UIManager.instance.stageSlots[i].bestScore );
                saveData.stageStarCount.Add( UIManager.instance.stageSlots[i].starCount );
                saveData.satgeStatusNumber.Add( (int)UIManager.instance.stageSlots[i].stageStatus );
            }
        }

        string json = JsonUtility.ToJson( saveData, true );

        File.WriteAllText( SAVE_DATA_DIRECTORY + SAVE_FILENAME, json );

        Debug.Log( json );
    }

    public bool LoadData()
    {
        if (File.Exists( SAVE_DATA_DIRECTORY + SAVE_FILENAME ))
        {
            string check = File.ReadAllText( SAVE_DATA_DIRECTORY + SAVE_FILENAME );

            if (check != "")
            {
                string loadJson = File.ReadAllText( SAVE_DATA_DIRECTORY + SAVE_FILENAME );
                saveData = JsonUtility.FromJson<SaveData>( loadJson );

                for (int i = 0; i < saveData.stageArrayNumber.Count; i++)
                {
                    UIManager.instance.LoadToStageSlot( saveData.stageArrayNumber[i], saveData.stageBestScore[i], saveData.stageStarCount[i], saveData.satgeStatusNumber[i] );
                }

                Debug.Log( "로드 성공" );
                return true;
            }
            else
            {
                Debug.Log( "저장된 데이터가 없습니다." );
                return false;
            }  
        }
        else
        {
            Debug.Log( "세이브 파일이 없습니다." );
            return false;
        }
    }

    public void ResetData()
    {
        File.WriteAllText( SAVE_DATA_DIRECTORY + SAVE_FILENAME, "" );
    }
}
