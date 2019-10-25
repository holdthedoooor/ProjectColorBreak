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
        Move,
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

    public Vector3 movePosition1;
    public Vector3 movePosition2;
    private Vector3 destination;

    //아이템의 스피드, 방향
    public float speed;
    [Header( "-1이면 movePosition1 부터 1이면 movePosition2 부터 시작" )]
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

        if (moveType == MoveType.Move)
            StartCoroutine( MoveCoroutine() );
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

    private IEnumerator MoveCoroutine()
    {
        while (status == Status.Live)
        {
            if (Vector3.Distance( movePosition1, transform.position ) <= 0.1f)
                destination = movePosition2;

            else if (Vector3.Distance( movePosition2, transform.position ) <= 0.1f)
                destination = movePosition1;

            transform.position = Vector3.MoveTowards( transform.position, destination, speed * Time.deltaTime );
            yield return null;
        }
    }
}