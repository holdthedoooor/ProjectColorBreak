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
    private Transform   playerTr;
    private SphereCollider   playerCol;
    private MeshRenderer     playerMr;
    private PlayerState     playerState = PlayerState.Start;

    public FollowCamera followCamera;

    private Vector3  slideVec = Vector3.zero;
    public Vector3   moveVec = Vector3.zero;
    private float   bouncePower = 0f;
    private float   maxSpeed;
    private float    borderDist;
    private bool     isGameOver = false;
  
    public Material[]    colorMt;

    public float     bounceMaxPower = 5.0f;
    public float    speed = 5.0f; //공의 하강속도
    public float    touchAmount = 0.3f; //터치 감도

    //--------------------변수선언-----------------(여기까지)

    protected override void OnEnable()
    {
        base.OnEnable();
        followCamera.transform.position = new Vector3(0,4.6f,-10);
    }

    void Awake()
    {
        playerTr = this.transform;
        playerCol = GetComponent<SphereCollider>();
        playerMr = GetComponentInChildren<MeshRenderer>();

        onDie += () => StageManager.instance.currentStage.FinishStage();
    }

    void Start()
    {
        borderDist= Camera.main.ScreenToWorldPoint(new Vector2(Screen.width,Screen.height)).x - playerCol.radius/2;
        //Screen.width - 게임 화면의 크기를 픽셀로 반환함.
        //ScreenToWorldPoint - 게임 화면의 픽셀위치를 월드포인트로 반환함.


        maxSpeed = speed;
        speed = 0f;
        playerMr.material = colorMt[(int)colorType];
    }

    void Update()
    {
        if (StageManager.instance.isGameOver)
            return;

        Moving();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine( BounceUp() );
        }
      
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

#if UNITY_ANDROID //안드로이드일때

        if(Input.GetTouch(0).phase== TouchPhase.Moved)
        {
            slideVec.x = Input.GetAxis( "Horizontal" ) * touchAmount;
        }
        else
        {
            slideVec.x = Vector3.Slerp( slideVec, Vector3.zero, 0.1f ).x;
        }
       
#else //에디터일때
        float stationary = Mathf.Abs( Input.GetAxis( "Horizontal" ) - 0 );

        if (Input.GetMouseButton( 0 ) && stationary > 0.5f)
        {
            slideVec.x = Input.GetAxis( "Horizontal" ) * touchAmount;
        }
        else
        {
            slideVec.x = Vector3.Slerp( slideVec, Vector3.zero, 0.1f ).x;
        }


#endif

        if(bouncePower>0)
        {
            bouncePower -= 0.1f;
        }

        moveVec = Vector3.down + slideVec;
        moveVec.y += bouncePower;

        //이동시키는 부분
        playerTr.Translate( moveVec * speed * Time.deltaTime );


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

    IEnumerator BounceUp()
    {
        bouncePower = bounceMaxPower;

        yield return new WaitForSeconds(0.5f);

    }

    public void ChangeColor(ColorType color)
    {
        colorType = color;
        playerMr.material = colorMt[(int)colorType];
    }

    private void OnTriggerEnter( Collider other )
    {
        if(other.tag == "Obstacle")
        {
            Obstacle obstacle = other.GetComponent<Obstacle>();

            if (obstacle != null)
            {
                if (obstacle.colorType == colorType)
                {
                    obstacle.OnDamage();
                }
                else
                    OnDamage();
            }
        }
        else if(other.tag == "Goal")
        {
            StageManager.instance.currentStage.FinishStage();
        }
    }

}
