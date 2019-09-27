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

    public bool         isMasterMode = false;
    public bool         isGameOver;
    public bool         isPause;
    public bool         isGoal;
    public int          score; //일반 스테이지에서의 점수
    public int          damage;//보스 스테이지에서의 데미지

    public GameObject        go_Player;
    //현재 스테이지   
    public Stage             currentStage;
    public StageSlot         currentStageSlot;
    public BossStage         currentBossStage;
    public BossStageSlot     currentBossStageSlot;
    public SaveLoad          theSaveLoad;

    void Awake()
    {
        if (instance != this)
            Destroy( gameObject );

        if(isMasterMode)
        {
            for (int i = 0; i < UIManager.instance.chapterSelectUI.chapter1_StageSlots.Length; i++)
            {
                UIManager.instance.chapterSelectUI.chapter1_StageSlots[i].SetOpen();
            }

            for (int i = 0; i < UIManager.instance.chapterSelectUI.chapter2_StageSlots.Length; i++)
            {
                UIManager.instance.chapterSelectUI.chapter2_StageSlots[i].SetOpen();
            }

            for (int i = 0; i < UIManager.instance.chapterSelectUI.chapter3_StageSlots.Length; i++)
            {
                UIManager.instance.chapterSelectUI.chapter3_StageSlots[i].SetOpen();
            }

            for (int i = 0; i < UIManager.instance.chapterSelectUI.chapter4_StageSlots.Length; i++)
            {
                UIManager.instance.chapterSelectUI.chapter4_StageSlots[i].SetOpen();
            }

            for (int i = 0; i < UIManager.instance.chapterSelectUI.chapter5_StageSlots.Length; i++)
            {
                UIManager.instance.chapterSelectUI.chapter5_StageSlots[i].SetOpen();
            }

            UIManager.instance.chapterSelectUI.chapter1_BossStageSlot.SetOpen();
        }
    }

    // 같은 ColorType의 장애물과 충돌하면 1점씩 추가
    public void AddScoreAndDamage( int _num = 1 )
    {
        if (!isGameOver)
        {
            if(currentStageSlot != null)
            {
                score += _num;
                UIManager.instance.stageUI.UpdateScoreText( score );
                UIManager.instance.StarImageChange();
                StartCoroutine( UIManager.instance.stageUI.UpdateScoreSliderCoroutine( score, currentStageSlot.checkPoints[2] ) );
            }
            else
            {
                damage += _num;
                UIManager.instance.bossStageUI.UpdateDamageText();
            }
        }
    }

   /* // 보스 스테이지에서 같은 ColoType의 장애물과 충돌하면 데미지가 1씩 축적된다.
    public void AddDamage(int _damage = 1)
    {
        if (!isGameOver)
        {
            damage += _damage;
            UIManager.instance.bossStageUI.UpdateDamageText( damage );
            UIManager.instance.StarImageChange();
        }
    }*/

    //플레이어가 보스와 충돌했을 때 실행
    public void BossCollision()
    {
        currentBossStageSlot.currentHp -= damage;
        UIManager.instance.bossStageUI.UpdateBossHpText();
        StartCoroutine( UIManager.instance.bossStageUI.UpdateBossHpSliderCoroutine() );
        if (currentBossStage.bossStageType == BossStage.BossStageType.Normal)
        {
            if(currentBossStageSlot.currentHp <= currentBossStageSlot.hardHp)
                 NextPhase();
            else
            {
                Destroy(currentBossStage.gameObject );
                if(currentBossStageSlot.isRandom)
                    currentBossStage = Instantiate( currentBossStageSlot.go_BossStageNormals[Random.Range(0, currentBossStageSlot.go_BossStageNormals.Length)]
                        , new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();
                else
                    currentBossStage = Instantiate( currentBossStageSlot.go_BossStageNormals[0], new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();

                go_Player.SetActive( false );
                go_Player.SetActive( true );
                currentBossStageSlot.challengeCount++;
                UIManager.instance.bossStageUI.UpdateChallengeCountText();
            }
        }
        else
        { 
            //게임 종료
            if (currentBossStageSlot.currentHp <= 0)
            {
                currentBossStageSlot.currentHp = 0;
                FinishStage();
            }
            else
            {
                Destroy( currentBossStage.gameObject );
                currentBossStage = Instantiate( currentBossStageSlot.go_BossStageHard, new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();
                go_Player.SetActive( false );
                go_Player.SetActive( true );
                currentBossStageSlot.challengeCount++;
                UIManager.instance.bossStageUI.UpdateChallengeCountText();
            } 
        }
    }

    // 활성화 되면 즉 스테이지 시작 시 실행
    public void StartStage()
    {
        if (isPause)
        {
            isPause = false;
            Time.timeScale = 1;
        }

        isGoal = false;
        if(currentStageSlot != null)
        {
            score = 0;
            UIManager.instance.SetStartUI();
        }
            
        else
        {
            damage = 0;
            if(currentBossStageSlot.challengeCount == 0)
            {
                currentBossStageSlot.currentHp = currentBossStageSlot.maxHp;
                currentBossStageSlot.challengeCount++;
                UIManager.instance.bossStageUI.UpdateChallengeCountText();
                UIManager.instance.SetStartUI();
            }         
        }
        
        isGameOver = false;
        go_Player.SetActive( true );

        //isMovable를 true로 바꿔줌으로 써 카메라가 다시 움직이도록 설정
        Camera.main.GetComponent<FollowCamera>().SetCamera();
    }

    // 스테이지를 통과 시 실행
    public void FinishStage()
    {
        isGameOver = true;

        if(currentStageSlot != null)
        {
            UIManager.instance.SetFinishUI();

            if (isGoal)
            {
                //여기에서 오류 발생하는것같다
                //현재 점수가 현재 스테이지에서 달성한 최대 점수보다 크다면 최대 점수 변경
                if (score > currentStageSlot.bestScore)
                {
                    currentStageSlot.starCount = UIManager.instance.starCount;
                    currentStageSlot.StarImageChange();
                    currentStageSlot.bestScore = score;
                    theSaveLoad.SaveData();
                }
            }
        }
        else
        {
            UIManager.instance.SetFinishUI();
            if (currentBossStageSlot.starCount < UIManager.instance.starCount)
                currentBossStageSlot.starCount = UIManager.instance.starCount;
            currentBossStageSlot.StarImageChange();
            currentBossStageSlot.challengeCount = 0;
        }
    }

    public void NextPhase()
    {
        //현재 페이즈 스테이지 오브젝트 제거
        Destroy(currentBossStage.gameObject );

        //BossStageHard 생성 후 currentBossStage에 BossStageHard를 할당
        currentBossStage = Instantiate( currentBossStageSlot.go_BossStageHard, new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();

        //플레이어 위치 및 색 초기화
        go_Player.SetActive( false );
        go_Player.SetActive( true );
    }
}
