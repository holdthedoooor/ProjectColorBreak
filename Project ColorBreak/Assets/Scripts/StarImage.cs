using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarImage : MonoBehaviour
{
    public enum StarType
    {
        Star1,
        Star2,
        Star3
    }
    public StarType starType;

    private RectTransform starTransform;
    private Image         starImage;
    public  Sprite        blankStarSprite;
    public  Sprite        starSprite;
    private Text          starText;

    private float   ratio;
    private float   minPositionX = -365f;
    private float   maxPositionX = 365f;

    void Awake()
    {
        starTransform = GetComponent<RectTransform>();
        starImage = GetComponent<Image>();
        starText = GetComponentInChildren<Text>();
    }

    void OnEnable()
    {
        ResetStar();
    }

    private void ResetStar()
    {
        switch(starType)
        {
            case StarType.Star1:
                starText.text = StageManager.instance.stage.checkPoint_1.ToString();
                ratio = StageManager.instance.stage.checkPoint_1 / (float)StageManager.instance.stage.checkPoint_3;
                break;
            case StarType.Star2:
                starText.text = StageManager.instance.stage.checkPoint_2.ToString();
                ratio = StageManager.instance.stage.checkPoint_2 / (float)StageManager.instance.stage.checkPoint_3;
                break;
            default:
                starText.text = StageManager.instance.stage.checkPoint_3.ToString();
                ratio = StageManager.instance.stage.checkPoint_3 / (float)StageManager.instance.stage.checkPoint_3;
                break;
        }

        transform.localPosition = new Vector3( Mathf.Lerp( minPositionX, maxPositionX, ratio ), transform.localPosition.y, transform.localPosition.z );

        starImage.sprite = blankStarSprite;
    }

    public void ChangeStar()
    {
        starImage.sprite = starSprite;
    }

    public void ChangeBlankStar()
    {
        starImage.sprite = blankStarSprite;
    }
}
