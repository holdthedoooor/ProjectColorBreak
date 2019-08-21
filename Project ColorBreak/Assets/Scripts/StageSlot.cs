using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSlot : MonoBehaviour
{
    public enum StageStatus
    {
        Rock, //잠겨있다.
        Open, //이전 스테이지를 통과한 경우 열린다.
        Clear //별을 1개라도 획득하면 클리어
    }
    public StageStatus stageStatus;

    public Image[]      starImages;
    public Sprite       rockSprite;
    public Sprite       openSprite;
    public GameObject   stagePrefab;

    //Stage Prefab이 생성, 파괴 될 수도 있기 때문에 StageSlot에 선언해준다.
    public int          bestScore = 0;
    public int          starCount = 0; //별이 몇개 채워졌는지 저장

    //해당 스테이지가 끝나고 점수가 bestScore보다 높을 때 실행
    //별 이미지 최대 점수 변경
    public void StageSlotChange()
    {
        if(StageManager.instance.currentStage.score > bestScore)
        {
            for (int i = 0; i < starCount; i++)
            {
                starImages[i].sprite = UIManager.instance.starSprite;
            }
        }
    }

}
