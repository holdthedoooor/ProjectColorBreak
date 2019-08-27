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
    public StageSelectUI        stageSelectUI;
    public StageInformationUI   stageInformationUI;
    public LobbyUI              lobbyUI;
    public PauseUI              pauseUI;
    public ChapterSelectUI      chapterSelectUI;
    public StageSlot[]          stageSlots;

    public int starCount { get; private set; }

    void Awake()
    {
        stageSlots = chapterSelectUI.go_Chapters[0].GetComponentsInChildren<StageSlot>();
    }

    //체크 포인트에 도달할 때마다 슬라이더의 Star Image와 gameOver의 Star Image를 변경
    //도달할 때마다 해당 스테이지 슬롯의 starCount를 증가;ㅑ
    public void StarImageChange()
    {
        if (StageManager.instance.score == StageManager.instance.currentStageSlot.checkPoints[0])
        {
            stageUI.starImages[0].sprite = starSprite;
            gameOverUI.starImages[0].sprite = starSprite;
            starCount = 1;
        }
        else if (StageManager.instance.score == StageManager.instance.currentStageSlot.checkPoints[1])
        {
            stageUI.starImages[1].sprite = starSprite;
            gameOverUI.starImages[1].sprite = starSprite;
            starCount = 2;
        }
        else if (StageManager.instance.score == StageManager.instance.currentStageSlot.checkPoints[2])
        {
            stageUI.starImages[2].sprite = starSprite;
            gameOverUI.starImages[2].sprite = starSprite;
            starCount = 3;
        }
    }

    //게임이 시작할 때
    public void SetStartUI()
    {
        starCount = 0;
        stageUI.ActivateUI();
        gameOverUI.ResetStar();
    }

    //게임이 끝날 때
    public void SetFinishUI()
    {
        //starCount가 1개 이상이면 Stage Clear
        if (UIManager.instance.starCount > 0 && StageManager.instance.isGoal)
        {
            gameOverUI.gameOverText.text = "Stage Clear";
            gameOverUI.gameOverText.color = Color.blue;

            //만약 현재 stageSlot이 Open만 된 상태였다면 Clear로 변경
            if(StageManager.instance.currentStageSlot.stageStatus == StageSlot.StageStatus.Open)
            {
                StageManager.instance.currentStageSlot.stageStatus = StageSlot.StageStatus.Clear;

                //현재 Stage를 Clear 했으니 다음 Stage를 Open 시켜준다.
                if(StageManager.instance.currentStageSlot.stageNumber + 1 != stageSlots.Length)
                     stageSlots[StageManager.instance.currentStageSlot.stageNumber + 1].StageSlotOpen();
            }
        }
        else
        {
            gameOverUI.gameOverText.text = "Game Over";
            gameOverUI.gameOverText.color = Color.red;
        }

        if(StageManager.instance.currentStageSlot.stageNumber + 1 != stageSlots.Length)
        {
            if (stageSlots[StageManager.instance.currentStageSlot.stageNumber + 1].stageStatus != StageSlot.StageStatus.Rock)
                nextButton.interactable = true;
            else
                nextButton.interactable = false;
        }
        else
            nextButton.interactable = false;

        gameOverUI.gameOverScoreText.text = StageManager.instance.score.ToString();
        gameOverUI.go_GameOverUI.SetActive( true );
    }

    //홈 버튼 클릭
    public void HomeButton()
    {
        stageUI.DeactivateUI();
        gameOverUI.go_GameOverUI.SetActive( false );
        Destroy( StageManager.instance.currentStage.gameObject );
        StageManager.instance.go_Player.SetActive( false );
        chapterSelectUI.go_CurrentChapterUI.SetActive( true );
    }

    //현재 스테이지를 다시 플레이하는 버튼
    public void RestartButton()
    {
        gameOverUI.go_GameOverUI.SetActive( false );
        StageManager.instance.go_Player.SetActive( false );
        StageManager.instance.currentStage.gameObject.SetActive( false );
        StageManager.instance.currentStage.gameObject.SetActive( true );
    }

    //다음 스테이지로 넘어가는 버튼
    public void NextButton()
    {
        gameOverUI.go_GameOverUI.SetActive( false );
        StageManager.instance.go_Player.SetActive( false );
        Destroy( StageManager.instance.currentStage.gameObject );
        StageManager.instance.currentStageSlot = stageSlots[StageManager.instance.currentStageSlot.stageNumber + 1];
        StageManager.instance.currentStage = Instantiate(StageManager.instance.currentStageSlot.go_StagePrefab,new Vector3(0,0,0),Quaternion.identity).GetComponent<Stage>();
    }

    //스테이지 선택 UI로 가는 버튼
    public void LobbyStartButton()
    {
        lobbyUI.go_LobbyUI.SetActive( false );
        chapterSelectUI.go_ChapterSelectUI.SetActive( true );
    }

    public void LoadToStageSlot(int _arrayNumber ,int _bestScore, int _starCount, int _statusNumber)
    {
        stageSlots[_arrayNumber].SetStageSlot( _bestScore, _starCount, _statusNumber );
    }
}
