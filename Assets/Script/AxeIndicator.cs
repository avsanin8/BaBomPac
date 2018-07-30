using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxeIndicator : MonoBehaviour {
   
    public Player player;
    public Image skillIcon;     

    private void Update()
    {
        skillIcon.fillAmount = player.CurWeaponCD / player.WeaponCD;
    }
    

}
