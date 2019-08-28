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
    public FollowCamera followCamera;

    private PlayerState playerState = PlayerState.Start;
    private Vector3 slideVec = Vector3.zero;
    public Vector3 moveVec = Vector3.zero;
    private float maxSpeed;
    private float bouncePower = 0f;
    private float borderDist;
    private bool isGameOver = false;
    private bool isBounce = false;

    private float slidePower = 0f;

    private Vector3 startTouchPos = Vector3.zero;
    private Vector3 endTouchPos = Vector3.zero;

    public Material[] colorMt;

    public float bounceMaxPower = 3.0f;//튕기는 정도
    public float speed = 5.0f; //공의 하강속도
    public float touchAmount = 0.3f; //터치 감도

    //--------------------변수선언-----------------(여기까지)

    protected override void OnEnable()
    {
        base.OnEnable();
        followCamera.transform.position = new Vector3( 0, 4.6f, -10 );
        colorType = ColorType.Red;
        ChangeColor( colorType );
    }

    void Awake()
    {
        playerTr = this.transform;
        playerCol = GetComponent<CircleCollider2D>();
        playerSr = GetComponentInChildren<SpriteRenderer>();
        playerAnim = GetComponent<Animator>();
        trailRenderer = GetComponent<TrailRenderer>();

        onDie += () => StageManager.instance.currentStage.FinishStage();
    }
  

    void Start()
    {
        borderDist = Camera.main.ScreenToWorldPoint( new Vector2( Screen.width, Screen.height ) ).x - playerCol.radius / 2;
        //Screen.width - 게임 화면의 크기를 픽셀로 반환함.
        //ScreenToWorldPoint - 게임 화면의 픽셀위치를 월드포인트로 반환함.

        maxSpeed = speed;
        speed = 0f;
        playerSr.material = colorMt[(int)colorType];
        trailRenderer.material = colorMt[(int)colorType];
    }
    
    void Update()
    {
        if (StageManager.instance.isGameOver)
            return;

        Moving();

        playerAnim.SetBool( "isBounce", isBounce );

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
            speed += 0.1f;

            if (speed >= maxSpeed)
            {
                speed = maxSpeed;
                playerState = PlayerState.Falling;
            }
        }

        float dist = 0f;
        bool isStationary = false;

#if UNITY_ANDROID //안드로이드일때


        if (Input.touchCount > 0 && Input.GetTouch( 0 ).phase == TouchPhase.Began)
        {
            startTouchPos = Input.GetTouch( 0 ).position;
        }
        else if((Input.touchCount > 0 && Input.GetTouch( 0 ).phase == TouchPhase.Stationary)
        {
            slidePower = Input.GetAxis( "Horizontal" ) * touchAmount;
            isStationary= true;
        }
        else if (Input.touchCount > 0 && Input.GetTouch( 0 ).phase == TouchPhase.Ended)
        {
            endTouchPos = Input.GetTouch( 0 ).position;
            Debug.Log( "endPos.x: " + endTouchPos.x );

            dist = Vector3.Distance( startTouchPos, endTouchPos ) / 50;

            Debug.Log( "dist: " + dist );

            if (startTouchPos.x < endTouchPos.x)
                slidePower = dist * touchAmount;
            else
                slidePower = -dist * touchAmount;

            Debug.Log( "slidePower:" + slidePower );

        }
        
        if (bouncePower > 0)
            bouncePower -= 0.1f;

        if(isStationary == false)
        {
          if (slidePower > 0)
            slidePower -= 0.1f;
         else
            slidePower += 0.1f;
        }

    
        //이동시키는 부분
        moveVec = Vector3.down;

        moveVec.x += slidePower;
        moveVec.y += bouncePower;
        playerTr.Translate( moveVec * speed * Time.deltaTime );

#else //에디터일때

        float stationary = Mathf.Abs( Input.GetAxis( "Horizontal" ) );

       if (Input.GetMouseButton( 0 ) && stationary > 1.5f)
       {
           slideVec.x = Input.GetAxis( "Horizontal" ) * touchAmount;
       }
       else
       {
           slideVec.x = Vector3.Slerp( slideVec, Vector3.zero, 0.1f ).x;
       }

        if (bouncePower > 0)
            bouncePower -= 0.1f;

        //이동시키는 부분
        moveVec = Vector3.down + slideVec;
        moveVec.y += bouncePower;
        playerTr.Translate( moveVec * speed * Time.deltaTime );

#endif

        //화면 끝 에외처리
        if (playerTr.position.x > borderDist)
        {
            playerTr.position = new Vector3( borderDist, playerTr.position.y, playerTr.position.z );
            slidePower = 0f;
        }
        else if (playerTr.position.x < -borderDist)
        {
            playerTr.position = new Vector3( -borderDist, playerTr.position.y, playerTr.position.z );
            slidePower = 0f;
        }

    }//Moving()

    public void ChangeColor(ColorType color)
    {
        colorType = color;
        playerSr.material = colorMt[(int)colorType];
        trailRenderer.material = colorMt[(int)colorType];

    }


    private void OnTriggerEnter2D( Collider2D other )
    {
        bool isCollisionUp = false;
        isCollisionUp = other.transform.position.y < playerTr.position.y;

        if(other.tag == "Obstacle")
        {
            Obstacle obstacle = other.GetComponent<Obstacle>();

            if (obstacle != null)
            {
                if(isCollisionUp== true)
                {
                    if (obstacle.isBreakable == false)
                    {
                        StartCoroutine( BounceBall() ); ;
                        return;

                    }

                    if (obstacle.colorType == colorType)
                    {
                        obstacle.OnDamage();

                        if (obstacle.status != Status.Die)
                            StartCoroutine( BounceBall() );
                    }
                    else if (obstacle.colorType != colorType)
                        OnDamage();

                }
                else if (obstacle.isBreakable == false)
                    return;
                else if(obstacle.colorType != colorType)
                    OnDamage();
            }
        }
        else if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();

            if(item != null)
            {
                if(item.itemType == Item.ItemType.ColorChange)
                    ChangeColor( item.colorType );
            }
            item.OnDamage();
        }
        else if(other.tag == "Goal")
        {
            StageManager.instance.isGoal = true;
            StageManager.instance.currentStage.FinishStage();
        }
    }

}
