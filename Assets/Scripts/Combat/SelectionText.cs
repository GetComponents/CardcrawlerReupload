using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectionText : MonoBehaviour
{
    public static SelectionText Instance;
    public TextMeshProUGUI selectionText;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void selectEnemy()
    {
        if (SelectionManager.Instance.currentCard.CardEffects[SelectionManager.Instance.currentCard.effectIteration] == EEffectType.ATTACKONE)
        {
           selectionText.text = "Select an Enemy to attack";
        }
        else
        {
            selectionText.text = "Select an Enemy to heal";
        }
    }

    public void selectCard()
    {
        selectionText.text = "Select a Card to discard";
    }

    public void ResetText()
    {
        selectionText.text = "";
    }
}
