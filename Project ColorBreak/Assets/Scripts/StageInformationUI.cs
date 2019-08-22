using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInformationUI : MonoBehaviour
{
    public GameObject   go_StageInformationUI;

    public Text         stageNumberText;
    public Text         bestScoreText;
    public Text[]       checkPointTexts;
    public Image[]      starImages;

    public void StageSelectCancelButton()
    {
        go_StageInformationUI.SetActive( false );
    }

    //최대 점수, Star Image, CheckPountText 초기화
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

    //클릭한 StageSlot의 정보에 맞게 설정
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

        stageNumberText.text = "STAGE " + (StageManager.instance.currentStageSlot.stageNumber + 1).ToString();

        go_StageInformationUI.SetActive( true );
    }

    public void StageStartButton()
    {
        UIManager.instance.stageSelectUI.go_StageSelectUI.SetActive( false );
        go_StageInformationUI.SetActive( false );
        StageManager.instance.currentStage.gameObject.SetActive( true );
    }
}
