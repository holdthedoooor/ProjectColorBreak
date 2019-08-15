using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform playerTr;
    private Transform cameraTr;
    private Vector3 moveVec = Vector3.zero;
    private float dumping = 2.5f;

    bool isStart = false;

    //----------------------변수선언---------------------(여기까지)

    private void Awake()
    {
        cameraTr = this.transform;
    }
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag( "Player" ).transform;
    }

    /// <summary>
    /// 플레이어와의 높이차가 일정이하가 되면 플레이어와 함께 카메라도 내려갑니다.
    /// </summary>
    void LateUpdate()
    {
        moveVec = cameraTr.position;
        moveVec.y = playerTr.position.y - dumping;

        if(playerTr.position.y< cameraTr.position.y+ dumping)
        {
            cameraTr.position = moveVec;
        }
        

    }
}
