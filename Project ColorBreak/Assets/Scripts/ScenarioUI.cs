using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioUI : MonoBehaviour
{
    public GameObject       go_ScenarioUI;
    public Image[]          scenario_Images;
    public Image            text_Image;

    public Coroutine coroutine;

    public bool isCoroutineRun = false;

    private Vector3 text_OrizinPos;

    private void Awake()
    {
        text_OrizinPos = text_Image.transform.localPosition;
    }

    public void SkipButton()
    {
        if(isCoroutineRun)
        {
            UIManager.instance.logoUI.StopCoroutine( coroutine );

            go_ScenarioUI.SetActive( false );

            UIManager.instance.lobbyUI.go_LobbyUI.SetActive( true );
            UIManager.instance.lobbyUI.coroutine1 = StartCoroutine( UIManager.instance.lobbyUI.TextShake() );
            UIManager.instance.lobbyUI.coroutine2 = StartCoroutine( UIManager.instance.lobbyUI.SlimeRotation() );

            isCoroutineRun = false;
        }   
    }

    public IEnumerator ScenarioCoroutine()
    {
        isCoroutineRun = true;

        go_ScenarioUI.SetActive( true );

        int _num = 0;
        float _time = 0;
        Color _color;

        while (_num < 3)
        {
            while (_time < 0.5f)
            {
                _time += Time.deltaTime;

                scenario_Images[_num].transform.localScale = Vector3.Lerp( new Vector3( 0.1f, 0.1f, 1f ), new Vector3( 1f, 1f, 1f ), _time / 0.5f );

                _color = scenario_Images[_num].color;
                _color.a = Mathf.Lerp( 0, 1, _time / 0.5f );
                scenario_Images[_num].color = _color;

                yield return null;
            }

            _num++;
            _time = 0;

            yield return new WaitForSeconds( 0.5f );
        }

        text_Image.enabled = true;
        _time = 0;

        while(_time < 3f)
        {
            _time += Time.deltaTime;

            text_Image.transform.localPosition = text_OrizinPos + (Random.insideUnitSphere * 15);
            text_Image.transform.localPosition = new Vector3( text_Image.transform.localPosition.x, text_Image.transform.localPosition.y, 0 );

            yield return null;
        }

        go_ScenarioUI.SetActive( false );

        UIManager.instance.lobbyUI.go_LobbyUI.SetActive( true );
        UIManager.instance.lobbyUI.coroutine1 = StartCoroutine( UIManager.instance.lobbyUI.TextShake() );
        UIManager.instance.lobbyUI.coroutine2 = StartCoroutine( UIManager.instance.lobbyUI.SlimeRotation() );

        isCoroutineRun = false;

        yield return null;
    }
}
