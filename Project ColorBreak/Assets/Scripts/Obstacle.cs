using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : LivingEntity
{
    //장애물 종류
    public enum ObstaclesType
    {
        Standard,
        Move,
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

    public Material[]       colorMaterials;
    private MeshRenderer     meshRenderer;

    private void Awake()
    {
        status = Status.Live;

        meshRenderer = GetComponent<MeshRenderer>();

        lastChangeTime = 0f;

        onDie += () => StageManager.instance.stage.AddScore();
        onDie += () => Destroy( gameObject );
    }

    private void Start()
    {
        SetMaterial();

        //장애물 타입이 Move면 MoveCoroutine 실행
        if (obstaclesType == ObstaclesType.Move)
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

    private void SetMaterial()
    {
        if ((int)colorType >= colorMaterials.Length)
            return;

        //colorType에 맞는 material을 설정
        meshRenderer.material = colorMaterials[(int)colorType];
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
                        colorType = ColorType.Green;
                        break;
                    default:
                        colorType = ColorType.Blue;
                        break;
                }

                SetMaterial();
            }
            yield return null;
        }
    }
}
