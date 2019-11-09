using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Start,
    Falling
}

public class PlayerCtrl : LivingEntity
{
    private Transform playerTr;
    private CircleCollider2D playerCol;
    private SpriteRenderer playerSr;
    private Animator playerAnim;
    private TrailRenderer trailRenderer;
    private FollowCamera followCamera;
    private Transform playerImage;
    public Rigidbody2D playerRb;

    private PlayerState playerState = PlayerState.Start;
    private Vector3 slideVec = Vector3.zero;
    private Vector3 touchDist = Vector3.zero;
    public Vector3 moveVec = Vector3.zero;
    public float bouncePower = 0f;

    private float touchStartTime = 0f;
    private float awakedTime = 0f;
    private float swipeTime = 0f;
    private float borderDist;

    private bool isSwiped = false;
    private bool isAwaked = false;
    public bool isBounce = false;

    public float speed;

    [Header( "시작 X, Y, Z 좌표를 입력해주세요!" )]
    public Vector3 originPosition;

    [SerializeField]
    private Vector3 startTouchPos = Vector3.zero;
    [SerializeField]
    private Vector3 endTouchPos = Vector3.zero;

    [SerializeField]
    private Vector3 wallPoint = Vector3.zero;

    public Vector3 curMousePos;

    public Material[] colorMt;

    public Sprite[] sprites;

    [Header( "공이 튕기는 정도를 수치로 설정해줍니다." )]
    public float bounceMaxPower = 3.0f;//튕기는 정도
    [Header( "공이 낙하는 최대 속도를 수치로 설정해줍니다." )]
    public float maxSpeed = 150.0f;    //공의 하강속도
    [Header( "터치의 감도를 수치로 설정해줍니다. 0.1 단위로 조작합니다." )]
    public float touchAmount = 1f; //터치 감도
    [Header( "중력을 조절합니다. 수치가 높을수록 최대속도에 빨리 도달합니다." )]
    public float gravity = 0.1f;
    [Header( "회전하는 속도를 조절합니다." )]
    public float rotateSpeed = 7f;


    //--------------------변수선언-----------------(여기까지)
    void Awake()
    {
        playerTr = this.transform;
        playerCol = GetComponent<CircleCollider2D>();
        playerSr = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        playerRb = GetComponent<Rigidbody2D>();

        onDie += StageManager.instance.FinishStage;
        onDie += OnInitialize;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        OnInitialize();
    }


    void Start()
    {
        borderDist = Camera.main.ScreenToWorldPoint( new Vector2( Screen.width, Screen.height ) ).x *(Camera.main.rect.width ) - playerCol.radius * transform.localScale.x;
        //Screen.width - 게임 화면의 크기를 픽셀로 반환함.
        //ScreenToWorldPoint - 게임 화면의 픽셀위치를 월드포인트로 반환함.

        trailRenderer.material = colorMt[(int)colorType];

        playerSr.sprite = sprites[(int)colorType];

        if ((int)colorType >= sprites.Length)
            playerSr.sprite = sprites[sprites.Length - 1];
    }

    void Update()
    {
        if (StageManager.instance.isGameOver)
            return;

        StageManager.instance.BossAnimationSkip();

        if (StageManager.instance.isBossStageStart)
            return;

        Roll();
    }

    private void FixedUpdate()
    {
        if (StageManager.instance.isGameOver)
            return;

        if (StageManager.instance.isBossStageStart)
            return;

        if (isAwaked == false)
        {
            awakedTime += Time.fixedDeltaTime;

            if (awakedTime > 0.25f)
            {
                isAwaked = true;
                CancleSwipe();
            }
            else
                return;
        }
        curMousePos = Input.mousePosition;
        Moving();
        
    }

   
    void CancleSwipe()
    {
        isSwiped = false;
        startTouchPos = endTouchPos = Input.mousePosition;
        swipeTime = 0f;
    }

    IEnumerator BounceBall()
    {
        isBounce = true;
        bouncePower = bounceMaxPower;

        yield return new WaitUntil( () => bouncePower <=0);

        isBounce = false;
    }

    private void Moving()
    {
        //처음에 위에서 천천히 떨어지는 구간
        if (playerState == PlayerState.Start)
        {
            speed += gravity;

            if (speed >= maxSpeed)
            {
                speed = maxSpeed;
                playerState = PlayerState.Falling;
            }
        }

#if UNITY_ANDROID //안드로이드일때

        if (Input.GetMouseButtonDown( 0 ))
        {
            CancleSwipe();
            slideVec.x = 0f;
            startTouchPos = Input.mousePosition;
        }

        isSwiped &= Input.touchCount == 1;

        if (Input.GetMouseButton( 0 ))
        {
            if (isSwiped == false) //스와이프로 판정이 안됬을때
            {
                swipeTime += Time.fixedDeltaTime;

                  if (swipeTime > Time.fixedDeltaTime * 2)
                {
                    isSwiped = true;
                    startTouchPos = Input.mousePosition;
                }

            }
            else if (isSwiped == true)//스와이프로 판정이 됐을때
            {

                endTouchPos = Input.mousePosition;

                touchDist = endTouchPos - startTouchPos;//팍튈때 x값이 500~이상 뜸

                //float swipeSpeed = Mathf.Abs( touchDist.x ) / swipeTime * Time.deltaTime; //같은 거리 대비 시간이 짧을 수록 값은 커진다.
                ////시간대비 이동한거리(x좌표기준)

                //if (swipeSpeed > 130f) //완전히 튕기는 경우
                //{
                //    touchDist /= 2f;
                //    //touchDist = Vector3.zero;
                //    //CancleSwipe();
                //}
                //else if (swipeSpeed > 100f) //재빨리 스와이프 한 경우(확 그은 경우)
                //{
                //    touchDist /= 2f;
                //}

                slideVec = Vector3.Slerp( slideVec, touchDist / 100, 1.0f ) * touchAmount;

                startTouchPos = Vector3.Slerp( startTouchPos, endTouchPos, 0.1f );

            }

        }
        else if (Input.GetMouseButtonUp( 0 ))
        {
            CancleSwipe();
        }
        else
        {
            slideVec = Vector3.Slerp( slideVec, Vector3.zero, 0.1f );
        }


#else //에디터일때

        if (Input.GetMouseButtonDown( 0 ))
        {
            CancleSwipe();
            slideVec.x = 0f;
        }

        if (Input.GetMouseButton( 0 ))
        {
            if (isSwiped == false) //스와이프로 판정이 안됬을때
            {
                swipeTime += Time.fixedDeltaTime;

                if (swipeTime > Time.fixedDeltaTime * 2)
                {
                    isSwiped = true;
                    startTouchPos = Input.mousePosition;
                }

            }
            else if (isSwiped == true)//스와이프로 판정이 됐을때
            {

                endTouchPos = Input.mousePosition;

                touchDist = endTouchPos - startTouchPos;//팍튈때 x값이 500~이상 뜸

                //float swipeSpeed = Mathf.Abs( touchDist.x ) / swipeTime * Time.deltaTime; //같은 거리 대비 시간이 짧을 수록 값은 커진다.
                ////시간대비 이동한거리(x좌표기준)

                //if (swipeSpeed > 130f) //완전히 튕기는 경우
                //{
                //    //touchDist = Vector3.zero;
                //    //CancleSwipe();
       
                //}
                //else if (swipeSpeed > 100f) //재빨리 스와이프 한 경우(확 그은 경우)
                //{
                //    touchDist /= 2f;
                //}

                slideVec = Vector3.Slerp( slideVec, touchDist / 100, 1.0f ) * touchAmount;

                startTouchPos = Vector3.Slerp( startTouchPos, endTouchPos, 0.1f );

            }

        }
        else if (Input.GetMouseButtonUp( 0 ))
        {
            CancleSwipe();
        }
        else
        {
            slideVec = Vector3.Slerp( slideVec, Vector3.zero, 0.1f );
        }

#endif
        if (bouncePower > 0)
            bouncePower -= gravity;

        ////이동시키는 부분
        moveVec = Vector3.down;
        moveVec.x += slideVec.x;
        moveVec.y += bouncePower * Time.fixedDeltaTime;

       
        //playerTr.Translate( moveVec * speed * Time.deltaTime, Space.World );

        RaycastHit2D hit = Physics2D.Linecast( transform.position, transform.position + moveVec * speed * Time.deltaTime, LayerMask.GetMask( "Wall" ) );

        if (hit)
        {
            Debug.Log( "벽 발견" );
            wallPoint = hit.point;

            if(transform.position.x > wallPoint.x) //벽이 왼쪽에 위치
            {
                transform.position = new Vector3( wallPoint.x + playerCol.radius * transform.localScale.x, wallPoint.y );
            }
            else
            {
                transform.position = new Vector3( wallPoint.x - playerCol.radius * transform.localScale.x, wallPoint.y );

            }
            CancleSwipe();
            slideVec.x = 0f;
            moveVec.x = 0f;

        }
        transform.position = transform.position + moveVec * speed * Time.deltaTime;


        //화면 끝 에외처리
        if (playerTr.position.x > borderDist)
        {
            playerTr.position = new Vector3( borderDist, playerTr.position.y, playerTr.position.z );
          

        }
        else if (playerTr.position.x < -borderDist)
        {
            playerTr.position = new Vector3( -borderDist, playerTr.position.y, playerTr.position.z );
        }

    }//Moving()

    private void Roll()
    {
        //playerImage.localEulerAngles += new Vector3( 0, 0, speed / maxSpeed * rotateSpeed );
        playerTr.Rotate( 0, 0, rotateSpeed * 100 * Time.deltaTime );
    }

    public void ChangeColor( ColorType color )
    {
        colorType = color;
        //playerSr.material = colorMt[(int)colorType];
        trailRenderer.material = colorMt[(int)colorType];

        playerSr.sprite = sprites[(int)colorType];

        if ((int)color >= sprites.Length)
            playerSr.sprite = sprites[(int)color];
    }

    private void OnTriggerEnter2D( Collider2D other )
    {
        if (other.tag == "Obstacle")
        {
            Obstacle obstacle = other.GetComponent<Obstacle>();

            if (obstacle != null)
            {
                if ((obstacle.colorType != colorType && obstacle.colorType != ColorType.White && obstacle.obstaclesType != Obstacle.ObstaclesType.SafeBlock)
                    || obstacle.obstaclesType == Obstacle.ObstaclesType.DeathBlock)
                {
                    ;
                    OnDamage();
                }
                //TO DO : 색으로 판정내리기 보다는 Obstacle에서 색에 따른 bool 변수 또는, ObstacleType을 지정해서 코드를 간소화할 필요가 있음.

                if (obstacle.isCollsionUp)
                {
                    if (other.transform.position.y < playerTr.position.y)
                    {
                        switch (obstacle.obstaclesType)
                        {
                            case Obstacle.ObstaclesType.Standard:
                                if (obstacle.colorType == colorType || obstacle.colorType == ColorType.White)
                                    obstacle.OnDamage();
                                break;

                            case Obstacle.ObstaclesType.SafeBlock:
                                StartCoroutine( BounceBall() ); ;
                                break;

                            case Obstacle.ObstaclesType.CrackBlock:
                                if (obstacle.colorType == colorType || obstacle.colorType == ColorType.White)
                                {
                                    if (obstacle.status != Status.Die)
                                        StartCoroutine( BounceBall() );

                                    obstacle.OnDamage();
                                }
                                break;

                            case Obstacle.ObstaclesType.BounceBlock:
                                if (obstacle.colorType == colorType || obstacle.colorType == ColorType.White)
                                {
                                    obstacle.OnDamage();

                                    if (obstacle.status != Status.Die)
                                        StartCoroutine( BounceBall() );
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }

            }//obstacle != null
        }
        else if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();

            if (item != null)
            {
                if (item.itemType == Item.ItemType.ColorChange)
                    ChangeColor( item.colorType );
                item.OnDamage();
            }
        }
        else if (other.tag == "Goal")
        {
            StageManager.instance.isGoal = true;
            StageManager.instance.FinishStage();
            OnInitialize();
        }


    }

    public void OnCollisionEnter2D( Collision2D collision )
    {
        if(collision.transform.tag == "Boss")
        {
            if (StageManager.instance.currentBossStageSlot.bossStageType == BossStageSlot.BossStageType.BounceAttack)
            {
                if (colorType == ColorType.Red)
                {
                    StageManager.instance.currentBossStageSlot.currentHp -= 1;

                    UIManager.instance.bossStageUI.UpdateBossHpText();

                    StartCoroutine( UIManager.instance.bossStageUI.UpdateBossHpSliderCoroutine() );

                    if (StageManager.instance.currentBossStageSlot.currentHp <= 0)
                    {
                        StageManager.instance.currentBossStageSlot.currentHp = 0;
                        StageManager.instance.FinishStage();
                        return;
                    }

                    StartCoroutine( BounceBall() );
                }
                else
                    StageManager.instance.FinishStage();

                return;
            }

            StageManager.instance.BossCollision();
        }

    }

    public void CollisionWithWall(Vector2 setPos)
    {
        transform.position = setPos;

        CancleSwipe();
        slideVec.x = 0f;
        touchDist = Vector2.zero;

    }

    public void OnInitialize()
    {
        colorType = ColorType.Red;
        ChangeColor( colorType );
        transform.position = originPosition;

        touchDist = Vector3.zero;
        startTouchPos = Vector3.zero;
        endTouchPos = Vector3.zero;
        slideVec = Vector3.zero;
        bouncePower = 0f;
        moveVec = Vector3.zero;
        playerRb.velocity = Vector3.zero;
        speed = 0f;
        touchStartTime = 0f;
        awakedTime = 0f;
        swipeTime = 0f;

        isSwiped = false;
        isBounce = false;
        isAwaked = false;

        trailRenderer.Clear();
        playerState = PlayerState.Start;
    }
}

