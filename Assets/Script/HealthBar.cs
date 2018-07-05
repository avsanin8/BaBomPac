using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image healthBarCurrent;
    public Text ratioText;
    public RectTransform healthTr;
    public Unit unit;

    //private float hitPoint = 150;
    //private float maxHitPoint = 150;

    private void Start()
    {
        UpdateHealthBar();
    }

    private void Update()
    {
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
        Vector2 itemPos = healthTr.position;

        if (itemPos.x > healthTr.position.x)
            healthTr.eulerAngles = new Vector3(0, 180, 0);
        else
            healthTr.eulerAngles = new Vector3(0, 0, 0);
    }
}
