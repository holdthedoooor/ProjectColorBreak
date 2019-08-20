using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageInformationUI : MonoBehaviour
{
    public GameObject   go_StageInformationUI;

    public Text         bestScoreText;
    public Text[]       checkPointTexts;
    public Image[]      starImages;

    //스테이지 선택 버튼
    public void StageSelectButton()
    {
        StageManager.instance.currentStageSlot = EventSystem.current.currentSelectedGameObject.GetComponent<StageSlot>();
        StageManager.instance.currentStage = StageManager.instance.currentStageSlot.stagePrefab.GetComponent<Stage>();
        if (StageManager.instance.currentStageSlot.stageStatus != StageSlot.StageStatus.Rock)
            return;

        ResetStageInformation();
        SetStageInformation();
    }

    public void ResetStageInformation()
    {
        bestScoreText.text = StageManager.instance.currentStageSlot.bestScore.ToString();

        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = UIManager.instance.blankStarSprite;
            checkPointTexts[i].text = StageManager.instance.currentStage.checkPoints[i].ToString();
            checkPointTexts[i].enabled = true;
        }
    }

    public void SetStageInformation()
    {
        if (StageManager.instance.currentStageSlot.starCount > 0)
        {
            for (int i = 0; i < StageManager.instance.currentStageSlot.starCount; i++)
            {
                starImages[i].sprite = UIManager.instance.starSprite;
            }
        }

        if (StageManager.instance.currentStageSlot.starCount == 3)
        {
            for (int i = 0; i < checkPointTexts.Length; i++)
            {
                checkPointTexts[i].enabled = false;
            }
        }
        else if (StageManager.instance.currentStageSlot.starCount == 2)
        {
            checkPointTexts[0].enabled = false;
            checkPointTexts[1].enabled = false;
        }
        else if (StageManager.instance.currentStageSlot.starCount == 1)
            checkPointTexts[0].enabled = false;

        go_StageInformationUI.SetActive( true );
    }

    public void StageStartButton()
    {
        UIManager.instance.stageSelectUI.go_StageSelectUI.SetActive( false );
        go_StageInformationUI.SetActive( false );
        StageManager.instance.currentStage.gameObject.SetActive( true );
    }
}
