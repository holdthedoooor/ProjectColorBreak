using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<StageManager>();
            }
            return m_instance;
        }
    }
    private static StageManager m_instance; //싱글톤이 할당될 변수

    public Stage stage;

    void Awake()
    {
        if (instance != this)
            Destroy( gameObject );
    }
    // Start is called before the first frame update
    void Start()
    {
        if ( stage )
        {
            stage.StartStage();
        }
    }
}
