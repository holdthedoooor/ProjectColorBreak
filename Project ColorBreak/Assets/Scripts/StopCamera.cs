using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCamera : MonoBehaviour
{
    private float   borderDist = 0f;
    private Transform    cameraTr;
    public float stopDumping = 0f;

    private void Start()
    {
        cameraTr = Camera.main.GetComponent<Transform>();
        borderDist = Camera.main.ScreenToWorldPoint( new Vector2( Screen.width, Screen.height ) ).y * (Camera.main.rect.height) / 2;
    }

    private void Update()
    {
        float cameraWithDist = Mathf.Abs( transform.position.y - cameraTr.position.y);

        if(Camera.main.GetComponent<FollowCamera>().pastPlayer.y > StageManager.instance.go_Player.transform.localPosition.y)
        {
            if (cameraWithDist <= borderDist + stopDumping)
                Camera.main.GetComponent<FollowCamera>().StopCamera();
        }
    }
}
