using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStageUI : MonoBehaviour
{
    public GameObject       go_BossStageUI;
    public Text             bossHp_Text;
    public Text             damage_Text;
    public Text             challengeCount_Text;
    public Image            bossHpSlider;
    public Image            hardPhaseLine;
    public Sprite[]         sliderSprites;
    public Image            stopImage;
    public Image            grrrrImage;

    public GameObject       go_DeactiveUI;

    private float ratio;
    private float minPositionX = -400f;
    private float maxPositionX = 400f;

    public Vector3 grrrrImageOrizinPos;

    private void Awake()
    {
        grrrrImageOrizinPos = grrrrImage.transform.localPosition;
    }

    public void ActivateUI()
    {
        bossHpSlider.sprite = sliderSprites[StageManager.instance.currentChapter - 1]; //해당 챕터에 맞는 슬라이더 이미지로 변경

        UpdateBossHpText(); //BossHpText를 보스의 최대 체력으로 변경

        bossHpSlider.fillAmount = 1f; //슬라이더 Fill을 꽉차게

        grrrrImage.gameObject.SetActive( false );

        go_BossStageUI.SetActive( true );

        if(StageManager.instance.currentBossStageSlot.bossStageType != BossStageSlot.BossStageType.BounceAttack)
        {
            UpdateDamageText();
            ratio = (float)StageManager.instance.currentBossStageSlot.hardHp / StageManager.instance.currentBossStageSlot.maxHp;
            hardPhaseLine.transform.localPosition = new Vector3( Mathf.Lerp( minPositionX, maxPositionX, ratio ), hardPhaseLine.transform.localPosition.y, hardPhaseLine.transform.localPosition.z );
            go_DeactiveUI.SetActive( true );
        }
        else
        {
            go_DeactiveUI.SetActive( false );
        }
    }

    public void DeactivateUI()
    {
        go_BossStageUI.SetActive( false);
    }

    public void UpdateBossHpText()
    {
        bossHp_Text.text = StageManager.instance.currentBossStageSlot.currentHp.ToString() + " / " + StageManager.instance.currentBossStageSlot.maxHp.ToString();
    }

    public void UpdateDamageText()
    {
        damage_Text.text = StageManager.instance.damage.ToString();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
    }

    public void UpdateChallengeCountText()
    {
        challengeCount_Text.text = StageManager.instance.panaltyPoint.ToString();
    }

    public IEnumerator UpdateBossHpSliderCoroutine()
    {
        while (bossHpSlider.fillAmount > StageManager.instance.currentBossStageSlot.currentHp / (float)StageManager.instance.currentBossStageSlot.maxHp)
        {
            bossHpSlider.fillAmount -= Time.deltaTime;
            yield return null;
        }
    }
}
