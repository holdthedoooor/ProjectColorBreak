﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : MonoBehaviour
{
    public enum BossStageType
    {
        Normal,
        Hard
    }
    public BossStageType bossStageType;

    public Obstacle[] obstacles;
    public Item[] items;
    //보스가 등잘할 때 보여주는 애니메이션
    public GameObject go_AppearAnimation;
    //애니메이션이 끝나면 활성화
    public GameObject go_BossPrefab;
    private Animator   boss_Animator;

    public Coroutine coroutine;
    public Coroutine coroutine2;

    public int coroutineNum;

    void Awake()
    {
        obstacles = transform.GetComponentsInChildren<Obstacle>();
        items = transform.GetComponentsInChildren<Item>();
    }

    void Start()
    {
        if (StageManager.instance.panaltyPoint == 0)
            coroutine = StartCoroutine( StartBossStageCoroutine() );
        StageManager.instance.StartBossStage();
    }

    public IEnumerator StartBossStageCoroutine()
    {
        if (go_AppearAnimation != null)
            go_AppearAnimation.SetActive( true );

        float _time = 0f;

        while(_time < 2f)
        {
            _time += Time.deltaTime;
            Camera.main.transform.localPosition = Vector3.Lerp( new Vector3(0, 4.4f, -10), new Vector3(0, go_BossPrefab.transform.position.y + 3, -10), _time / 2 );
            yield return null;
        }

        _time = 0f;

        UIManager.instance.bossStageUI.grrrrImage.gameObject.SetActive( true );
        //여기에 grrrr 텍스트 이미지
        while (_time < 2f)
        {
            _time += Time.deltaTime;

            UIManager.instance.bossStageUI.grrrrImage.transform.localPosition = UIManager.instance.bossStageUI.grrrrImageOrizinPos + (Random.insideUnitSphere * 15);
            UIManager.instance.bossStageUI.grrrrImage.transform.localPosition = new Vector3( UIManager.instance.bossStageUI.grrrrImage.transform.localPosition.x,
                UIManager.instance.bossStageUI.grrrrImage.transform.localPosition.y, 0 );

            yield return null;
        }

        UIManager.instance.bossStageUI.grrrrImage.gameObject.SetActive( false );

        _time = 0f;

        while (_time < 2f)
        {
            _time += Time.deltaTime;
            Camera.main.transform.localPosition = Vector3.Lerp( new Vector3( 0, go_BossPrefab.transform.position.y + 3, -10 ), new Vector3( 0, 4.4f, -10 ), _time/2 );
            yield return null;
        }

        if (go_AppearAnimation != null)
            go_AppearAnimation.SetActive( false );

        go_BossPrefab.SetActive( true );

        StageManager.instance.isBossStageStart = false;

        coroutine2 = StartCoroutine( StageManager.instance.StageReadyCoroutine() );
        coroutineNum = 0;
    }
}

