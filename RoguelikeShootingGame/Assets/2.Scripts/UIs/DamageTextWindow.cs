using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextWindow : MonoBehaviour
{
    TextMeshProUGUI _text;

    public void InitSet(int damage, Vector2 position)
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _text.text = damage.ToString();

        transform.position = position;

        Destroy(gameObject, 0.5f);
    }
}
