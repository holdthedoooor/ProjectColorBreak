using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }
            return m_instance;
        }
    }
    private static UIManager m_instance; //싱글톤이 할당될 변수

    //나중에 기능별로 나눌 예정
    public Sprite               starSprite; //팝업창에 쓰이는 별 활성화 이미지
    public Sprite               star2Sprite; //스테이지 슬롯과 슬라이더에 쓰일 별 활성화 이미지
    public Sprite               blankStarSprite;
    public Sprite               blankStar2Sprite;
    public Button               nextButton; //보스 스테이지 StageClear UI의 Next 버튼
    public StageUI              stageUI;
    public YouDiedUI            youDiedUI;
    public StageClearUI         stageClearUI;
    public StageInformationUI   stageInformationUI;
    public BossStageInformationUI bossStageInformationUI;
    public LobbyUI              lobbyUI;
    public PauseUI              pauseUI;
    public ChapterSelectUI      chapterSelectUI;
    public BossStageUI          bossStageUI;
    public QuitUI               quitUI;
    public ScenarioUI           scenarioUI;
    public LogoUI               logoUI;
    public StageSlot[]          stageSlots; //현재 Chapter의 스테이지 슬롯들
    public BossStageSlot        bossStageSlot; //현재 Chapter의 보스 스테이지

    public int starCount;


    void Awake()
    {
        stageSlots = chapterSelectUI.go_Chapters[0].GetComponentsInChildren<StageSlot>();
    }

    //체크 포인트에 도달할 때마다 슬라이더의 Star Image와 gameOver의 Star Image를 변경
    //도달할 때마다 해당 스테이지 슬롯의 starCount를 증가;
    public void StarImageChange()
    {
        if(StageManager.instance.currentStageSlot != null)
        {
            if (StageManager.instance.score >= StageManager.instance.currentStageSlot.checkPoints[2])
            {
                for (int i = 0; i < 3; i++)
                {
                    stageUI.starImages[i].sprite = star2Sprite;
                }
                starCount = 3;
            }
            else if (StageManager.instance.score >= StageManager.instance.currentStageSlot.checkPoints[1])
            {
                for (int i = 0; i < 2; i++)
                {
                    stageUI.starImages[i].sprite = star2Sprite;
                }
                starCount = 2;
            }
            else if (StageManager.instance.score >= StageManager.instance.currentStageSlot.checkPoints[0])
            {
                stageUI.starImages[0].sprite = star2Sprite;
                starCount = 1;
            }
        }
    }

    //게임이 시작할 때
    public void SetStartUI()
    {
        starCount = 0;
        if (StageManager.instance.currentStageSlot != null)
        {
            stageUI.ActivateUI();
        }
        else
        {
            bossStageUI.ActivateUI();
        }
    }

    //게임이 끝날 때
    public void SetFinishUI()
    {
        //일반 스테이지라면
        StageSlot currentStageSlot = StageManager.instance.currentStageSlot;
        if ( currentStageSlot != null )
        {
            stageUI.DeactivateUI();
            //starCount가 1개 이상이면 Stage Clear
            if (starCount > 0 && StageManager.instance.isGoal)
            {
                //만약 현재 stageSlot이 Open만 된 상태였다면 Clear로 변경
                if ( currentStageSlot.stageStatus == StageSlot.StageStatus.Open)
                {
                    currentStageSlot.stageStatus = StageSlot.StageStatus.Clear;

                    //현재 Stage를 Clear 했으니 다음 Stage를 Open 시켜준다.
                    int stageIdx = currentStageSlot.stageNumber + 1;
                    if ( stageIdx != stageSlots.Length )
                    {
                        if ( stageSlots[ stageIdx ] == null )
                            Debug.Log( "다음 스테이지 슬롯이 비어있습니다. 스테이지를 추가해주세요." );
                        else
                            stageSlots[ stageIdx ].StageSlotOpen();
                    }
                    else
                    {
                        //보스 스테이지 슬롯 오픈
                        bossStageSlot.BossStageSlotOpen();
                    }
                }
                stageClearUI.S_ActiveClearUI();
            }
            //클리어 못했을 시
            else
            {
                youDiedUI.S_ActiveYouDiedUI();
            }
            
        }
        //보스 스테이지라면
        else
        {
            bossStageUI.DeactivateUI();

            if (StageManager.instance.currentBossStageSlot.currentHp <= 0 && StageManager.instance.panaltyPoint > 0)
            {
                if (StageManager.instance.panaltyPoint >= StageManager.instance.currentBossStageSlot.panaltyPoints[0] - StageManager.instance.currentBossStageSlot.panaltyPoints[1])
                    starCount = 3;
                else if (StageManager.instance.panaltyPoint > 0 && StageManager.instance.panaltyPoint < StageManager.instance.currentBossStageSlot.panaltyPoints[0] - StageManager.instance.currentBossStageSlot.panaltyPoints[1])
                    starCount = 2;
                else if (StageManager.instance.panaltyPoint > 0)
                    starCount = 1;

                if (StageManager.instance.currentBossStageSlot.bossStageStatus == BossStageSlot.BossStageStatus.Open)
                    StageManager.instance.currentBossStageSlot.bossStageStatus = BossStageSlot.BossStageStatus.Clear;

                if (StageManager.instance.chaptersStarCount[StageManager.instance.currentChapter - 1] >= StageManager.instance.chaptersUnlockStarCount[StageManager.instance.currentChapter - 1])
                    nextButton.interactable = true;
                else
                    nextButton.interactable = false;

                stageClearUI.BS_ActiveClearUI();
            }
            else
                youDiedUI.BS_ActiveYouDiedUI();
        }
        StageManager.instance.go_Player.SetActive( false );
    }

    //홈 버튼 클릭
    public void HomeButton()
    {
        if (StageManager.instance.currentStage != null)
        {
            stageUI.DeactivateUI();
            if (starCount > 0 && StageManager.instance.isGoal)
                stageClearUI.S_DeactiveClearUI();
            else
                youDiedUI.S_DeactiveYouDiedUI();
            Destroy( StageManager.instance.currentStage.gameObject );
            StageManager.instance.currentStage = null;
            StageManager.instance.currentStageSlot = null;
        }
        else
        {
            if (StageManager.instance.isBossStageStart)
                StageManager.instance.currentBossStage.StopCoroutine( StageManager.instance.currentBossStage.coroutine );
            bossStageUI.DeactivateUI();
            if (StageManager.instance.currentBossStageSlot.currentHp <= 0)
                stageClearUI.BS_DeactiveClearUI();
            else
                youDiedUI.BS_DeactiveYouDiedUI();
            Destroy( StageManager.instance.currentBossStage.gameObject );
            StageManager.instance.currentBossStage = null;
            StageManager.instance.currentBossStageSlot = null;
        }

        pauseUI.go_PauseUI.SetActive( false );
        StageManager.instance.go_Player.SetActive( false );
        chapterSelectUI.go_CurrentChapterUI.SetActive( true );
        Quit.instance.quitStatus = Quit.QuitStatus.StageSelect;
    }

    //현재 스테이지를 다시 플레이하는 버튼
    public void RestartButton()
    {
        StageManager.instance.go_Player.SetActive( false );
        if (StageManager.instance.currentStage != null)
        {
            if (starCount > 0 && StageManager.instance.isGoal)
                stageClearUI.S_DeactiveClearUI();
            else
                youDiedUI.S_DeactiveYouDiedUI();

            Destroy( StageManager.instance.currentStage.gameObject );
            StageManager.instance.currentStage = Instantiate( StageManager.instance.currentStageSlot.go_StagePrefab, new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<Stage>();       
        }
        else
        {
            if (StageManager.instance.currentBossStageSlot.currentHp <= 0)
                stageClearUI.BS_DeactiveClearUI();
            else
                youDiedUI.BS_DeactiveYouDiedUI();

            Destroy( StageManager.instance.currentBossStage.gameObject );
            StageManager.instance.panaltyPoint = 0;
            if (StageManager.instance.currentBossStageSlot.bossStageType == BossStageSlot.BossStageType.StageRandom)
                StageManager.instance.currentBossStage = Instantiate( StageManager.instance.currentBossStageSlot.go_BossStageNormals[Random.Range(0, StageManager.instance.currentBossStageSlot.go_BossStageNormals.Length)]
                    , new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();
            else
                StageManager.instance.currentBossStage = Instantiate( StageManager.instance.currentBossStageSlot.go_BossStageNormals[0], new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();
        }
        pauseUI.go_PauseUI.SetActive( false );
    }

    //다음 스테이지로 넘어가는 버튼
    public void NextButton()
    {
        StageManager.instance.go_Player.SetActive( false );

        //일반 스테이지라면
        if(StageManager.instance.currentStageSlot != null)
        {
            stageClearUI.S_DeactiveClearUI();

            Destroy( StageManager.instance.currentStage.gameObject );

            //다음 스테이지가 일반 스테이지라면
            if (StageManager.instance.currentStageSlot.stageNumber + 1 != stageSlots.Length)
            {
                StageManager.instance.currentStageSlot = stageSlots[StageManager.instance.currentStageSlot.stageNumber + 1];
                StageManager.instance.currentStage = Instantiate( StageManager.instance.currentStageSlot.go_StagePrefab, new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<Stage>();
            }
            //다음 스테이지가 보스 스테이지라면
            else
            {
                StageManager.instance.currentStageSlot = null;
                StageManager.instance.currentStage = null;
                StageManager.instance.currentBossStageSlot = bossStageSlot;
                StageManager.instance.panaltyPoint = 0;

                if(StageManager.instance.currentBossStageSlot.bossStageType == BossStageSlot.BossStageType.StageRandom)
                    StageManager.instance.currentBossStage = Instantiate( StageManager.instance.currentBossStageSlot.go_BossStageNormals[Random.Range(0, StageManager.instance.currentBossStageSlot.go_BossStageNormals.Length)]
                        , new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();
                else
                    StageManager.instance.currentBossStage = Instantiate( StageManager.instance.currentBossStageSlot.go_BossStageNormals[0]
                        , new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();

                if (StageManager.instance.currentBossStageSlot.currentHp <= 0)
                    stageClearUI.BS_DeactiveClearUI();
                else
                    youDiedUI.BS_DeactiveYouDiedUI();
            }
        }
        //보스 스테이지면
        else
        {
            stageClearUI.BS_DeactiveClearUI();
            Destroy( StageManager.instance.currentBossStage.gameObject );
            StageManager.instance.currentBossStageSlot = null;
            StageManager.instance.currentBossStage = null;

            for (int i = 0; i < stageSlots.Length; i++)
            {
                stageSlots[i] = null;
            }

            StageManager.instance.currentChapter++;

            //다음 챕터의 슬롯들을 할당해준다.
            for (int i = 0; i < chapterSelectUI.allStageSlot[StageManager.instance.currentChapter - 1].stageSlots.Length; i++)
            {
                stageSlots[i] = chapterSelectUI.allStageSlot[StageManager.instance.currentChapter - 1].stageSlots[i];
            }
            chapterSelectUI.go_CurrentChapterUI = chapterSelectUI.go_Chapters[StageManager.instance.currentChapter - 1];
            bossStageSlot = chapterSelectUI.allStageSlot[StageManager.instance.currentChapter - 1].bossStageSlot;

            StageManager.instance.currentStageSlot = stageSlots[0];
            StageManager.instance.currentStage = Instantiate( StageManager.instance.currentStageSlot.go_StagePrefab, new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<Stage>();
        }
    }

    public void PauseButton()
    {
        pauseUI.go_PauseUI.SetActive( true );
        StageManager.instance.isPause = true;
        Time.timeScale = 0;
        Quit.instance.quitStatus = Quit.QuitStatus.StopGame;
    }
}
