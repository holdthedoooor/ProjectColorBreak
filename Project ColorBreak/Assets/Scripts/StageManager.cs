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

    public bool         isMasterMode = false;
    [Header("체크 - 저장한 데이터 초기화, 체크해제 - 저장한 데이터 불러옴")]
    public bool         isDataReset = false;
    public bool         isGameOver;
    public bool         isPause;
    public bool         isGoal;
    //보스 스테이지를 시작했을 때 애니메이션이 끝난 뒤에 이동
    public bool         isBossStageStart = false;
    public bool         isReady = false; //스테이지를 시작했을 때 1초간 준비시간이 있음
    public bool         isButtonUp = false;
    public int          score; //일반 스테이지에서의 점수
    public int          damage;//보스 스테이지에서의 데미지
    public int          panaltyPoint;
    public int          currentChapter;
    public int[]        chaptersUnlockStarCount;

    //현재 챕터의 별 총 개수
    public int[] chaptersStarCount;//챕터마다 획득한 별 개수 

    public GameObject        go_Player;
    public GameObject[]      go_AddScoreEffects;
    public GameObject[]      go_PlayerDieEffects;
    public GameObject        go_TeleportEffect;
    //현재 스테이지   
    public Stage             currentStage;
    public StageSlot         currentStageSlot;
    public BossStage         currentBossStage;
    public BossStageSlot     currentBossStageSlot;
    public SaveLoad          theSaveLoad;

    void Awake()
    {
        if (instance != this)
            Destroy( gameObject );

        if (isDataReset)
            theSaveLoad.DataReset();

        for (int i = 0; i < UIManager.instance.chapterSelectUI.allStageSlot.Length; i++)
        {
            UIManager.instance.chapterSelectUI.allStageSlot[i].stageSlots = UIManager.instance.chapterSelectUI.go_SlotParents[i].GetComponentsInChildren<StageSlot>();
        }

        if (isMasterMode)
        {
            for (int i = 0; i < UIManager.instance.chapterSelectUI.allStageSlot.Length; i++)
            {
                for (int j = 0; j < UIManager.instance.chapterSelectUI.allStageSlot[i].stageSlots.Length; j++)
                {
                    UIManager.instance.chapterSelectUI.allStageSlot[i].stageSlots[j].SetOpen();
                }
                if (UIManager.instance.chapterSelectUI.allStageSlot[i].bossStageSlot != null)
                    UIManager.instance.chapterSelectUI.allStageSlot[i].bossStageSlot.SetOpen();
            }

            UIManager.instance.chapterSelectUI.chapterUnlock = 4;
            UIManager.instance.chapterSelectUI.LoadChapterOpen();
        }
        else
            theSaveLoad.LoadData();

        UIManager.instance.chapterSelectUI.LoadStarCountText();
    }

    // 같은 ColorType의 장애물과 충돌하면 1점씩 추가
    public void AddScoreAndDamage( int _num = 1 )
    {
        if (!isGameOver)
        {
            if(currentStageSlot != null)
            {
                score += _num;
                UIManager.instance.stageUI.UpdateScoreText( score );
                UIManager.instance.StarImageChange();
                UIManager.instance.stageUI.updateSliderCoroutine = StartCoroutine( UIManager.instance.stageUI.UpdateScoreSliderCoroutine() );
            }
            else
            {
                damage += _num;
                UIManager.instance.bossStageUI.UpdateDamageText();
            }
        }
    }

   /* // 보스 스테이지에서 같은 ColoType의 장애물과 충돌하면 데미지가 1씩 축적된다.
    public void AddDamage(int _damage = 1)
    {
        if (!isGameOver)
        {
            damage += _damage;
            UIManager.instance.bossStageUI.UpdateDamageText( damage );
            UIManager.instance.StarImageChange();
        }
    }*/

    //플레이어가 보스와 충돌했을 때 실행
    public void BossCollision()
    {
        SoundManager.instance.PlaySFX( "Boss_Damage" );

        currentBossStageSlot.currentHp -= damage;

        StartCoroutine( UIManager.instance.bossStageUI.UpdateBossHpSliderCoroutine() );

        UIManager.instance.bossStageUI.UpdateBossHpText();

        if (currentBossStage.bossStageType == BossStage.BossStageType.Normal)
        {
            if(currentBossStageSlot.currentHp <= currentBossStageSlot.hardHp)
            {
                go_Player.SetActive( false );

                currentBossStage.go_BossPrefab.GetComponent<Animator>().SetBool( "isHit", true );

                GameObject clone = Instantiate( go_TeleportEffect );

                clone.transform.position = go_Player.transform.position;
                Destroy( clone, 0.15f );

                Invoke( "NextPhase", 0.5f );
            }

            else
            {
                panaltyPoint--;

                go_Player.SetActive( false );

                UIManager.instance.bossStageUI.UpdateChallengeCountText();

                if (panaltyPoint == 0)
                {
                    FinishStage();
                    return;
                }

                currentBossStage.go_BossPrefab.GetComponent<Animator>().SetBool( "isHit", true );

                GameObject clone = Instantiate( go_TeleportEffect);

                clone.transform.position = go_Player.transform.position;
                Destroy( clone, 0.15f );

                Invoke( "BossHitNormal", 0.5f );
            }
        }
        else
        { 
            //게임 종료
            if (currentBossStageSlot.currentHp <= 0)
            {
                currentBossStageSlot.currentHp = 0;
                currentBossStage.go_BossPrefab.GetComponent<Animator>().SetBool( "isHit", true );
                FinishStage();
            }
            else
            {
                panaltyPoint--;

                go_Player.SetActive( false );

                UIManager.instance.bossStageUI.UpdateChallengeCountText();

                if (panaltyPoint == 0)
                {
                    FinishStage();
                    return;
                }

                GameObject.FindGameObjectWithTag( "Boss" ).GetComponent<Animator>().SetBool( "isHit", true );

                GameObject clone = Instantiate( go_TeleportEffect );

                clone.transform.position = go_Player.transform.position;
                Destroy( clone, 0.15f );

                Invoke( "BossHitHard", 0.5f );
            } 
        }        
    }

    // 활성화 되면 즉 스테이지 시작 시 실행
    public void StartStage()
    {
        if(isPause)
        {
            Time.timeScale = 1;
            isPause = false;
        }

        isGoal = false;

        score = 0;
        UIManager.instance.SetStartUI();

        isGameOver = false;
        go_Player.SetActive( true );

        //isMovable를 true로 바꿔줌으로 써 카메라가 다시 움직이도록 설정
        Camera.main.GetComponent<FollowCamera>().SetCamera();
        StartCoroutine( Camera.main.GetComponent<FollowCamera>().PastPlayerCoroutine() );

        isReady = false;
        currentStage.coroutine = StartCoroutine( StageReadyCoroutine() );

        Quit.instance.quitStatus = Quit.QuitStatus.InGame;
    }

    public void StartBossStage()
    {
        if (isPause)
        {
            Time.timeScale = 1;
            isPause = false;
        }

        damage = 0;
        UIManager.instance.bossStageUI.UpdateDamageText();

        if (panaltyPoint == 0)
        {
            isBossStageStart = true;
            isReady = false;
            currentBossStageSlot.currentHp = currentBossStageSlot.maxHp;
            panaltyPoint = currentBossStageSlot.panaltyPoints[0];
            UIManager.instance.bossStageUI.UpdateChallengeCountText();
            UIManager.instance.SetStartUI();
        }

        isGameOver = false;
        go_Player.SetActive( true );

        Camera.main.GetComponent<FollowCamera>().SetCamera();
        StartCoroutine( Camera.main.GetComponent<FollowCamera>().PastPlayerCoroutine() );
        Quit.instance.quitStatus = Quit.QuitStatus.InGame;
    }

    // 스테이지를 통과 시 실행
    public void FinishStage()
    {
        isGameOver = true;

        if(currentStageSlot != null)
        {
            UIManager.instance.SetFinishUI();
            if (isGoal)
            {
                //여기에서 오류 발생하는것같다
                //현재 점수가 현재 스테이지에서 달성한 최대 점수보다 크다면 최대 점수 변경
                if (score > currentStageSlot.bestScore && UIManager.instance.starCount > 0)
                {
                    currentStageSlot.starCount = UIManager.instance.starCount;
                    currentStageSlot.StarImageChange();
                    currentStageSlot.bestScore = score;
                    ChapterOpenCheck();
                    theSaveLoad.SaveData();
                }
            }
        }
        //보스 스테이지면
        else
        {
            UIManager.instance.SetFinishUI();

            if (currentBossStageSlot.starCount < UIManager.instance.starCount)
            {
                currentBossStageSlot.starCount = UIManager.instance.starCount;
                ChapterOpenCheck();

                if(currentBossStageSlot.bossStageType != BossStageSlot.BossStageType.BounceAttack)
                {
                    if (currentBossStageSlot.minPanaltyPoint > panaltyPoint)
                        currentBossStageSlot.minPanaltyPoint = panaltyPoint;
                }

                theSaveLoad.SaveData();
            }
            else if(currentBossStageSlot.starCount == UIManager.instance.starCount && currentBossStageSlot.bossStageType != BossStageSlot.BossStageType.BounceAttack)
            {
                if (currentBossStageSlot.minPanaltyPoint > panaltyPoint)
                    currentBossStageSlot.minPanaltyPoint = panaltyPoint;

                theSaveLoad.SaveData();
            }
                
            currentBossStageSlot.StarImageChange();
            panaltyPoint = 0;
        }
    }

    public void NextPhase()
    {
        //현재 페이즈 스테이지 오브젝트 제거
        Destroy(currentBossStage.gameObject );

        //BossStageHard 생성 후 currentBossStage에 BossStageHard를 할당
        currentBossStage = Instantiate( currentBossStageSlot.go_BossStageHard, new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();

        //플레이어 위치 및 색 초기화
        go_Player.SetActive( true );
    }

    public void ChapterOpenCheck()
    {
        if ( currentChapter < 4)
        {
            chaptersStarCount[currentChapter - 1] = 0;
            for (int i = 0; i < UIManager.instance.stageSlots.Length; i++)
            {
                if (UIManager.instance.stageSlots[i].stageStatus != StageSlot.StageStatus.Clear)
                    break;

                chaptersStarCount[currentChapter - 1] += UIManager.instance.stageSlots[i].starCount;
            }

            if (UIManager.instance.bossStageSlot.bossStageStatus == BossStageSlot.BossStageStatus.Clear)
                chaptersStarCount[currentChapter - 1] += UIManager.instance.bossStageSlot.starCount;

            UIManager.instance.chapterSelectUI.SetStarCountText();

            if (UIManager.instance.chapterSelectUI.chapterUnlock >= currentChapter)
            {
                int total = 0;

                for (int i = 0; i < UIManager.instance.chapterSelectUI.chapterUnlock; i++)
                {
                    total += chaptersStarCount[i];
                }

                if (total >= chaptersUnlockStarCount[currentChapter - 1])
                {
                    //다음 챕터 오픈
                    UIManager.instance.chapterSelectUI.NextChapterOpen();
                    //다음 챕터의 1스테이지 오픈
                    UIManager.instance.chapterSelectUI.allStageSlot[UIManager.instance.chapterSelectUI.chapterUnlock].stageSlots[0].StageSlotOpen();
                    UIManager.instance.chapterSelectUI.chapterUnlock++;
                }
            }
        }
    }

    public void BossAnimationSkip()
    {
        if(isBossStageStart)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Camera.main.GetComponent<FollowCamera>().SetCamera();

                if (currentBossStage.go_AppearAnimation != null)
                    currentBossStage.go_AppearAnimation.SetActive( false );

                currentBossStage.go_BossPrefab.SetActive( true );

                UIManager.instance.bossStageUI.grrrrImage.gameObject.SetActive( false );

                currentBossStage.StopCoroutine( currentBossStage.coroutine );

                isBossStageStart = false;

                currentBossStage.coroutine2 = StartCoroutine( StageReadyCoroutine() );
                currentBossStage.coroutineNum = 1;

                isButtonUp = false;
            }
        }
    }

    public void CreateDieEffect()
    {
        StartCoroutine( DieEffectCoroutine() );
    }

    public IEnumerator DieEffectCoroutine()
    {
        go_Player.SetActive( false );

        SoundManager.instance.PlaySFX( "Character_Destroy" );

        float destroyTime = 0.25f;

        int index = (int)go_Player.GetComponent<LivingEntity>().colorType;

        GameObject clone = Instantiate(go_PlayerDieEffects[index] );
        clone.transform.position = go_Player.transform.position;

        yield return new WaitForSeconds( destroyTime );

        Destroy( clone );
        FinishStage();
    }

    public void StageReadySkip()
    {
        if(isReady && !isBossStageStart)
        {
            if (currentBossStage != null)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log( "버튼 UP" );
                    isButtonUp = true;
                }
            }

            if(currentBossStage != null)
            {
                if (Input.GetMouseButtonDown( 0 ) && isButtonUp)
                {
                    Debug.Log( "버튼 DOWN" );
                    UIManager.instance.bossStageUI.stopImage.enabled = false;
                    if (currentBossStage.coroutineNum == 0)
                        currentBossStage.StopCoroutine( currentBossStage.coroutine2 );
                    else
                        StopCoroutine( currentBossStage.coroutine2 );
                    isReady = false;
                    isButtonUp = false;
                    Time.timeScale = 1;
                }
            }
            else
            {
                if(Input.GetMouseButtonDown( 0 ))
                {
                    UIManager.instance.stageUI.stopImage.enabled = false;
                    StopCoroutine( currentStage.coroutine );
                    isReady = false;
                    Time.timeScale = 1;
                }
            }
        }
    }

    public IEnumerator StageReadyCoroutine()
    {
        isReady = true;

        if (currentStage != null)
            UIManager.instance.stageUI.stopImage.enabled = true;
        else
            UIManager.instance.bossStageUI.stopImage.enabled = true;

        yield return new WaitForSeconds( 1f );

        if (currentStage != null)
            UIManager.instance.stageUI.stopImage.enabled = false;
        else
            UIManager.instance.bossStageUI.stopImage.enabled = false;

        isReady = false;
        isButtonUp = false;
    }

    public void BossHitNormal()
    {
        Destroy( currentBossStage.gameObject );

        if (currentBossStageSlot.bossStageType == BossStageSlot.BossStageType.StageRandom)
            currentBossStage = Instantiate( currentBossStageSlot.go_BossStageNormals[Random.Range( 0, currentBossStageSlot.go_BossStageNormals.Length )]
                , new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();
        else if (currentBossStageSlot.bossStageType == BossStageSlot.BossStageType.Standard)
            currentBossStage = Instantiate( currentBossStageSlot.go_BossStageNormals[0], new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();

        currentBossStage.go_BossPrefab.SetActive( true );

        go_Player.SetActive( true );
    }

    public void BossHitHard()
    {
        Destroy( currentBossStage.gameObject );

        currentBossStage = Instantiate( currentBossStageSlot.go_BossStageHard, new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();

        go_Player.SetActive( true );
    }
}
