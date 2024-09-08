using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    Image _fillImg;

    public void InitSet()
    {
        _fillImg = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetFillHpBar(float rate)
    {
        _fillImg.fillAmount = rate;
    }
}
