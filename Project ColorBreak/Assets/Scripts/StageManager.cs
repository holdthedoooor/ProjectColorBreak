using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<StageManager>();
            }
            return m_instance;
        }
    }
    private static StageManager m_instance; //싱글톤이 할당될 변수

    public bool         isGameOver;

    public GameObject   go_Player;
    //현재 스테이지
    public Stage        currentStage;
    public StageSlot    currentStageSlot;

    void Awake()
    {
        if (instance != this)
            Destroy( gameObject );
    }

    /*void Start()
    {
        if (currentStage)
        {
            currentStage.StartStage();
        }
    }*/
}
