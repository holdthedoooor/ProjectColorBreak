﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : LivingEntity
{
    //장애물 종류
    public enum ObstaclesType
    {
        Standard,
        SafeBlock,
        ColorChange
    }
    public ObstaclesType obstaclesType;

    //장애물의 최대, 최소 x position
    public float     maxPositionX;
    public float     minPositionX;

    //장애물의 스피드, 방향
    public float    speed;
    private int     direction;

    //몇초 마다 장애물 색깔이 바뀔지 결정, 랜덤일수도 
    public float    colorChangeTime;
    private float   lastChangeTime;

    //랜덤으로 색을 바꿀 떄 사용
    private int     colorChangeNum;

    //이동형 여부
    public bool isMovable = false;

    private int obstacleScore = 1;

    
    public Material[]       colorMaterials;
    private SpriteRenderer     spriteRenderer;

    void Awake()
    {
        status = Status.Live;

        spriteRenderer = GetComponent<SpriteRenderer>();

        lastChangeTime = 0f;

        onDie += AddScore;
        onDie += () => gameObject.SetActive(false);
    }

    void Start()
    {
        if (obstaclesType == ObstaclesType.SafeBlock)
        {
            isBreakable = false;
            obstacleScore = 0;
            //TO DO: 컬러타입 회색으로 
        }

        SetMaterial();

        //장애물 타입이 Move면 MoveCoroutine 실행
        if (isMovable == true)
        {
            if (Random.Range( 0, 1 ) == 0)
                direction = -1;
            else
                direction = 1;
            StartCoroutine( MoveCoroutine() );
        }
        //장애물 타입이 ColorChange면 ColorChangeCoroutine 실행
        else if (obstaclesType == ObstaclesType.ColorChange)
            StartCoroutine( ColorChangeCoroutine() );
    }

    void AddScore()
    {
        if (maxLife == 2 && isMovable == true)
            obstacleScore = 3;
        else if (maxLife == 2 || isMovable == true)
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

    private IEnumerator MoveCoroutine()
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

    private IEnumerator ColorChangeCoroutine()
    {
        while(status == Status.Live)
        {
            if(lastChangeTime + colorChangeTime <= Time.time )
            {
                lastChangeTime = Time.time;

                colorChangeNum = Random.Range( 0, 4 );

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
}
