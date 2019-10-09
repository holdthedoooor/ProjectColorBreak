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

    public Sprite[] colorSprites;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        onDie += () => gameObject.SetActive( false );
    }

    void Start()
    {
        SetSprite();
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
}
