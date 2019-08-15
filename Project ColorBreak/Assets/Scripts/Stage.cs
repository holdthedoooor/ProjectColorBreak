using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private int     score;
    public bool     isGameOver { get; private set; }

    // 스테이지 시작 시 실행
    public void StartStage()
    {
        score = 0;
        isGameOver = false;
    }

    // 스테이지를 통과 시 실행
    public void FinishStage()
    {
        isGameOver = true;
        UIManager.instance.SetActiveGameClearUI( true );
    }

    public void AddScore(int _score = 1)
    {
        if(!isGameOver)
        {
            score += _score;
            UIManager.instance.UpdateScoreText( score );
        }
    }

    //장애물에 부딪혀서 플레이어가 죽었을 때 실행
    public void EndGame()
    {
        isGameOver = true;
        UIManager.instance.SetActiveGameOverUI( true );
    }
}
