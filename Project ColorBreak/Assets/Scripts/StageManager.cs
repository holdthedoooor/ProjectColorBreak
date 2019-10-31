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
    [Header("체크 - 저장한 데이터 초기화, 체크해제 - 저장한 데이터 불러옴")]
    public bool         isDataReset = false;
    public bool         isGameOver;
    public bool         isPause;
    public bool         isGoal;
    //보스 스테이지를 시작했을 때 애니메이션이 끝난 뒤에 이동
    public bool         isBossStageStart = false;
    public int          score; //일반 스테이지에서의 점수
    public int          damage;//보스 스테이지에서의 데미지
    public int          panaltyPoint;
    public int          currentChapter;
    public int[]        chaptersUnlockStarCount;
    //현재 챕터의 별 총 개수
    public int          chapterStarCount;
    public GameObject        go_Player;
    public GameObject[]      go_AddScoreEffects;
    public GameObject[]      go_PlayerDieEffects;
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

        if (isDataReset)
            theSaveLoad.DataReset();

        for (int i = 0; i < UIManager.instance.chapterSelectUI.allStageSlot.Length; i++)
        {
            UIManager.instance.chapterSelectUI.allStageSlot[i].stageSlots = UIManager.instance.chapterSelectUI.go_SlotParents[i].GetComponentsInChildren<StageSlot>();
        }

        if (isMasterMode)
        {
            for (int i = 0; i < UIManager.instance.chapterSelectUI.allStageSlot.Length; i++)
            {
                for (int j = 0; j < UIManager.instance.chapterSelectUI.allStageSlot[i].stageSlots.Length; j++)
                {
                    UIManager.instance.chapterSelectUI.allStageSlot[i].stageSlots[j].SetOpen();
                }
                if (UIManager.instance.chapterSelectUI.allStageSlot[i].bossStageSlot != null)
                    UIManager.instance.chapterSelectUI.allStageSlot[i].bossStageSlot.SetOpen();
            }

            UIManager.instance.chapterSelectUI.chapterUnlock = 5;
            UIManager.instance.chapterSelectUI.LoadChapterOpen();
        }
        else
            theSaveLoad.LoadData();

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
                UIManager.instance.stageUI.updateSliderCoroutine = StartCoroutine( UIManager.instance.stageUI.UpdateScoreSliderCoroutine() );
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
        
        StartCoroutine( UIManager.instance.bossStageUI.UpdateBossHpSliderCoroutine() );
        if (currentBossStage.bossStageType == BossStage.BossStageType.Normal)
        {
            if(currentBossStageSlot.currentHp <= currentBossStageSlot.hardHp)
                 NextPhase();
            else
            {
                panaltyPoint++;
                Destroy(currentBossStage.gameObject );
                if(currentBossStageSlot.isRandom)
                    currentBossStage = Instantiate( currentBossStageSlot.go_BossStageNormals[Random.Range(0, currentBossStageSlot.go_BossStageNormals.Length)]
                        , new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();
                else
                    currentBossStage = Instantiate( currentBossStageSlot.go_BossStageNormals[0], new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();

                currentBossStage.go_BossPrefab.SetActive( true );

                go_Player.SetActive( false );
                go_Player.SetActive( true );
                
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
                panaltyPoint++;

                currentBossStage = Instantiate( currentBossStageSlot.go_BossStageHard, new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();

                go_Player.SetActive( false );
                go_Player.SetActive( true );

                UIManager.instance.bossStageUI.UpdateChallengeCountText();
            } 
        }

        UIManager.instance.bossStageUI.UpdateBossHpText();
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

        score = 0;
        UIManager.instance.SetStartUI();

        isGameOver = false;
        go_Player.SetActive( true );

        //isMovable를 true로 바꿔줌으로 써 카메라가 다시 움직이도록 설정
        Camera.main.GetComponent<FollowCamera>().SetCamera();
        StartCoroutine( Camera.main.GetComponent<FollowCamera>().PastPlayerCoroutine() );
        Quit.instance.quitStatus = Quit.QuitStatus.InGame;
    }

    public void StartBossStage()
    {
        if (isPause)
        {
            isPause = false;
            Time.timeScale = 1;
        }

        damage = 0;
        UIManager.instance.bossStageUI.UpdateDamageText();

        if (panaltyPoint == 0)
        {
            isBossStageStart = true;
            currentBossStage.startBossStageCoroutine = StartCoroutine( currentBossStage.StartBossStageCoroutine() );
            currentBossStageSlot.currentHp = currentBossStageSlot.maxHp;
            panaltyPoint++;
            UIManager.instance.bossStageUI.UpdateChallengeCountText();
            UIManager.instance.SetStartUI();
        }

        isGameOver = false;
        go_Player.SetActive( true );

        Camera.main.GetComponent<FollowCamera>().SetCamera();
        StartCoroutine( Camera.main.GetComponent<FollowCamera>().PastPlayerCoroutine() );
        Quit.instance.quitStatus = Quit.QuitStatus.InGame;
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
                    ChapterOpenCheck();
                    theSaveLoad.SaveData();
                }
            }
        }
        //보스 스테이지면
        else
        {
            UIManager.instance.SetFinishUI();

            if (currentBossStageSlot.starCount < UIManager.instance.starCount)
            {
                currentBossStageSlot.starCount = UIManager.instance.starCount;
                ChapterOpenCheck();
                if (currentBossStageSlot.minPanaltyPoint > panaltyPoint)
                    currentBossStageSlot.minPanaltyPoint = panaltyPoint;
                theSaveLoad.SaveData();
            }
            else if(currentBossStageSlot.starCount == UIManager.instance.starCount)
            {
                if (currentBossStageSlot.minPanaltyPoint > panaltyPoint)
                    currentBossStageSlot.minPanaltyPoint = panaltyPoint;
                theSaveLoad.SaveData();
            }
                
            currentBossStageSlot.StarImageChange();
            panaltyPoint = 0;
        }

        go_Player.SetActive( false );
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

    public void ChapterOpenCheck()
    {
        chapterStarCount = 0;

        for (int i = 0; i < UIManager.instance.stageSlots.Length; i++)
        {
            chapterStarCount += UIManager.instance.stageSlots[i].starCount;
        }
        chapterStarCount += UIManager.instance.bossStageSlot.starCount;

        if (UIManager.instance.chapterSelectUI.chapterUnlock == currentChapter)
        {   
            if (chapterStarCount >= chaptersUnlockStarCount[currentChapter - 1] && UIManager.instance.bossStageSlot.bossStageStatus == BossStageSlot.BossStageStatus.Clear)
            {
                //다음 챕터 오픈
                UIManager.instance.chapterSelectUI.NextChapterOpen();
                //다음 챕터의 1스테이지 오픈
                UIManager.instance.chapterSelectUI.allStageSlot[currentChapter].stageSlots[0].StageSlotOpen();
                UIManager.instance.chapterSelectUI.chapterUnlock++;
            }
        }
    }
}
