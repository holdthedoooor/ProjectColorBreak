using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public int      score { get; private set; }
    public int[]    checkPoints;
    

    public Obstacle[] obstacles;

    void Awake()
    {
        obstacles = transform.GetComponentsInChildren<Obstacle>();
    }

    void OnEnable()
    {
        StartStage();
    }

    // 스테이지 시작 시 실행
    public void StartStage()
    {
        Debug.Log( "스타트" );
        score = 0;
        StageManager.instance.isGameOver = false;
        StageManager.instance.go_Player.SetActive( true );
        StageManager.instance.go_Player.GetComponent<PlayerCtrl>().followCamera.isMovable = true;
        UIManager.instance.SetStartUI();
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].gameObject.SetActive( false );
            obstacles[i].gameObject.SetActive( true );
        }
    }

    // 스테이지를 통과 시 실행
    public void FinishStage()
    {
        StageManager.instance.isGameOver = true;
        UIManager.instance.SetFinishUI();
        if (score > StageManager.instance.currentStageSlot.bestScore)
            StageManager.instance.currentStageSlot.bestScore = score;

    }

    public void AddScore(int _score = 1)
    {
        if(!StageManager.instance.isGameOver)
        {
            score += _score;
            UIManager.instance.stageUI.UpdateScoreText( score );
            UIManager.instance.StarImageChange();
            StartCoroutine( UIManager.instance.stageUI.UpdateScoreSliderCoroutine( score, checkPoints[2]) );
        }
    }
}
