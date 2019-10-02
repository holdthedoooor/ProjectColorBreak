using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStageInformationUI : MonoBehaviour
{
    public GameObject go_BossStageInformationUI;

    public Text[]  panaltyPointTexts;
    public Text  minPanaltyPoint_Text;
    public Image[] starImages;

    //현재 슬롯 및 스테이지를 null로 바꿔준다.
    public void StageSelectCancelButton()
    {
        go_BossStageInformationUI.SetActive( false );
        StageManager.instance.currentBossStageSlot = null;
        StageManager.instance.currentBossStage = null;
    }

    //최대 점수, Star Image, CheckPountText 초기화
    public void ResetBossStageInformation()
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = UIManager.instance.blankStarSprite;
            panaltyPointTexts[i].text = StageManager.instance.currentBossStageSlot.panaltyPoints[i].ToString();
            panaltyPointTexts[i].enabled = true;
        }
    }

    //클릭한 StageSlot의 정보에 맞게 설정
    public void SetStageInformation()
    {
        if (StageManager.instance.currentBossStageSlot != null)
        {
            if (StageManager.instance.currentBossStageSlot.starCount > 0)
            {
                for (int i = 0; i < StageManager.instance.currentBossStageSlot.starCount; i++)
                {
                    starImages[i].sprite = UIManager.instance.starSprite;
                    panaltyPointTexts[i].text = StageManager.instance.currentBossStageSlot.panaltyPoints[i].ToString();
                }
            }

            if (StageManager.instance.currentBossStageSlot.starCount == 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    panaltyPointTexts[i].enabled = false;
                }
            }
            else if (StageManager.instance.currentBossStageSlot.starCount == 2)
            {
                panaltyPointTexts[0].enabled = false;
                panaltyPointTexts[1].enabled = false;
            }
            else if (StageManager.instance.currentBossStageSlot.starCount == 1)
                panaltyPointTexts[0].enabled = false;

            if (StageManager.instance.currentBossStageSlot.minPanaltyPoint == 100)
                minPanaltyPoint_Text.text = "0";
            else
                minPanaltyPoint_Text.text = StageManager.instance.currentBossStageSlot.minPanaltyPoint.ToString();

            go_BossStageInformationUI.SetActive( true );
        }
    }

    //누르면 게임 시작
    public void StageStartButton()
    {
        UIManager.instance.chapterSelectUI.go_CurrentChapterUI.SetActive( false );
        go_BossStageInformationUI.SetActive( false );

        //보스 스테이지면
        if(StageManager.instance.currentBossStageSlot != null)
        {
            StageManager.instance.panaltyPoint = 0;
            if (StageManager.instance.currentBossStageSlot.isRandom)
                StageManager.instance.currentBossStage = Instantiate( StageManager.instance.currentBossStageSlot.go_BossStageNormals[Random.Range( 0, StageManager.instance.currentBossStageSlot.go_BossStageNormals.Length )]
                    , new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();
            else
                StageManager.instance.currentBossStage = Instantiate( StageManager.instance.currentBossStageSlot.go_BossStageNormals[0]
                    , new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();
        }
    }
}
