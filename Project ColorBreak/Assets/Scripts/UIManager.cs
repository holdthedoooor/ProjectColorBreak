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
    public Button               nextButton;
    public Sprite               starSprite; 
    public Sprite               blankStarSprite;
    public StageUI              stageUI;
    public GameOverUI           gameOverUI;
    public StageInformationUI   stageInformationUI;
    public LobbyUI              lobbyUI;
    public PauseUI              pauseUI;
    public ChapterSelectUI      chapterSelectUI;
    public BossStageUI          bossStageUI;
    public StageSlot[]          stageSlots; //현재 Chapter의 스테이지 슬롯들
    public BossStageSlot        bossStageSlot; //현재 Chapter의 보스 스테이지

    public int starCount { get; private set; }
    public int currentChapter;

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
                stageUI.starImages[2].sprite = starSprite;
                gameOverUI.stageStarImages[2].sprite = starSprite;
                starCount = 3;
            }
            else if (StageManager.instance.score >= StageManager.instance.currentStageSlot.checkPoints[1])
            {
                stageUI.starImages[1].sprite = starSprite;
                gameOverUI.stageStarImages[1].sprite = starSprite;
                starCount = 2;
            }
            else if (StageManager.instance.score >= StageManager.instance.currentStageSlot.checkPoints[0])
            {
                stageUI.starImages[0].sprite = starSprite;
                gameOverUI.stageStarImages[0].sprite = starSprite;
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
            gameOverUI.StageResetStar();
        }
        else
        {
            bossStageUI.ActivateUI();
            gameOverUI.BossStageResetStar();
        }
    }

    //게임이 끝날 때
    public void SetFinishUI()
    {
        //일반 스테이지라면
        StageSlot currentStageSlot = StageManager.instance.currentStageSlot;
        if ( currentStageSlot != null )
        {
            //starCount가 1개 이상이면 Stage Clear
            if (starCount > 0 && StageManager.instance.isGoal)
            {
                gameOverUI.gameoverText.text = "STAGE CLEAR!";
                gameOverUI.gameoverText.color = Color.blue;

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
            }
            else
            {
                gameOverUI.gameoverText.text = "GAME OVER";
                gameOverUI.gameoverText.color = Color.red;
            }

            if (StageManager.instance.currentStageSlot.stageNumber + 1 != stageSlots.Length)
            {
                if(stageSlots[StageManager.instance.currentStageSlot.stageNumber + 1] != null)
                {
                    if (stageSlots[StageManager.instance.currentStageSlot.stageNumber + 1].stageStatus != StageSlot.StageStatus.Rock)
                        nextButton.interactable = true;
                    else
                        nextButton.interactable = false;
                }
                else
                    nextButton.interactable = false;
            }
            else
            {
                if (bossStageSlot.bossStageStatus != BossStageSlot.BossStageStatus.Rock)
                    nextButton.interactable = true;
                else
                    nextButton.interactable = false;
            }

            gameOverUI.gameoverScoreText.text = StageManager.instance.score.ToString();
            stageUI.DeactivateUI();
            gameOverUI.go_StageGameoverUI.SetActive( true );
        }
        //보스 스테이지라면
        else
        {
            if(StageManager.instance.currentBossStageSlot.currentHp <= 0)
            {
                if (StageManager.instance.currentBossStageSlot.challengeCount <= StageManager.instance.currentBossStageSlot.checkChallengeCount[0])
                {
                    for (int i = 0; i < 3; i++)
                    {
                        gameOverUI.bossStageStarImages[i].sprite = starSprite;
                    }
                    starCount = 3;
                }
                else if (StageManager.instance.currentBossStageSlot.challengeCount <= StageManager.instance.currentBossStageSlot.checkChallengeCount[1])
                {
                    for (int i = 0; i < 2; i++)
                    {
                        gameOverUI.bossStageStarImages[i].sprite = starSprite;
                    }
                    starCount = 2;
                }
                else
                {
                    gameOverUI.bossStageStarImages[0].sprite = starSprite;
                    starCount = 1;
                }
                    
                gameOverUI.bossGameoverText.text = "BOSS CLEAR!";
                gameOverUI.bossGameoverText.color = Color.blue;
            }
            else
            {
                gameOverUI.bossGameoverText.text = "GAME OVER";
                gameOverUI.bossGameoverText.color = Color.red;
            }

            gameOverUI.SetGameoverUI();
            bossStageUI.DeactivateUI();
        }

    }

    //홈 버튼 클릭
    public void HomeButton()
    {
        

        if(StageManager.instance.currentStage != null)
        {
            gameOverUI.go_StageGameoverUI.SetActive( false );
            Destroy( StageManager.instance.currentStage.gameObject );
            stageUI.go_StageUI.SetActive( false );
            StageManager.instance.currentStage = null;
            StageManager.instance.currentStageSlot = null;
        }
        else
        {
            gameOverUI.go_BossStageGameoverUI.SetActive( false );
            Destroy( StageManager.instance.currentBossStage.gameObject );
            bossStageUI.go_BossStageUI.SetActive( false );
            StageManager.instance.currentBossStageSlot.challengeCount = 0;
            StageManager.instance.currentBossStage = null;
            StageManager.instance.currentBossStageSlot = null;
        }    

        StageManager.instance.go_Player.SetActive( false );
        chapterSelectUI.go_CurrentChapterUI.SetActive( true );
    }

    //현재 스테이지를 다시 플레이하는 버튼
    public void RestartButton()
    {
        
        StageManager.instance.go_Player.SetActive( false );
        if (StageManager.instance.currentStageSlot != null)
        {
            Destroy( StageManager.instance.currentStage.gameObject );
            StageManager.instance.currentStage = Instantiate( StageManager.instance.currentStageSlot.go_StagePrefab, new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<Stage>();
            gameOverUI.go_StageGameoverUI.SetActive( false );
        }
        else
        {
            Destroy( StageManager.instance.currentBossStage.gameObject );

            if(StageManager.instance.currentBossStageSlot.isRandom)
                StageManager.instance.currentBossStage = Instantiate( StageManager.instance.currentBossStageSlot.go_BossStageNormals[Random.Range(0, StageManager.instance.currentBossStageSlot.go_BossStageNormals.Length)]
                    , new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();
            else
                StageManager.instance.currentBossStage = Instantiate( StageManager.instance.currentBossStageSlot.go_BossStageNormals[0], new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();

            gameOverUI.go_BossStageGameoverUI.SetActive( false );
        }
    }

    //다음 스테이지로 넘어가는 버튼
    public void NextButton()
    {
        StageManager.instance.go_Player.SetActive( false );

        //일반 스테이지라면
        if(StageManager.instance.currentStageSlot != null)
        {
            gameOverUI.go_StageGameoverUI.SetActive( false );
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

                if(StageManager.instance.currentBossStageSlot.isRandom)
                    StageManager.instance.currentBossStage = Instantiate( StageManager.instance.currentBossStageSlot.go_BossStageNormals[Random.Range(0, StageManager.instance.currentBossStageSlot.go_BossStageNormals.Length)]
                        , new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();
                else
                    StageManager.instance.currentBossStage = Instantiate( StageManager.instance.currentBossStageSlot.go_BossStageNormals[0]
                        , new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();
            }
        }
        //보스 스테이지면
        else
        {
            gameOverUI.go_BossStageGameoverUI.SetActive( false );
            Destroy( StageManager.instance.currentBossStage.gameObject );
            StageManager.instance.currentBossStageSlot = null;
            StageManager.instance.currentBossStage = null;

            for (int i = 0; i < stageSlots.Length; i++)
            {
                stageSlots[i] = null;
            }

            currentChapter++;

            //다음 챕터의 슬롯들을 할당해준다.
            switch(currentChapter)
            {
                case 2:
                    for (int i = 0; i < chapterSelectUI.chapter2_StageSlots.Length; i++)
                    {
                        stageSlots[i] = chapterSelectUI.chapter2_StageSlots[i];
                    }
                    chapterSelectUI.go_CurrentChapterUI = chapterSelectUI.go_Chapters[1];
                    break;

                case 3:
                    for (int i = 0; i < chapterSelectUI.chapter3_StageSlots.Length; i++)
                    {
                        stageSlots[i] = chapterSelectUI.chapter3_StageSlots[i];
                    }
                    chapterSelectUI.go_CurrentChapterUI = chapterSelectUI.go_Chapters[2];
                    break;

                case 4:
                    for (int i = 0; i < chapterSelectUI.chapter4_StageSlots.Length; i++)
                    {
                        stageSlots[i] = chapterSelectUI.chapter4_StageSlots[i];
                    }
                    chapterSelectUI.go_CurrentChapterUI = chapterSelectUI.go_Chapters[3];
                    break;

                case 5:
                    for (int i = 0; i < chapterSelectUI.chapter5_StageSlots.Length; i++)
                    {
                        stageSlots[i] = chapterSelectUI.chapter5_StageSlots[i];
                    }
                    chapterSelectUI.go_CurrentChapterUI = chapterSelectUI.go_Chapters[4];
                    break;
            }

            //다음 챕터의 첫 번째 스테이지의 Status가 Rock이라면 Open해준다.
            stageSlots[0].StageSlotOpen();

            StageManager.instance.currentStageSlot = stageSlots[0];
            StageManager.instance.currentStage = Instantiate( StageManager.instance.currentStageSlot.go_StagePrefab, new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<Stage>();
        }
    }

    public void PauseButton()
    {
        pauseUI.go_PauseUI.SetActive( true );
        StageManager.instance.isPause = true;
        Time.timeScale = 0;
    }

    public void LoadToStageSlot(int _arrayNumber ,int _bestScore, int _starCount, int _statusNumber)
    {
        stageSlots[_arrayNumber].SetStageSlot( _bestScore, _starCount, _statusNumber );
    }
}
