using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StopCamera : MonoBehaviour
{

    private float   borderDist = 0f;
    private Transform    cameraTr;

    private void Start()
    {
        cameraTr = Camera.main.GetComponent<Transform>();
        borderDist = Camera.main.ScreenToWorldPoint( new Vector2( Screen.width, Screen.height ) ).y / 2 + GetComponent<BoxCollider2D>().size.y/2;
    }

    private void OnEnable()
    {
        Camera.main.GetComponent<FollowCamera>().SetCamera();
    }

    private void Update()
    {
        float cameraWithDist = Mathf.Abs( transform.position.y - cameraTr.position.y);

        if(cameraWithDist <= borderDist)
             Camera.main.GetComponent<FollowCamera>().StopCamera();

    }

}
