using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StageClearUI : MonoBehaviour
{
    public Text       scoreText;

    public GameObject go_S_ClearUI;
    public GameObject go_BS_ClearUI;

    public Image[]    s_StarImages;
    public Text[]     checkPointTexts;

    public Image[]    bs_StarImages;
    public Image      resultBossHpSlider;
    public Text       hpPersentText;
    public Text       panaltyPointText;
    public Text[]     panaltyTexts;

    public void S_ResetStar()
    {
        for (int i = 0; i < s_StarImages.Length; i++)
        {
            s_StarImages[i].sprite = UIManager.instance.blankStarSprite;
            checkPointTexts[i].text = StageManager.instance.currentStageSlot.checkPoints[i].ToString();
        }
    }

    public void S_SetStar()
    {
        for (int i = 0; i < UIManager.instance.starCount; i++)
        {
            s_StarImages[i].sprite = UIManager.instance.starSprite;
        }
    }

    public void S_ActiveClearUI()
    {
        S_ResetStar();
        S_SetStar();
        scoreText.text = StageManager.instance.score.ToString();
        go_S_ClearUI.SetActive( true );
    }

    public void S_DeactiveClearUI()
    {
        go_S_ClearUI.SetActive( false );
    }

    public void BS_ResetStar()
    {
        for (int i = 0; i < bs_StarImages.Length; i++)
        {
            bs_StarImages[i].sprite = UIManager.instance.blankStarSprite;
        }

        for (int i = 0; i < panaltyTexts.Length; i++)
        {
            panaltyTexts[i].text = StageManager.instance.currentBossStageSlot.panaltyPoints[i].ToString();
        }
    }

    public void BS_SetStar()
    {
        for (int i = 0; i < UIManager.instance.starCount; i++)
        {
            bs_StarImages[i].sprite = UIManager.instance.starSprite;
        }
    }

    public void BS_ActiveClearUI()
    {
        BS_ResetStar();
        BS_SetStar();
        resultBossHpSlider.fillAmount = (float)StageManager.instance.currentBossStageSlot.currentHp / StageManager.instance.currentBossStageSlot.maxHp;
        hpPersentText.text = (Math.Truncate( (float)StageManager.instance.currentBossStageSlot.currentHp / StageManager.instance.currentBossStageSlot.maxHp * 100 )).ToString() + "%";
        panaltyPointText.text = StageManager.instance.panaltyPoint.ToString();
        go_BS_ClearUI.SetActive( true );
    }

    public void BS_DeactiveClearUI()
    {
        go_BS_ClearUI.SetActive( false );
    }
}
