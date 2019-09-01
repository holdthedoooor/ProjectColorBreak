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
        Purple,
        Gray
    }
    public ColorType colorType;

    [Header("최대 체력을 조절할 수 있습니다. (정수만 가능)")]
    //최대 체력
    public int               maxLife;
    //현재 체력
    public int               curLife { get; protected set; }
    [Header( "체크 해제하면 세이프 블럭이 됩니다!" )]
    //파괴가능 여부
    public bool isBreakable = true;
    //죽을 때 실행되는 이벤트
    protected event Action   onDie;

    virtual protected void OnEnable()
    {
        curLife = maxLife;
        status = Status.Live;
    }

    //데미지를 받았을 때 실행되는 함수
    virtual public void OnDamage()
    {
        if (isBreakable == false)//부실수 없는 장애물의 경우
            return;

        curLife -= 1;

        if (curLife <= 0)
            Die();
    }

    //죽었을 때 실행되는 함수
    public void Die()
    {
        status = Status.Die;
        if (onDie != null)
            onDie();
    }
}
