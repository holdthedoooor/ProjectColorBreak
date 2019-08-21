using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Text         gameOverScoreText;
    public Text         gameOverText;

    public GameObject   go_GameOverUI;

    public Image[]      starImages;
    public Text[]       checkPointTexts;

    public void ResetStar()
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = UIManager.instance.blankStarSprite;
            checkPointTexts[i].enabled = true;
            checkPointTexts[i].text = StageManager.instance.currentStage.checkPoints[i].ToString();
        }
    }
}
