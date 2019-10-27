using System.Collections;
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

    void Awake()
    {
        obstacles = transform.GetComponentsInChildren<Obstacle>();
        items = transform.GetComponentsInChildren<Item>();
    }

    void Start()
    {
        StageManager.instance.StartBossStage();
    }

    public IEnumerator StartBossStageCoroutine()
    {
        go_AppearAnimation.SetActive( true );

        float _time = 0f;

        while(_time < 2f)
        {
            _time += Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp( Camera.main.transform.position, new Vector3(0, go_AppearAnimation.transform.position.y + 5f, -10), _time / 2f );
            yield return null;
        }

        yield return new WaitForSeconds( 2f );

        _time = 0f;

        while (_time < 2f)
        {
            _time += Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp( Camera.main.transform.position, new Vector3( 0, 4.4f, -10 ), _time / 2f );
            yield return null;
        }

        go_AppearAnimation.SetActive( false );
        go_BossPrefab.SetActive( true );
        StageManager.instance.isBossStageStart = false;
    }
}

