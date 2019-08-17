using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public int      score { get; private set; }
    public bool     isGameOver { get; private set; }

    public int      checkPoint_1;
    public int      checkPoint_2;
    public int      checkPoint_3;

    // 스테이지 시작 시 실행
    public void StartStage()
    {
        score = 0;
        isGameOver = false;
        UIManager.instance.SetStartUI();
    }

    // 스테이지를 통과 시 실행
    public void FinishStage()
    {
        isGameOver = true;
        UIManager.instance.SetFinishUI();
        UIManager.instance.SetActiveGameClearUI( true );
    }

    public void AddScore(int _score = 1)
    {
        if(!isGameOver)
        {
            score += _score;
            UIManager.instance.UpdateScoreText( score );
            StartCoroutine( UIManager.instance.UpdateScoreSliderCoroutine( score, checkPoint_3 ) );
            UIManager.instance.StarColorChange();
        }
    }

    //장애물에 부딪혀서 플레이어가 죽었을 때 실행
    public void EndGame()
    {
        isGameOver = true;
        UIManager.instance.SetActiveGameOverUI( true );
    }
}
