using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSlot : MonoBehaviour
{
    public enum StageStatus
    {
        Rock, //잠겨있다.
        Open, //이전 스테이지를 통과한 경우 열린다. 별이 0개인 경우
        Clear //별을 1개라도 획득하면 클리어
    }
    public StageStatus stageStatus;

    public   Text         stageNumberText;
    public   Image        stageSlotImage;  
    public   Image[]      starImages;
    public   Sprite       rockSprite;
    public   Sprite       openSprite;
    public   Button       stageSelectButton;
    public   GameObject   go_StagePrefab;
    public   GameObject   go_StageInformation;

    //Stage Prefab이 생성, 파괴 될 수도 있기 때문에 StageSlot에 선언해준다.
    public int[]        checkPoints;
    public int          bestScore = 0;
    public int          starCount = 0; //별이 몇개 채워졌는지 저장
    public int          stageNumber;

    void Awake()
    {
        stageNumberText.text = (stageNumber + 1).ToString();
    }

    //해당 스테이지가 끝나고 점수가 bestScore보다 높을 때 실행
    //별 이미지 최대 점수 변경
    public void StarImageChange()
    {
        for (int i = 0; i < starCount; i++)
        {
            starImages[i].sprite = UIManager.instance.starSprite;
        }
    }

    //이전 스테이지를 클리어해서 다음 스테이지 슬롯이 열림
    public void StageSlotOpen()
    {
        if(stageStatus == StageStatus.Rock)
        {
            stageSelectButton.interactable = true; //버튼 클릭 가능
            stageSlotImage.sprite = openSprite;    //오픈 Sprite로 변경
            go_StageInformation.SetActive( true ); //스테이지 Text, Star Image 활성화
            stageStatus = StageStatus.Open;
        }   
    }

    //스테이지 선택 버튼
    public void StageSelectButton()
    {
        StageManager.instance.currentStageSlot = transform.GetComponent<StageSlot>();
        StageManager.instance.currentStage = StageManager.instance.currentStageSlot.go_StagePrefab.GetComponent<Stage>();

        UIManager.instance.stageInformationUI.ResetStageInformation();
        UIManager.instance.stageInformationUI.SetStageInformation();
    }

    public void SetStageSlot(int _bestScore, int _starCount, int _statusNumber)
    {
        bestScore = _bestScore;
        starCount = _starCount;
        StarImageChange();
        if (_statusNumber == 1)
            stageStatus = StageStatus.Open;
        else
            stageStatus = StageStatus.Clear;
        stageSelectButton.interactable = true; //버튼 클릭 가능
        stageSlotImage.sprite = openSprite;    //오픈 Sprite로 변경
        go_StageInformation.SetActive( true ); //스테이지 Text, Star Image 활성화
    }

    public void SetOpen()
    {
        bestScore = 0;
        starCount = 0;
        stageSelectButton.interactable = true; //버튼 클릭 가능
        stageSlotImage.sprite = openSprite;    //오픈 Sprite로 변경
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = UIManager.instance.blankStarSprite;
        }
        go_StageInformation.SetActive( true ); //스테이지 Text, Star Image 활성화
        stageStatus = StageStatus.Open;
    }
}
