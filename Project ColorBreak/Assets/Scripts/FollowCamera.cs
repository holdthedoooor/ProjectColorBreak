using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform playerTr;
    public Transform cameraTr { get; private set; }
    private Vector3 moveVec = Vector3.zero;
    private float dumping = 2.6f;
    public bool isNearGoal = false;

    public Vector3 stopPos = Vector3.zero;
    public Vector3 pastPlayer;

    //----------------------변수선언---------------------(여기까지)

    private void Awake()
    {
        cameraTr = this.transform;
    }
    /*
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag( "Player" ).transform;
    }*/

    /// <summary>
    /// 플레이어와의 높이차가 일정이하가 되면 플레이어와 함께 카메라도 내려갑니다.
    /// </summary>
    void LateUpdate()
    {
        if (StageManager.instance.isGameOver)
            return;

        if (StageManager.instance.isBossStageStart)
            return;

        moveVec = cameraTr.position;
        moveVec.y = playerTr.position.y - dumping;

        if(isNearGoal)
        {
            if (moveVec.y <= stopPos.y)
            {
                moveVec.y = stopPos.y;
            }
        }
      
        cameraTr.position = moveVec;   
    }

    public void StopCamera()
    {
        isNearGoal = true;
        stopPos = transform.position;
    }

    public void SetCamera()
    {
        isNearGoal = false;
        cameraTr.position = new Vector3( 0, 4.4f, -10 );
    }

    public IEnumerator PastPlayerCoroutine()
    {
        while (!StageManager.instance.isGameOver)
        {
            pastPlayer = StageManager.instance.go_Player.transform.localPosition;

            yield return new WaitForSeconds( 0.1f );
        }
        yield return null;
    }
}
