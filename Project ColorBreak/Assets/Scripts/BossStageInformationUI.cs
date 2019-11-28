using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStageInformationUI : MonoBehaviour
{
    public GameObject go_BossStageInformationUI;
    //BounceAttack 패턴의 보스 스테이지 슬롯을 눌렀을 때 나오는 UI
    public GameObject go_BounceAttackUI;

    public Text[]  heartPoint_Texts;
    public Text  heartPoint_Text;
    public Image[] starImages;
    public Image[] starImages2;

    //현재 슬롯 및 스테이지를 null로 바꿔준다.
    public void StageSelectCancelButton()
    {
        Quit.instance.DeactivePopUP();
        StageManager.instance.currentBossStageSlot = null;
        StageManager.instance.currentBossStage = null;
    }

    //최대 점수, Star Image, CheckPountText 초기화
    public void ResetBossStageInformation()
    {
        if(StageManager.instance.currentBossStageSlot.bossStageType != BossStageSlot.BossStageType.BounceAttack)
        {
            for (int i = 0; i < StageManager.instance.currentBossStageSlot.panaltyPoints.Length; i++)
            {
                heartPoint_Texts[i].text = StageManager.instance.currentBossStageSlot.panaltyPoints[i].ToString();
            }

            for (int i = 0; i < starImages.Length; i++)
            {
                starImages[i].sprite = UIManager.instance.blankStarSprite;
            }
        }
        else
        {
            for (int i = 0; i < starImages2.Length; i++)
            {
                starImages2[i].sprite = UIManager.instance.blankStarSprite;
            }
        }

    }

    //클릭한 StageSlot의 정보에 맞게 설정
    public void SetStageInformation()
    {
        if (StageManager.instance.currentBossStageSlot != null)
        {

            if (StageManager.instance.currentBossStageSlot.bossStageType != BossStageSlot.BossStageType.BounceAttack)
            {
                if (StageManager.instance.currentBossStageSlot.minPanaltyPoint == 100)
                    heartPoint_Text.text = "0";
                else
                    heartPoint_Text.text = StageManager.instance.currentBossStageSlot.minPanaltyPoint.ToString();

                if (StageManager.instance.currentBossStageSlot.starCount > 0)
                {
                    for (int i = 0; i < StageManager.instance.currentBossStageSlot.starCount; i++)
                        starImages[i].sprite = UIManager.instance.starSprite;
                }

                Quit.instance.ActivePopUp( go_BossStageInformationUI, Quit.instance.quitStatus );
            }
            else
            {
                if (StageManager.instance.currentBossStageSlot.starCount > 0)
                {
                    for (int i = 0; i < StageManager.instance.currentBossStageSlot.starCount; i++)
                        starImages2[i].sprite = UIManager.instance.starSprite;
                }
                Quit.instance.ActivePopUp( go_BounceAttackUI, Quit.instance.quitStatus );
            }
        }
    }

    //누르면 게임 시작
    public void StageStartButton()
    {
        SoundManager.instance.PlaySFX( "Click_2" );
        SoundManager.instance.PlayBGM( "BGM_3" );

        UIManager.instance.chapterSelectUI.go_CurrentChapterUI.SetActive( false );

        if (StageManager.instance.currentBossStageSlot.bossStageType != BossStageSlot.BossStageType.BounceAttack)
            go_BossStageInformationUI.SetActive( false );
        else
            go_BounceAttackUI.SetActive( false );

        //보스 스테이지면
        StageManager.instance.panaltyPoint = 0;
        if (StageManager.instance.currentBossStageSlot.bossStageType == BossStageSlot.BossStageType.StageRandom)
            StageManager.instance.currentBossStage = Instantiate( StageManager.instance.currentBossStageSlot.go_BossStageNormals[Random.Range( 0, StageManager.instance.currentBossStageSlot.go_BossStageNormals.Length )]
                , new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();
        else if(StageManager.instance.currentBossStageSlot.bossStageType == BossStageSlot.BossStageType.BounceAttack)
            StageManager.instance.currentBossStage = Instantiate( StageManager.instance.currentBossStageSlot.go_BossStageNormals[0]
                , new Vector3( 0, 0, 0 ), Quaternion.identity ).GetComponent<BossStage>();
    }
}
