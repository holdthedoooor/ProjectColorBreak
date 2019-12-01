using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoUI : MonoBehaviour
{
    public GameObject go_Background;
    public Image logo_Image;
    
    void Start()
    {
        Debug.Log( "시작" );
        StartCoroutine( LogoCoroutine() );
    }
    
    public IEnumerator LogoCoroutine()
    {
        float _time = 0;
        float _alpha = 0;
        Color c = logo_Image.color;

        while (_time < 1.5f)
        {
            Debug.Log( "로고" );
            _time += Time.deltaTime;

            _alpha = Mathf.Lerp( 0f, 1f, _time / 1.5f );

            c.a = _alpha;
            logo_Image.color = c;

            yield return null;
        }

        yield return new WaitForSeconds( 0.5f );

        _time = 0;

        while (_time < 1.5f)
        {
            _time += Time.deltaTime;

            _alpha = Mathf.Lerp( 1f, 0f, _time / 1.5f );

            c.a = _alpha;
            logo_Image.color = c;

            yield return null;
        }

        yield return new WaitForSeconds( 0.2f );

        go_Background.SetActive( false );


        UIManager.instance.scenarioUI.coroutine = StartCoroutine( UIManager.instance.scenarioUI.ScenarioCoroutine() );
    }
}
