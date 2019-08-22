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

    public Material[] colorMaterials;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        onDie += () => gameObject.SetActive( false );
    }

    void Start()
    {
        SetMaterial();
    }

    private void SetMaterial()
    {
        if ((int)colorType >= colorMaterials.Length)
            return;

        //colorType에 맞는 material을 설정
        spriteRenderer.material = colorMaterials[(int)colorType];
    }

    override public void OnDamage()
    {
        Die();
    }
}
