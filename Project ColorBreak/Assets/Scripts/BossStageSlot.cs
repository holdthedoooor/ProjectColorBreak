﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStageSlot : MonoBehaviour
{
    public enum BossStageStatus
    {
        Rock, //잠겨있다.
        Open, //이전 스테이지를 통과한 경우 열린다. 별이 0개인 경우
        Clear //별을 1개라도 획득하면 클리어
    }
    public BossStageStatus bossStageStatus;

    public Image stageSlotImage;
    public Image[] starImages;
    public Button stageSelectButton;
    public GameObject go_RockImage;
    public GameObject[] go_BossStageNormals;
    public GameObject go_BossStageHard;
    public GameObject go_BossStageInformation;

    //보스의 최대 체력
    public int maxHp;
    //보스의 hard페이즈 체력
    public int hardHp;
    public int currentHp;

    public int[] panaltyPoints;
    //재도전 횟수
    public int minPanaltyPoint = 100;
    public int starCount = 0; //별이 몇개 채워졌는지 저장

    [Header("체크하면 Normal Phase가 랜덤으로 나온다")]
    public bool isRandom;

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
    public void BossStageSlotOpen()
    {
        if (bossStageStatus == BossStageStatus.Rock)
        {
            stageSelectButton.interactable = true; //버튼 클릭 가능
            go_BossStageInformation.SetActive( true ); //스테이지 Text, Star Image 활성화
            bossStageStatus = BossStageStatus.Open;
        }
    }

    //스테이지 선택 버튼
    public void BossStageSelectButton()
    {
        StageManager.instance.currentBossStageSlot = transform.GetComponent<BossStageSlot>();

        UIManager.instance.bossStageInformationUI.ResetBossStageInformation();
        UIManager.instance.bossStageInformationUI.SetStageInformation();
    }

    //마스터 모드 
    public void SetOpen()
    {
        starCount = 0;
        stageSelectButton.interactable = true; //버튼 클릭 가능
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = UIManager.instance.blankStarSprite;
        }
        go_BossStageInformation.SetActive( true ); //스테이지 Text, Star Image 활성화
        bossStageStatus = BossStageStatus.Open;
    }

    public void SetBossStageSlot( int _statusNumber, int _minPanaltyPoint, int _starCount)
    {
        Debug.Log( "보스 스테이지 설정" );
        if (_statusNumber == 1)
            bossStageStatus = BossStageStatus.Open;
        else if (_statusNumber == 2)
            bossStageStatus = BossStageStatus.Clear;
        else
            return;

        minPanaltyPoint = _minPanaltyPoint;
        starCount = _starCount;

        StarImageChange();

        go_BossStageInformation.SetActive( true );
        stageSelectButton.interactable = true;
    }
}
