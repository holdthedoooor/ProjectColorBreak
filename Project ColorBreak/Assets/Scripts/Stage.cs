using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public int[]         checkPoints;

    public Obstacle[]    obstacles;
    public Item[]        items;

    void Awake()
    {
        obstacles = transform.GetComponentsInChildren<Obstacle>();
        items = transform.GetComponentsInChildren<Item>();
    }

    void OnEnable()
    {
        StartStage();
    }

    // 활성화 되면 즉 스테이지 시작 시 실행
    public void StartStage()
    {
        if(StageManager.instance.isPause)
        {
            StageManager.instance.isPause = false;
            Time.timeScale = 1;
        }

        StageManager.instance.isGoal = false;
        StageManager.instance.score = 0;
        StageManager.instance.isGameOver = false;
        StageManager.instance.go_Player.SetActive( true );

        //isMovable를 true로 바꿔줌으로 써 카메라가 다시 움직이도록 설정
        StageManager.instance.go_Player.GetComponent<PlayerCtrl>().followCamera.isMovable = true;
        UIManager.instance.SetStartUI();

        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].gameObject.SetActive( false );
            obstacles[i].gameObject.SetActive( true );
        }

        for (int i = 0; i < items.Length; i++)
        {
            items[i].gameObject.SetActive( false );
            items[i].gameObject.SetActive( true );
        }
    }

    // 스테이지를 통과 시 실행
    public void FinishStage()
    {
        StageManager.instance.isGameOver = true;

        UIManager.instance.SetFinishUI();

        if(StageManager.instance.isGoal)
        {
            //현재 점수가 현재 스테이지에서 달성한 최대 점수보다 크다면 최대 점수 변경
            if (StageManager.instance.score > StageManager.instance.currentStageSlot.bestScore)
            {
                StageManager.instance.currentStageSlot.starCount = UIManager.instance.starCount;
                StageManager.instance.currentStageSlot.StarImageChange();
                StageManager.instance.currentStageSlot.bestScore = StageManager.instance.score;
            }
        }

        Debug.Log( "BestScore : " + StageManager.instance.currentStageSlot.bestScore + "  StarCount : " + StageManager.instance.currentStageSlot.starCount );
        StageManager.instance.theSaveLoad.SaveData();
    }
}
