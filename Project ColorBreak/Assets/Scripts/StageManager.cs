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
    public int          score;

    public GameObject   go_Player;
    //현재 스테이지
    public Stage        currentStage;
    public StageSlot    currentStageSlot;

    void Awake()
    {
        if (instance != this)
            Destroy( gameObject );
    }

    // 같은 ColorType의 장애물과 충돌하면 1점씩 추가
    public void AddScore( int _score = 1 )
    {
        if (!StageManager.instance.isGameOver)
        {
            score += _score;
            UIManager.instance.stageUI.UpdateScoreText( score );
            UIManager.instance.StarImageChange();
            StartCoroutine( UIManager.instance.stageUI.UpdateScoreSliderCoroutine( score, currentStage.checkPoints[2] ) );
        }
    }
    /*void Start()
    {
        if (currentStage)
        {
            currentStage.StartStage();
        }
    }*/
}
