using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StopCamera : MonoBehaviour
{

    private float   borderDist = 0f;
    private Transform    cameraTr;
    public float stopDumping = 5f;

    private void Start()
    {
        cameraTr = Camera.main.GetComponent<Transform>();
        borderDist = Camera.main.ScreenToWorldPoint( new Vector2( Screen.width, Screen.height ) ).y * (Camera.main.rect.height) / 2 + GetComponent<BoxCollider2D>().size.y/2;
    }

    private void Update()
    {
        float cameraWithDist = Mathf.Abs( (transform.position.y - 0.005f) - cameraTr.position.y);
        Debug.Log( "cameraTr : " + Camera.main.transform.position.y );
        Debug.Log( "transform.position.y - cameraTr.position.y : " + Mathf.Abs( transform.position.y - cameraTr.position.y ) );
        Debug.Log( "borderDist : " + borderDist );
        if(cameraWithDist <= borderDist)
             Camera.main.GetComponent<FollowCamera>().StopCamera();
    }

}
