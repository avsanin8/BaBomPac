using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image healthBarCurrent;
    public Text ratioText;
    public RectTransform healthTr;
    public Unit unit;
    private bool turn = false;

    private void Start()
    {
        UpdateHealthBar();
    }

    private void Update()
    {
        if (!unit.lookRight && !turn)
            FixedPosition();
        if (unit.lookRight && turn)
            FixedPosition();


        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float ratio = unit.hitPoint / unit.maxHitPoint;
        healthBarCurrent.rectTransform.localScale = new Vector3(ratio, 1, 1);
        ratioText.text = (ratio * 100).ToString("0") + "%";
    }
    

    private void FixedPosition()
    {
        Vector2 curPos = healthTr.localScale;
        curPos.x *= -1;
        healthTr.localScale = curPos;
        turn = !turn;        
    }
}
