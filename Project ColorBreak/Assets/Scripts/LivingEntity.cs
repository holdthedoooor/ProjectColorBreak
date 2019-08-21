using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//체력이 존재하는 오브젝트들의 부모 클래스
//장애물, 플레이어 오브젝트 등
//체력이 있고 죽을 수 있는(파괴 될 수 있는) 오브젝트들에게 할당
public class LivingEntity : MonoBehaviour
{
    public enum Status
    {
        Live,
        Die
    }
    public Status status { get; protected set; }

    //색깔 종류
    public enum ColorType
    {
        Red,
        Yellow,
        Blue,
        Purple
    }
    public ColorType colorType;

    //최대 체력
    public int               maxHp;
    //현재 체력
    public int               currentHP { get; protected set; }
    public Vector3           originPosition;

    //죽을 때 실행되는 이벤트
    protected event Action   onDie;

    virtual protected void OnEnable()
    {
        currentHP = maxHp;
        status = Status.Live;
        transform.position = originPosition;
    }

    //데미지를 받았을 때 실행되는 함수
    public void OnDamage()
    {
        currentHP -= 1;

        if (currentHP <= 0)
            Die();
    }

    //죽었을 때 실행되는 함수
    public void Die()
    {
        if (onDie != null)
            onDie();
    }
}
