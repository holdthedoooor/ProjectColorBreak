using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public Stage stage;

    // Start is called before the first frame update
    void Start()
    {
        if ( stage )
        {
            stage.StartStage();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
