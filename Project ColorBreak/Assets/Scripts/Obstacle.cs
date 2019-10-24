﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : LivingEntity
{
    //장애물 종류
    public enum ObstaclesType
    {
        Standard, //기본 
        SafeBlock, //색 상관없이 안전한 블럭
        DeathBlock, //무조건 죽음
        CrackBlock,
        BounceBlock,
        AutoColorChange, //자동으로 색이 변하는 블록
        DamagedColorChange //데미지를 입을때마다 색이 변하는 블록
    }

    [Header("CreakBlock - 충돌 시 튕기는 블럭, 최대 체력을 1~3으로 설정")]
    [Header( "BounceBlock - 충돌 시 1회 튕긴 다음 충돌 시 파괴되는 블럭, 최대 체력을 2로 설정" )]
    public ObstaclesType obstaclesType;

    public enum MoveType
    {
        MoveX, //X좌표로 이동
        MoveY, //Y좌표로 이동
        Fix //고정
    }
    [Header("MoveX - X좌표로 이동, MoveY - Y좌표로 이동, Fix - 고정")]
    public MoveType moveType;

    /*
    [Header("체크하시면 파괴되면서 튀기고 체크안하면 그냥 파괴만됩니다.")]
    public bool isBounceBlock = false;*/
    
    [Header("체크하시면 플레이어가 떨어지고 있을 때만 장애물과 충돌합니다.")]
    [Header("일단 모든 장애물에서 체크해주세요." )]
    public bool isCollsionUp = true;

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

    public GameObject[]         particles;
    //public Material[]           colorMaterials;
    public Sprite[]             colorSprites;
    public Sprite[]             crackSprites;
    public Sprite[]             crack1Sprites;
    public Sprite[]             crack2Sprites;
    public Sprite[]             bounceSprites;
    public Sprite[]             bounce1Sprites;
    public Sprite               deathSprite;
    public Sprite               safeSprite;
    private SpriteRenderer      spriteRenderer;

    private Text         lifeText;


    void Awake()
    {
        status = Status.Live;

        spriteRenderer = GetComponent<SpriteRenderer>();
        lifeText = GetComponentInChildren<Text>();
        lastChangeTime = 0f;

        onDie += AddScore;
        //onDie += CreateParticle;
        onDie += CreateEffect;
        onDie += () => gameObject.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //SetLifeUI();

        if(obstaclesType == ObstaclesType.AutoColorChange)
            StartCoroutine( ColorChangeCoroutine() );
    }

    void Start()
    {
        if (obstaclesType == ObstaclesType.SafeBlock)
        {
            obstacleScore = 0;
        }

        //SetMaterial();
        SetSprite();

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

        //SetLifeUI();

        /*
        if (obstaclesType == ObstaclesType.DamagedColorChange)
        {
            int colorNum = 0;
            bool isRandomColor = true;
            ChangeColor( colorNum, isRandomColor );
        }*/
        if (colorType == ColorType.White)
            return;

        switch(obstaclesType)
        {
            case ObstaclesType.CrackBlock:
                if (curLife == 2)
                    spriteRenderer.sprite = crack1Sprites[(int)colorType];
                else if (curLife == 1)
                    spriteRenderer.sprite = crack2Sprites[(int)colorType];
                break;
            case ObstaclesType.BounceBlock:
                spriteRenderer.sprite = bounce1Sprites[(int)colorType];
                break;
        }
    }

    /*
    void SetLifeUI()
    {
        if (curLife > 1)
            lifeText.text = curLife.ToString();
        else
            lifeText.text = " ";
    }*/

    void AddScore()
    {
        if (maxLife == 2 && moveType != MoveType.Fix)
            obstacleScore = 3;
        else if (maxLife == 2 || moveType != MoveType.Fix)
            obstacleScore = 2;
        else
            obstacleScore = 1;

        if (obstaclesType == ObstaclesType.SafeBlock)
            obstacleScore = 0;

        StageManager.instance.AddScoreAndDamage( obstacleScore );
    }

    /*
    private void SetMaterial()
    {
        if ((int)colorType >= colorMaterials.Length)
            return;

        //colorType에 맞는 material을 설정
        spriteRenderer.material = colorMaterials[(int)colorType];
    }*/

    private void SetSprite()
    {
        switch (obstaclesType)
        {
            case ObstaclesType.Standard:
                if ((int)colorType >= colorSprites.Length)
                    return;
                spriteRenderer.sprite = colorSprites[(int)colorType];
                break;
            case ObstaclesType.SafeBlock:
                spriteRenderer.sprite = safeSprite;
                break;
            case ObstaclesType.DeathBlock:
                spriteRenderer.sprite = deathSprite;
                break;
            case ObstaclesType.CrackBlock:
                if (colorType != ColorType.White)
                {
                    if (maxLife == 1)
                        spriteRenderer.sprite = crack2Sprites[(int)colorType];
                    else if (maxLife == 2)
                        spriteRenderer.sprite = crack1Sprites[(int)colorType];
                    else if (maxLife == 3)
                        spriteRenderer.sprite = crackSprites[(int)colorType];
                }
                break;
            case ObstaclesType.BounceBlock:
                if (colorType != ColorType.White && maxLife == 2)
                    spriteRenderer.sprite = bounceSprites[(int)colorType];
                break;
            default:
                break;
        }
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
                transform.position = new Vector3( transform.position.x, maxPositionY, transform.position.z );
                direction = -1;
            }

            transform.Translate( Vector3.up * speed * Time.deltaTime * direction );
            yield return null;
        }
    }

    /*
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
    }*/

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

                //SetMaterial();
            }
            yield return null;
        }
    }

    void CreateParticle()
    {
        float destroyTime = 1.5f;

        int index = (int)colorType;

        if (index >= particles.Length)
            index = particles.Length-1;

        GameObject obj = GameObject.Instantiate( particles[index]);
        obj.transform.position = new Vector3( transform.position.x, transform.position.y, obj.transform.position.z );
        Destroy( obj, destroyTime );
    }

    public void CreateEffect()
    {
        float destroyTime = 0.4f;

        int index = (int)colorType;

        if (index > 3)
            return;

        GameObject clone = Instantiate( StageManager.instance.go_AddScoreEffects[index]);
        clone.transform.position = new Vector3( StageManager.instance.go_Player.transform.localPosition.x, StageManager.instance.go_Player.transform.localPosition.y, transform.position.z );
        Destroy( clone, destroyTime );
    }
}
