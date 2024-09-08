using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerWindow : MonoBehaviour
{
    Image _fillImg;
    TextMeshProUGUI _hpText;

    public void InitSet()
    {
        _fillImg = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        _hpText = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetValue(float hpRate, int maxhp, int curhp)
    {
        _fillImg.fillAmount = hpRate;
        _hpText.text = curhp + "/" + maxhp;
    }
}
