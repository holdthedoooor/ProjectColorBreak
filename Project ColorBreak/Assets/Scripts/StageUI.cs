using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    public GameObject    go_StageUI;
    public Text          scoreText;
    public Image         scoreSlider;
    public Sprite[]      sliderSprites;
    public Image[]       starImages;
    public Text[]        starTexts;

    private float ratio;
    private float minPositionX = -400f;
    private float maxPositionX = 400f;

    //현재 스코어 갱신
    public void UpdateScoreText( int _score )
    {
        scoreText.text = _score.ToString();
    }

    public IEnumerator UpdateScoreSliderCoroutine( )
    {
        while (scoreSlider.fillAmount < StageManager.instance.score / (float)StageManager.instance.currentStageSlot.checkPoints[2])
        {
            scoreSlider.fillAmount += Time.deltaTime;

            yield return null;
        }
    }

    private void ResetStar()
    {
        if(StageManager.instance.currentStageSlot != null)
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
    }

    public void ActivateUI()
    {
        scoreSlider.sprite = sliderSprites[StageManager.instance.currentChapter - 1];
        ResetStar();
        UpdateScoreText( 0 );
        scoreSlider.fillAmount = 0f;
        go_StageUI.SetActive( true );
    }

    public void DeactivateUI()
    {
        go_StageUI.SetActive( false );
        StopCoroutine( UpdateScoreSliderCoroutine() );
    }
}
