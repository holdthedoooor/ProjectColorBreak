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

    void Awake()
    {
        stageSlots = go_StageSlotsParent.transform.GetComponentsInChildren<StageSlot>();
    }

    //나중에 기능별로 나눌 예정
    public GameObject           go_StageSlotsParent;
    public Button               nextButton;
    public Sprite               starSprite;
    public Sprite               blankStarSprite;
    public StageUI              stageUI;
    public GameOverUI           gameOverUI;
    public StageSelectUI        stageSelectUI;
    public StageInformationUI   stageInformationUI;
    public LobbyUI              lobbyUI;
    private StageSlot[]         stageSlots;

    //체크 포인트에 도달할 때마다 StarImage를 변경
    public void StarImageChange()
    {
        if (StageManager.instance.currentStage.score == StageManager.instance.currentStage.checkPoints[0])
        {
            stageUI.starImages[0].sprite = starSprite;
            gameOverUI.starImages[0].sprite = starSprite;
            if (StageManager.instance.currentStageSlot.starCount < 1)
                StageManager.instance.currentStageSlot.starCount = 1;
        }
        else if (StageManager.instance.currentStage.score == StageManager.instance.currentStage.checkPoints[1])
        {
            stageUI.starImages[1].sprite = starSprite;
            gameOverUI.starImages[1].sprite = starSprite;
            if (StageManager.instance.currentStageSlot.starCount < 2)
                StageManager.instance.currentStageSlot.starCount = 2;
        }
        else if (StageManager.instance.currentStage.score == StageManager.instance.currentStage.checkPoints[2])
        {
            stageUI.starImages[2].sprite = starSprite;
            gameOverUI.starImages[2].sprite = starSprite;
            if (StageManager.instance.currentStageSlot.starCount < 3)
                StageManager.instance.currentStageSlot.starCount = 3;
        }
    }

    //게임이 시작할 때
    public void SetStartUI()
    {
        stageUI.ActivateUI();
        gameOverUI.ResetStar();
    }

    //게임이 끝날 때
    public void SetFinishUI()
    {
        if (StageManager.instance.currentStageSlot.starCount > 0)
        {
            gameOverUI.gameOverText.text = "Stage Clear";
            gameOverUI.gameOverText.color = Color.blue;
            if(StageManager.instance.currentStageSlot.stageStatus == StageSlot.StageStatus.Open)
            {
                StageManager.instance.currentStageSlot.stageStatus = StageSlot.StageStatus.Clear;
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
        
        StageManager.instance.currentStageSlot.StageSlotChange();
        gameOverUI.gameOverScoreText.text = StageManager.instance.currentStage.score.ToString();
        gameOverUI.go_GameOverUI.SetActive( true );
    }

    //홈 버튼 클릭
    public void HomeButton()
    {
        stageUI.DeactivateUI();
        gameOverUI.go_GameOverUI.SetActive( false );
        StageManager.instance.currentStage.gameObject.SetActive( false );
        StageManager.instance.go_Player.SetActive( false );
        stageSelectUI.go_StageSelectUI.SetActive( true );
    }

    public void RestartButton()
    {
        gameOverUI.go_GameOverUI.SetActive( false );
        StageManager.instance.go_Player.SetActive( false );
        StageManager.instance.currentStage.gameObject.SetActive( false );
        StageManager.instance.currentStage.gameObject.SetActive( true );
    }

    public void NextButton()
    {
        gameOverUI.go_GameOverUI.SetActive( false );
        StageManager.instance.go_Player.SetActive( false );
        StageManager.instance.currentStage.gameObject.SetActive( false );
        StageManager.instance.currentStageSlot = stageSlots[StageManager.instance.currentStageSlot.stageNumber + 1];
        StageManager.instance.currentStage = StageManager.instance.currentStageSlot.go_StagePrefab.GetComponent<Stage>();
        StageManager.instance.currentStage.gameObject.SetActive( true );
    }

    //스테이지 선택 UI로 가는 버튼
    public void LobbyStartButton()
    {
        lobbyUI.go_LobbyUI.SetActive( false );
        stageSelectUI.go_StageSelectUI.SetActive( true );
    }
}
