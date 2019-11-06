using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
    Transform playerTr;

    [SerializeField]
    float boundX;

    [SerializeField]
    float boundY;

    [SerializeField]
    Vector2 dist;

    public bool isCollision;

    // Start is called before the first frame update
    void Start()
    {
        playerTr = FindObjectOfType<PlayerCtrl>().transform;

        boundX = GetComponent<Collider2D>().bounds.size.x /2 + playerTr.GetComponent<CircleCollider2D>().radius * playerTr.localScale.x;
        boundY = GetComponent<Collider2D>().bounds.size.y /2;
    }

    // Update is called once per frame
    void Update()
    {

        dist = ( playerTr.position - transform.position );

        bool isRight = playerTr.position.x > transform.position.x; //벽을 기준으로 플레이어의 위치
        
        if(Mathf.Abs(dist.x) < boundX) //좌우 충돌범위 안으로 들어왔을때
        {
           

            if (Mathf.Abs( dist.y ) < boundY) //상하 충돌범위 안으로 들어왔을때
            {
                //Debug.Break();

                isCollision = true;
                if(isRight)
                {
                    Vector2 setPos = new Vector2( transform.position.x + boundX, playerTr.position.y );
                    playerTr.GetComponent<PlayerCtrl>().CollisionWithWall( setPos);
                }
                else
                {
                    Vector2 setPos = new Vector2( transform.position.x - boundX, playerTr.position.y );
                    playerTr.GetComponent<PlayerCtrl>().CollisionWithWall( setPos );
                }
            }
            else
            {
                isCollision = false;
            }

        }


    }
}
