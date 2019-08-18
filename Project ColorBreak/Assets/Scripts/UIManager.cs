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

    public Text         scoreText;
    public Text         gameOverScoreText;
    public Text         gameOverText;
    public Button       nextButton;
    public Image        scoreSlider;
    private StarImage[] starImages;
    private Image[]     gameOverStarImages;
    public GameObject   scoreUI;
    public GameObject   scoreSliderUI;
    public GameObject   starImageUI;
    public GameObject   gameOverStarImageUI;
    public GameObject   gameOverUI;

    void Awake()
    {
        starImages = starImageUI.GetComponentsInChildren<StarImage>();
        gameOverStarImages = gameOverStarImageUI.GetComponentsInChildren<Image>();
    }

    public void UpdateScoreText(int _score)
    {
        scoreText.text = _score.ToString();
    }

    public void StarColorChange()
    {
        if(StageManager.instance.stage.score == StageManager.instance.stage.checkPoint_1)
            starImages[0].ChangeColorYellow();
        else if (StageManager.instance.stage.score == StageManager.instance.stage.checkPoint_2)
            starImages[1].ChangeColorYellow();
        else if (StageManager.instance.stage.score == StageManager.instance.stage.checkPoint_3)
            starImages[2].ChangeColorYellow();
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

        for (int i = 0; i < gameOverStarImages.Length; i++)
        {
            gameOverStarImages[i].color = Color.gray;
        }

        if(StageManager.instance.stage.score >= StageManager.instance.stage.checkPoint_1)
        {
            gameOverText.text = "Stage Clear";
            gameOverText.color = Color.blue;
            gameOverStarImages[0].color = Color.yellow;
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
            gameOverStarImages[1].color = Color.yellow;
            gameOverStarImages[2].color = Color.yellow;
        }
        else if (StageManager.instance.stage.score >= StageManager.instance.stage.checkPoint_2)
        {
            gameOverStarImages[1].color = Color.yellow;            
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
