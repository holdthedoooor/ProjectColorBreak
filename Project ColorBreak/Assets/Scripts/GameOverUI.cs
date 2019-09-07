using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameOverUI : MonoBehaviour
{
    public Text         gameoverScoreText;
    public Text         gameoverText;

    public GameObject   go_StageGameoverUI;
    public GameObject   go_BossStageGameoverUI;

    public Image[]      stageStarImages;
    public Text[]       checkPointTexts;

    public Image[]      bossStageStarImages;
    public Image        resultBossHpSlider;
    public Text         bossGameoverText;
    public Text         hpPersentText;
    public Text         panaltyPointText;

    public void StageResetStar()
    {
        for (int i = 0; i < stageStarImages.Length; i++)
        {
            stageStarImages[i].sprite = UIManager.instance.blankStarSprite;
            checkPointTexts[i].enabled = true;
            checkPointTexts[i].text = StageManager.instance.currentStageSlot.checkPoints[i].ToString();
        }
    }

    public void BossStageResetStar()
    {
        for (int i = 0; i < bossStageStarImages.Length; i++)
        {
            bossStageStarImages[i].sprite = UIManager.instance.blankStarSprite;
        }
    }

    public void SetGameoverUI()
    {
        resultBossHpSlider.fillAmount = (float)StageManager.instance.currentBossStageSlot.currentHp / StageManager.instance.currentBossStageSlot.maxHp;
        hpPersentText.text = (Math.Truncate((float)StageManager.instance.currentBossStageSlot.currentHp / StageManager.instance.currentBossStageSlot.maxHp * 100)).ToString() + "%";
        panaltyPointText.text = StageManager.instance.currentBossStageSlot.challengeCount.ToString();
        go_BossStageGameoverUI.SetActive( true );
    }
}
