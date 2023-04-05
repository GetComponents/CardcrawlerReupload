using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManaDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;
    public static ManaDisplay Instance;
    private int maxMana;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        text = GetComponent<TextMeshProUGUI>();
    }

    public void ChangeMana(int _manaAmaount, int _maxMana)
    {
        text.text = $"Mana: {_manaAmaount}/{_maxMana}";
    }
}
