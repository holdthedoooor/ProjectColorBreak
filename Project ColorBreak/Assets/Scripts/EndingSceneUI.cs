using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneUI : MonoBehaviour
{
    public GameObject go_EndingSceneUI;
    public Transform endingCreditPos;

    public bool isEndingCredit = false;

    private Vector3 orizinPos;

    public Coroutine coroutine;

    void Awake()
    {
        orizinPos = endingCreditPos.localPosition;
    }

    public IEnumerator EndingCreditCoroutine()
    {
        isEndingCredit = true;

        float _time = 0f;

        go_EndingSceneUI.SetActive( true );

        endingCreditPos.localPosition = orizinPos;

        Debug.Log( endingCreditPos.localPosition );

        //1625

        while(_time < 10f)
        {
            _time += Time.deltaTime;

            endingCreditPos.localPosition = new Vector3( 0, Mathf.Lerp( -1401f, 1625f, _time / 10f ), 0 );

            yield return null;
        }

        go_EndingSceneUI.SetActive( false );

        isEndingCredit = false;
    }

    public void EndingSceneButton()
    {
        Quit.instance.ActivePopUp( go_EndingSceneUI, Quit.QuitStatus.ChapterSelect );
        coroutine = StartCoroutine( EndingCreditCoroutine() );
    }
}
