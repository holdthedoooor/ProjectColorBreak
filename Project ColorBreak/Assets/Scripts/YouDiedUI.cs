using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class YouDiedUI : MonoBehaviour
{
    public Text         scoreText;

    public GameObject   go_S_YouDiedUI;
    public GameObject   go_BS_YouDiedUI;

    public Image[]      s_StarImages;
    public Text[]       checkPointTexts;

    public Image[]      bs_StarImages;
    public Image        resultBossHpSlider;
    public Text         hpPersentText;
    public Text         panaltyPointText;
    public Text[]       panaltyTexts;

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
        if(UIManager.instance.starCount == 3)
        {
            for (int i = 0; i < s_StarImages.Length; i++)
            {
                s_StarImages[i].sprite = UIManager.instance.starSprite;
            }
        }
        else if(UIManager.instance.starCount == 2)
        {
            s_StarImages[0].sprite = UIManager.instance.starSprite;
            s_StarImages[1].sprite = UIManager.instance.starSprite;
        }
        else if(UIManager.instance.starCount == 1)
            s_StarImages[0].sprite = UIManager.instance.starSprite;
    }

    public void S_ActiveYouDiedUI()
    {
        S_ResetStar();
        S_SetStar();
        scoreText.text = StageManager.instance.score.ToString();
        go_S_YouDiedUI.SetActive( true );
    }

    public void S_DeactiveYouDiedUI()
    {
        go_S_YouDiedUI.SetActive( false );
    }


    public void BS_ResetStar()
    {
        for (int i = 0; i < bs_StarImages.Length; i++)
        {
            bs_StarImages[i].sprite = UIManager.instance.blankStarSprite;
        }
    }

    public void BS_SetStar()
    {
        for (int i = 0; i < UIManager.instance.starCount; i++)
        {
            bs_StarImages[i].sprite = UIManager.instance.starSprite;
        }
    }

    public void BS_ActiveYouDiedUI()
    {
        resultBossHpSlider.fillAmount = (float)StageManager.instance.currentBossStageSlot.currentHp / StageManager.instance.currentBossStageSlot.maxHp;
        hpPersentText.text = (Math.Truncate((float)StageManager.instance.currentBossStageSlot.currentHp / StageManager.instance.currentBossStageSlot.maxHp * 100)).ToString() + "%";
        panaltyPointText.text = StageManager.instance.panaltyPoint.ToString();
        for (int i = 0; i < panaltyTexts.Length; i++)
        {
            panaltyTexts[i].text = StageManager.instance.currentBossStageSlot.panaltyPoints[i].ToString();
        }
        go_BS_YouDiedUI.SetActive( true );
    }

    public void BS_DeactiveYouDiedUI()
    {
        go_BS_YouDiedUI.SetActive( false );
    }
}
