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
    [Header( "Move - 설정한 좌표로 이동, Fix - 고정" )]
    public MoveType moveType;

    public Vector3 movePosition1;
    public Vector3 movePosition2;
    private Vector3 destination;
    private CircleCollider2D circleCollider;

    //아이템의 스피드, 방향
    public float speed;
    [Header( "1이면 movePosition1 부터 2면 movePosition2 부터 시작" )]
    public int direction; //-1이면 왼쪽 or 아래쪽, 1이면 오른쪽 or 위쪽

    public Sprite[] colorSprites;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        status = Status.Live;
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

        onDie += () => circleCollider.enabled = false;
        onDie += () => spriteRenderer.enabled = false;
    }

    void Start()
    {
        SetSprite();

        if (direction == 1)
            destination = movePosition1;
        else if (direction == 2)
            destination = movePosition2;

        if (moveType == MoveType.Move)
            StartCoroutine( MoveCoroutine() );
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        
    }

    public void SetSprite()
    {
        if ((int)colorType >= colorSprites.Length)
        {
            return;
        }

        Debug.Log( (int)colorType + ", " + colorSprites.Length );

        //colorType에 맞는 material을 설정
        spriteRenderer.sprite = colorSprites[(int)colorType];
    }

    override public void OnDamage(int _damage = 1)
    {
        Die();
    }

    private IEnumerator MoveCoroutine()
    {
        while (!StageManager.instance.isGameOver)
        {
            if(!StageManager.instance.isReady && !StageManager.instance.isBossStageStart)
            {
                if (Vector3.Distance( movePosition1, transform.position ) <= 0.1f)
                    destination = movePosition2;

                else if (Vector3.Distance( movePosition2, transform.position ) <= 0.1f)
                {
                    if (StageManager.instance.currentBossStageSlot != null)
                    {
                        if (StageManager.instance.currentBossStageSlot.bossStageType == BossStageSlot.BossStageType.BounceAttack)
                        {
                            int _randNum = Random.Range( 0, 4 );

                            switch (_randNum)
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
                                case 3:
                                    colorType = ColorType.Purple;
                                    break;
                            }

                            SetSprite();

                            status = Status.Live;

                            circleCollider.enabled = true;
                            spriteRenderer.enabled = true;
                        }
                    }
                    destination = movePosition1;
                }
                    

                transform.position = Vector3.MoveTowards( transform.position, destination, speed * Time.deltaTime );
            }
            yield return null;
        }
    }
}