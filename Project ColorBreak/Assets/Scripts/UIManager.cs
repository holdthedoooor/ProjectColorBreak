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
    public GameObject   gameOverUI;
    public GameObject   gameClearUI;

    public void UpdateScoreText(int _score)
    {
        scoreText.text = "Score : " + _score;
    }

    public void SetActiveGameOverUI(bool _active)
    {
        gameOverUI.SetActive( _active );
    }

    public void SetActiveGameClearUI(bool _active)
    {
        gameClearUI.SetActive( _active );
    }
}
