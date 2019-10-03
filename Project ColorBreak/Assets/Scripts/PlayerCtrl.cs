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
    private Rigidbody2D playerRb;

    private PlayerState playerState = PlayerState.Start;
    private Vector3 slideVec = Vector3.zero;
    private Vector3 touchDist = Vector3.zero;
    public Vector3 moveVec = Vector3.zero;
    public float bouncePower = 0f;

    private float touchStartTime = 0f;
    private bool isSwiped = false;

    private float borderDist;
    public bool isBounce = false;
    public float speed;

    [Header( "시작 X, Y, Z 좌표를 입력해주세요!" )]
    public Vector3 originPosition;
    private Vector3 startTouchPos = Vector3.zero;
    private Vector3 endTouchPos = Vector3.zero;

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

        Roll();

    }

    private void FixedUpdate()
    {
        if (StageManager.instance.isGameOver)
            return;

        Moving();
    }

    IEnumerator BounceBall()
    {
        isBounce = true;
        bouncePower = bounceMaxPower;

        yield return new WaitForSeconds( 2.5f );

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
            startTouchPos = Input.mousePosition;
            touchStartTime = Time.time;
        }

        if(isSwiped == false)
        {
            isSwiped = Time.time - touchStartTime > 0.1f;

        }
         isSwiped &= Input.touchCount == 1;

        if (Input.GetMouseButton( 0 ) && isSwiped == true)
        {
            endTouchPos = Input.mousePosition;

            touchDist = endTouchPos - startTouchPos;

            slideVec = Vector3.Slerp( slideVec, touchDist/100, 1.0f ) * touchAmount;

            startTouchPos = Vector3.Slerp( startTouchPos, endTouchPos, 0.1f );
        }
        else
        {
            slideVec = Vector3.Slerp( slideVec, Vector3.zero, 0.1f );
            isSwiped = false;
        }


#else //에디터일때

        if (Input.GetMouseButtonDown( 0 ))
        {
            startTouchPos = Input.mousePosition;
            touchStartTime = Time.time;
        }

        if (isSwiped == false)
            isSwiped = Time.time - touchStartTime > 0.1f;

        if (Input.GetMouseButton( 0 ) && isSwiped == true)
        {
            endTouchPos = Input.mousePosition;

            touchDist = endTouchPos - startTouchPos;

            slideVec = Vector3.Slerp( slideVec, touchDist / 100, 1.0f ) * touchAmount;

            startTouchPos = Vector3.Slerp( startTouchPos, endTouchPos, 0.1f );
        }
        else
        {
            slideVec = Vector3.Slerp( slideVec, Vector3.zero, 0.1f );
            isSwiped = false;
        }

#endif
        if (bouncePower > 0)
            bouncePower -= gravity;

        ////이동시키는 부분
        moveVec = Vector3.down;
        moveVec.x += slideVec.x;
        moveVec.y += bouncePower * Time.fixedDeltaTime;

        //playerTr.Translate( moveVec * speed * Time.deltaTime, Space.World );
        playerRb.velocity = moveVec * speed * Time.fixedDeltaTime;

        //화면 끝 에외처리
        if (playerTr.position.x > borderDist)
        {
            playerTr.position = new Vector3( borderDist, playerTr.position.y, playerTr.position.z );
            moveVec.x = 0;
            slideVec = Vector3.zero;
            playerRb.velocity = new Vector3( 0, playerRb.velocity.y, 0 );
            
        }
        else if (playerTr.position.x < -borderDist)
        {
            playerTr.position = new Vector3( -borderDist, playerTr.position.y, playerTr.position.z );
            slideVec = Vector3.zero;
            moveVec.x = 0;
            playerRb.velocity = new Vector3( 0, playerRb.velocity.y, 0 );

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
            playerSr.sprite = sprites[sprites.Length - 1];

    }

    
    private void OnTriggerEnter2D( Collider2D other )
    {
        if (other.tag == "Obstacle")
        {
            Obstacle obstacle = other.GetComponent<Obstacle>();

            if (obstacle != null)
            {
                if (obstacle.colorType != colorType && obstacle.colorType != ColorType.White && obstacle.colorType != ColorType.Gray)
                {
                    OnDamage();
                }
                //TO DO : 색으로 판정내리기 보다는 Obstacle에서 색에 따른 bool 변수 또는, ObstacleType을 지정해서 코드를 간소화할 필요가 있음.

                if (obstacle.isCollsionUp)
                {
                    if (other.transform.position.y < playerTr.position.y)
                    {
                        //파괴 불가능한 장애물
                        if (obstacle.isBreakable == false)
                        {
                            StartCoroutine( BounceBall() ); ;
                            return;
                        }

                        //색이 같으면
                        if (obstacle.colorType == colorType || obstacle.colorType == ColorType.White)
                        {
                            if (obstacle.isBounceBlock)
                            {
                                if (obstacle.status != Status.Die)
                                    StartCoroutine( BounceBall() );

                                obstacle.OnDamage();
                            }
                            else
                            {
                                obstacle.OnDamage();

                                if (obstacle.status != Status.Die)
                                    StartCoroutine( BounceBall() );
                            }
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
            }
            item.OnDamage();
        }
        else if (other.tag == "Goal")
        {
            StageManager.instance.isGoal = true;
            StageManager.instance.FinishStage();
            OnInitialize();
        }
        else if (other.tag == "Boss")
        {
            StageManager.instance.BossCollision();
            OnInitialize();

        }
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

        isSwiped = false;
        isBounce = false;

        trailRenderer.Clear();
        playerState = PlayerState.Start;
    }

}
