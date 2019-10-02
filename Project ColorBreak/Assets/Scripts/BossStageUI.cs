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

    private float ratio;
    private float minPositionX = -360f;
    private float maxPositionX = 360f;

    public void ActivateUI()
    {
        UpdateBossHpText();
        UpdateDamageText();
        bossHpSlider.fillAmount = 1f;
        ratio = (float)StageManager.instance.currentBossStageSlot.hardHp / StageManager.instance.currentBossStageSlot.maxHp;
        hardPhaseLine.transform.localPosition = new Vector3( Mathf.Lerp( minPositionX, maxPositionX, ratio ), hardPhaseLine.transform.localPosition.y, hardPhaseLine.transform.localPosition.z );
        go_BossStageUI.SetActive( true );
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
        damage_Text.text = "Damage : " + StageManager.instance.damage;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
    }

    public void UpdateChallengeCountText()
    {
        challengeCount_Text.text = "Count : " + StageManager.instance.panaltyPoint.ToString();
    }

    public IEnumerator UpdateBossHpSliderCoroutine()
    {
        while (bossHpSlider.fillAmount > StageManager.instance.currentBossStageSlot.currentHp / (float)StageManager.instance.currentBossStageSlot.maxHp)
        {
            bossHpSlider.fillAmount -= Time.deltaTime;
            Debug.Log( "슬라이더 " );
            yield return null;
        }
    }
}
