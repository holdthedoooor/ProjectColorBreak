using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : LivingEntity
{
    public enum ItemType
    {
        ColorChange
    }
    public ItemType itemType;

    public enum MoveType
    {
        MoveX, //X좌표로 이동
        MoveY, //Y좌표로 이동
        Fix //고정
    }
    [Header( "MoveX - X좌표로 이동, MoveY - Y좌표로 이동, Fix - 고정" )]
    public MoveType moveType;

    //아이템의 최대, 최소 x position
    public float maxPositionX;
    public float minPositionX;

    //아이템의 최대, 최소 y position
    public float maxPositionY;
    public float minPositionY;

    //아이템의 스피드, 방향
    public float speed;
    [Header( "-1이면 왼쪽 or 아래쪽, 1이면 오른쪽 or 위쪽부터 시작" )]
    public int direction; //-1이면 왼쪽 or 아래쪽, 1이면 오른쪽 or 위쪽

    public Sprite[] colorSprites;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        status = Status.Live;
        spriteRenderer = GetComponent<SpriteRenderer>();

        onDie += () => gameObject.SetActive( false );
    }

    void Start()
    {
        SetSprite();

        if (moveType == MoveType.MoveX)
        {
            StartCoroutine( MoveXCoroutine() );
        }
        else if (moveType == MoveType.MoveY)
        {
            StartCoroutine( MoveYCoroutine() );
        }
    }

    private void SetSprite()
    {
        if ((int)colorType >= colorSprites.Length)
            return;

        //colorType에 맞는 material을 설정
        spriteRenderer.sprite = colorSprites[(int)colorType];
    }

    override public void OnDamage()
    {
        Die();
    }

    private IEnumerator MoveXCoroutine()
    {
        while (status == Status.Live)
        {
            if (transform.position.x <= minPositionX)
            {
                transform.position = new Vector3( minPositionX, transform.position.y, transform.position.z );
                direction = 1;
            }
            else if (transform.position.x >= maxPositionX)
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
}
