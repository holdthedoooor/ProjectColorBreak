using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : LivingEntity
{
    //장애물 종류
    public enum ObstaclesType
    {
        Standard,
        SafeBlock,
        AutoColorChange, //자동으로 색이 변하는 블록
        DamagedColorChange //데미지를 입을때마다 색이 변하는 블록
    }
    public ObstaclesType obstaclesType;

    public enum MoveType
    {
        MoveX, //X좌표로 이동
        MoveY, //Y좌표로 이동
        Fix //고정
    }
    [Header("MoveX - X좌표로 이동, MoveY - Y좌표로 이동, Fix - 고정")]
    public MoveType moveType;

    [Header("체크하시면 파괴되면서 튀기고 체크안하면 그냥 파괴만됩니다.")]
    public bool isBounceBlock = false;
    
    //장애물의 최대, 최소 x position
    public float     maxPositionX;
    public float     minPositionX;

    //장애물의 최대, 최소 y position
    public float     maxPositionY;
    public float     minPositionY;

    //장애물의 스피드, 방향
    public float    speed;
    [Header( "-1이면 왼쪽 or 아래쪽, 1이면 오른쪽 or 위쪽부터 시작" )]
    public int      direction; //-1이면 왼쪽 or 아래쪽, 1이면 오른쪽 or 위쪽

    //몇초 마다 장애물 색깔이 바뀔지 결정, 랜덤일수도 
    [HideInInspector]
    public float    colorChangeTime;
    private float   lastChangeTime; 

    private int obstacleScore = 1;

    public GameObject       particle;
    public Material[]       colorMaterials;
    private SpriteRenderer     spriteRenderer;

    private Text         lifeText;


    void Awake()
    {
        status = Status.Live;

        spriteRenderer = GetComponent<SpriteRenderer>();
        lifeText = GetComponentInChildren<Text>();
        lastChangeTime = 0f;

        onDie += AddScore;
        onDie += CreateParticle;
        onDie += () => gameObject.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetLifeUI();

        if(obstaclesType == ObstaclesType.AutoColorChange)
            StartCoroutine( ColorChangeCoroutine() );
    }

    void Start()
    {
        if (obstaclesType == ObstaclesType.SafeBlock)
        {
            isBreakable = false;
            obstacleScore = 0;
            colorType = ColorType.Gray;
        }

        SetMaterial();

        //장애물 타입이 Move면 MoveCoroutine 실행
        if (moveType == MoveType.MoveX)
        {
            StartCoroutine( MoveXCoroutine() );
        }
        else if(moveType == MoveType.MoveY)
        {
            StartCoroutine( MoveYCoroutine() );
        }

    }

    public override void OnDamage()
    {
        base.OnDamage();

        SetLifeUI();

        if (obstaclesType == ObstaclesType.DamagedColorChange)
        {
            int colorNum = 0;
            bool isRandomColor = true;
            ChangeColor( colorNum, isRandomColor );
        }

    }

    void SetLifeUI()
    {
        if (curLife > 1)
            lifeText.text = curLife.ToString();
        else
            lifeText.text = " ";
    }

    void AddScore()
    {
        if (maxLife == 2 && moveType != MoveType.Fix)
            obstacleScore = 3;
        else if (maxLife == 2 || moveType != MoveType.Fix)
            obstacleScore = 2;
        else
            obstacleScore = 1;

        if (isBreakable == false || obstaclesType== ObstaclesType.SafeBlock)
            obstacleScore = 0;

        StageManager.instance.AddScore( obstacleScore );
    }

    private void SetMaterial()
    {
        if ((int)colorType >= colorMaterials.Length)
            return;

        //colorType에 맞는 material을 설정
        spriteRenderer.material = colorMaterials[(int)colorType];
    }

    private IEnumerator MoveXCoroutine()
    {
        while(status == Status.Live)
        {
            if(transform.position.x <= minPositionX)
            {
                transform.position = new Vector3( minPositionX, transform.position.y, transform.position.z );
                direction = 1;
            }
            else if(transform.position.x >= maxPositionX)
            {
                transform.position = new Vector3( maxPositionX, transform.position.y, transform.position.z );
                direction = -1;
            }

            transform.Translate( Vector3.right * speed * Time.deltaTime * direction );
            yield return null;
        }
    }

    private IEnumerator MoveYCoroutine()
    {
        while (status == Status.Live)
        {
            if (transform.position.y <= minPositionY)
            {
                transform.position = new Vector3( transform.position.x, minPositionY, transform.position.z );
                direction = 1;
            }
            else if (transform.position.y >= maxPositionY)
            {
                transform.position = new Vector3( transform.position.x, minPositionY, transform.position.z );
                direction = -1;
            }

            transform.Translate( Vector3.up * speed * Time.deltaTime * direction );
            yield return null;
        }
    }

    private void ChangeColor( int colorNum = 0, bool isRandom = true)
    {
        int colorMaxNum = colorMaterials.Length -1;

        if(isRandom== true)
        {
            int randomColor = Random.Range( 0, colorMaxNum );

            while (randomColor != (int)colorType)
            {
                randomColor = Random.Range( 0, colorMaxNum );
            }

            colorType = (ColorType)randomColor;
        }
        else
        {
            colorType = (ColorType)colorNum;
        }
              
        SetMaterial();

    }

    private IEnumerator ColorChangeCoroutine()
    {
        if (obstaclesType != ObstaclesType.AutoColorChange)
            yield return null;

        while(status == Status.Live)
        {
            if(lastChangeTime + colorChangeTime <= Time.time )
            {
                lastChangeTime = Time.time;

                int colorChangeNum = Random.Range( 0, 4 );

                switch(colorChangeNum)
                {
                    case 0:
                        colorType = ColorType.Red;
                        break;
                    case 1:
                        colorType = ColorType.Yellow;
                        break;
                    case 2:
                        colorType = ColorType.Blue;
                        break;
                    default:
                        colorType = ColorType.Purple;
                        break;
                }

                SetMaterial();
            }
            yield return null;
        }
    }

    void CreateParticle()
    {
        float destroyTime = 1.5f;
        GameObject obj = GameObject.Instantiate( particle);
        obj.transform.position = new Vector3( transform.position.x, transform.position.y, obj.transform.position.z );
        Destroy( obj, destroyTime );
    }
}
