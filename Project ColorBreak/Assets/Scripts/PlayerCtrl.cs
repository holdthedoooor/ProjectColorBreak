﻿using System.Collections;
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
    private SphereCollider playerCol;
    private MeshRenderer playerMr;

    private PlayerState playerState = PlayerState.Start;
    private Vector3 slideVec = Vector3.zero;
    public Vector3 moveVec = Vector3.zero;
    private float maxSpeed;
    private float borderDist;
    private bool isGameOver = false;
  
    public Material[] colorMt;

    public float speed = 5.0f; //공의 하강속도
    public float touchAmount = 0.3f; //터치 감도

    //--------------------변수선언-----------------(여기까지)

    private void Awake()
    {
        playerTr = this.transform;
        playerCol = GetComponent<SphereCollider>();
        playerMr = GetComponentInChildren<MeshRenderer>();
        
    }

    void Start()
    {
        borderDist= Camera.main.pixelWidth/60- playerCol.radius;
        maxSpeed = speed;
        speed = 0f;
        playerMr.material = colorMt[(int)colorType];
        
    }

    void Update()
    {
        if (isGameOver)
            return;

        Moving();
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

#if UNITY_EDITOR //에디터일때

        if (Input.GetMouseButton( 0 ))
        {
            slideVec.x = Input.GetAxis( "Horizontal" ) * touchAmount;

        }
        else
        {
            slideVec.x = Vector3.Slerp( slideVec, Vector3.zero, 0.1f ).x;
        }

   
#else //에디터가 아닐때 (터치로 작동)
      
        if(Input.GetTouch(0).phase== TouchPhase.Moved)
        {
            slideVec.x = Input.GetAxis( "Horizontal" ) * touchAmount;
        }
        else
        {
            slideVec.x = Vector3.Slerp( slideVec, Vector3.zero, 0.1f ).x;
        }

#endif
      
        //이동시키는 부분
        moveVec = Vector3.down + slideVec;
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

    public void ChangeColor(ColorType color)
    {
        colorType = color;
        playerMr.material = colorMt[(int)colorType];

    }

}