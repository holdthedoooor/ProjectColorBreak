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
    public Image        scoreSlider;
    private StarImage[] starImages;
    public GameObject   scoreUI;
    public GameObject   scoreSliderUI;
    public GameObject   starImageUI;
    public GameObject   gameOverUI;
    public GameObject   gameClearUI;

    void Awake()
    {
        starImages = starImageUI.GetComponentsInChildren<StarImage>();
    }

    public void UpdateScoreText(int _score)
    {
        scoreText.text = _score.ToString();
    }

    public void SetActiveGameOverUI(bool _active)
    {
        gameOverUI.SetActive( _active );
    }

    public void SetActiveGameClearUI(bool _active)
    {
        gameClearUI.SetActive( _active );
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
