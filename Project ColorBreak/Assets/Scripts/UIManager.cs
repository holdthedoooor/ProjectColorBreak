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

    //나중에 간편하게 관리
    public Text         scoreText;
    public Text         gameOverScoreText;
    public Text         gameOverText;
    public Button       nextButton;
    public Image        scoreSlider;
    private StarImage[] starImages;
    public ResultStar   resultStar;
    public GameObject   scoreUI;
    public GameObject   scoreSliderUI;
    public GameObject   starImageUI;
    public GameObject   gameOverUI;

    // ============== 변수 선언 =============================================//
    void Awake()
    {
        starImages = starImageUI.GetComponentsInChildren<StarImage>();
    }

    public void UpdateScoreText(int _score)
    {
        scoreText.text = _score.ToString();
    }

    public void StarColorChange()
    {
        if(StageManager.instance.stage.score == StageManager.instance.stage.checkPoint_1)
            starImages[0].ChangeStar();
        else if (StageManager.instance.stage.score == StageManager.instance.stage.checkPoint_2)
            starImages[1].ChangeStar();
        else if (StageManager.instance.stage.score == StageManager.instance.stage.checkPoint_3)
            starImages[2].ChangeStar();
    }

    public void SetStartUI()
    {
        UpdateScoreText( 0 );
        starImageUI.SetActive( true );
        scoreSliderUI.SetActive( true );
        scoreUI.SetActive( true );
    }

    public void SetFinishUI()
    {
        scoreSliderUI.SetActive( false );
        scoreUI.SetActive( false );

        for (int i = 0; i < resultStar.starImages.Length; i++)
        {
            resultStar.starImages[i].sprite = resultStar.blankStarSprite;
        }

        if(StageManager.instance.stage.score >= StageManager.instance.stage.checkPoint_1)
        {
            gameOverText.text = "Stage Clear";
            gameOverText.color = Color.blue;
            resultStar.starImages[0].sprite = resultStar.starSprite;
            nextButton.interactable = true;
        }
        else
        {
            gameOverText.text = "Game Over";
            gameOverText.color = Color.red;
            nextButton.interactable = false;
        }

        if (StageManager.instance.stage.score >= StageManager.instance.stage.checkPoint_3)
        {
            resultStar.starImages[1].sprite = resultStar.starSprite;
            resultStar.starImages[2].sprite = resultStar.starSprite;
        }
        else if (StageManager.instance.stage.score >= StageManager.instance.stage.checkPoint_2)
        {
            resultStar.starImages[1].sprite = resultStar.starSprite;
        }
        
        gameOverScoreText.text = StageManager.instance.stage.score.ToString();
        gameOverUI.SetActive( true );
    }

    public IEnumerator UpdateScoreSliderCoroutine( int _score, int _checkPoint_3 )
    {
        while(scoreSlider.fillAmount < _score / (float)_checkPoint_3)
        {
            scoreSlider.fillAmount += Time.deltaTime;

            yield return null;
        }
    }
}
