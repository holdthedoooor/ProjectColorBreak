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
    public GameObject   go_BS2_YouDiedUI; //Bounce Attack 패턴 UI

    public Image[]      s_StarImages;
    public Text[]       checkPointTexts;

    public Image[]      bs_StarImages;
    public Image        resultBossHpSlider;
    public Text         hpPersentText;
    public Text         panaltyPointText;
    public Text[]       panaltyTexts;

    //Bounce Attack 패턴 UI
    public Image        resultBossHpSlider2;
    public Text         hpPersentText2;

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
        if(StageManager.instance.currentBossStageSlot.bossStageType != BossStageSlot.BossStageType.BounceAttack)
        {
            BS_ResetStar();
            BS_SetStar();
            resultBossHpSlider.fillAmount = (float)StageManager.instance.currentBossStageSlot.currentHp / StageManager.instance.currentBossStageSlot.maxHp;
            hpPersentText.text = (Math.Truncate( (float)StageManager.instance.currentBossStageSlot.currentHp / StageManager.instance.currentBossStageSlot.maxHp * 100 )).ToString() + "%";
            panaltyPointText.text = StageManager.instance.panaltyPoint.ToString();
            for (int i = 0; i < panaltyTexts.Length; i++)
            {
                panaltyTexts[i].text = StageManager.instance.currentBossStageSlot.panaltyPoints[i].ToString();
            }
            go_BS_YouDiedUI.SetActive( true );
        }
        else
        {
            resultBossHpSlider2.fillAmount = (float)StageManager.instance.currentBossStageSlot.currentHp / StageManager.instance.currentBossStageSlot.maxHp;
            hpPersentText2.text = (Math.Truncate( (float)StageManager.instance.currentBossStageSlot.currentHp / StageManager.instance.currentBossStageSlot.maxHp * 100 )).ToString() + "%";
            go_BS2_YouDiedUI.SetActive( true );
        }

    }

    public void BS_DeactiveYouDiedUI()
    {
        if (StageManager.instance.currentBossStageSlot.bossStageType != BossStageSlot.BossStageType.BounceAttack)
            go_BS_YouDiedUI.SetActive( false );
        else
            go_BS2_YouDiedUI.SetActive( false );
    }
}
