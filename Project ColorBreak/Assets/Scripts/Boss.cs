using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : LivingEntity
{
    private void Start()
    {
        StartCoroutine( ScaleY() );
    }

    public IEnumerator ScaleY()
    {
        float _time;
        
        while(!StageManager.instance.isGameOver)
        {
            _time = 0;

            while (_time < 0.5f)
            {
                _time += Time.deltaTime;

                transform.localScale = Vector3.Lerp( new Vector3( 1, 1, 1 ), new Vector3( 1, 1.2f, 1 ), _time / 0.5f );

                yield return null;
            }

            _time = 0;

            while (_time < 0.5f)
            {
                _time += Time.deltaTime;

                transform.localScale = Vector3.Lerp( new Vector3( 1, 1.2f, 1 ), new Vector3( 1, 1, 1 ), _time / 0.5f );

                yield return null;
            }
        }
    }
}
