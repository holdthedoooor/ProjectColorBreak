using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( SpriteRenderer ) )]
public class StopCamera : MonoBehaviour
{
    //카메라에 들어와서 렌더가 시작되면 호출이 되는 함수입니다.
    //이 함수를 쓰려면 렌더러 컴포넌트가 있어야합니다.
    void OnBecameVisible()
    {
        //Camera.main.GetComponent<FollowCamera>().StopCamera();
    }

    
}
