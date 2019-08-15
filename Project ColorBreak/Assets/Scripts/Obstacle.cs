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

    public float maxPositionX;
    public float minPositionX;

    public float speed;
    private int direction;

    //몇초 마다 장애물 색깔이 바뀔지 결정, 랜덤일수도 
    public float colorChangeTime;
    private float lastChangeTime;

    private int colorChangeNum;

    public Material[] colorMaterials;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        lastChangeTime = 0f;

        onDie += () => Destroy( gameObject );
    }

    private void Start()
    {
        SetMaterial();

        if (obstaclesType == ObstaclesType.Move)
        {
            if (Random.Range( 0, 1 ) == 0)
                direction = -1;
            else
                direction = 1;
            StartCoroutine( MoveCoroutine() );
        }
        else if (obstaclesType == ObstaclesType.ColorChange)
            StartCoroutine( ColorChangeCoroutine() );
    }

    private void SetMaterial()
    {
        if (colorType == ColorType.Red)
            meshRenderer.material = colorMaterials[0];
        else if (colorType == ColorType.Yellow)
            meshRenderer.material = colorMaterials[1];
        else if (colorType == ColorType.Green)
            meshRenderer.material = colorMaterials[2];
        else
            meshRenderer.material = colorMaterials[3];
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

                if (colorChangeNum == 0)
                    colorType = ColorType.Red;
                else if (colorChangeNum == 1)
                    colorType = ColorType.Yellow;
                else if (colorChangeNum == 2)
                    colorType = ColorType.Green;
                else
                    colorType = ColorType.Blue;

                SetMaterial();
            }
            yield return null;
        }
    }
}
