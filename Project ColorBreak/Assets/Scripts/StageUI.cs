using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    public GameObject    go_StageUI;
    public Text          scoreText;
    public Image         scoreSlider;
    public Image[]       starImages;
    public Text[]        starTexts;

    private float ratio;
    private float minPositionX = -365f;
    private float maxPositionX = 365f;

    //현재 스코어 갱신
    public void UpdateScoreText( int _score )
    {
        scoreText.text = _score.ToString();
    }

    public IEnumerator UpdateScoreSliderCoroutine( int _score, int _checkPoint_3 )
    {
        while (scoreSlider.fillAmount < _score / (float)_checkPoint_3)
        {
            scoreSlider.fillAmount += Time.deltaTime;

            yield return null;
        }
    }

    private void ResetStar()
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            ratio = StageManager.instance.currentStageSlot.checkPoints[i] / (float)StageManager.instance.currentStageSlot.checkPoints[2];
            starTexts[i].enabled = true;
            starTexts[i].text = StageManager.instance.currentStageSlot.checkPoints[i].ToString();
            starImages[i].transform.localPosition = new Vector3( Mathf.Lerp( minPositionX, maxPositionX, ratio ), starImages[i].transform.localPosition.y, starImages[i].transform.localPosition.z );
            starImages[i].sprite = UIManager.instance.blankStarSprite;
        }
    }

    public void ActivateUI()
    {
        ResetStar();
        UpdateScoreText( 0 );
        go_StageUI.SetActive( true );
        scoreSlider.fillAmount = 0f;
    }

    public void DeactivateUI()
    {
        go_StageUI.SetActive( false );
    }

    public void PauseButton()
    {
        UIManager.instance.pauseUI.go_PauseUI.SetActive( true );
        StageManager.instance.isPause = true;
        Time.timeScale = 0;
    }
}
