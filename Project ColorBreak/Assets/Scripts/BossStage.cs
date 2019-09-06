using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : MonoBehaviour
{
    public enum BossStageType
    {
        Normal,
        Hard
    }
    public BossStageType bossStageType;

    public Obstacle[] obstacles;
    public Item[] items;

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
        if (StageManager.instance.isPause)
        {
            StageManager.instance.isPause = false;
            Time.timeScale = 1;
        }
        StageManager.instance.damage = 0;
        StageManager.instance.isGameOver = false;
        StageManager.instance.go_Player.SetActive( true );

        //isMovable를 true로 바꿔줌으로 써 카메라가 다시 움직이도록 설정
        Camera.main.GetComponent<FollowCamera>().SetCamera();
        UIManager.instance.SetStartUI();
    }
}
