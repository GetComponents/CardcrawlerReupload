using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("All functionaliy has beed moved to CombatController.cs", true)]
public class TurnManager : MonoBehaviour
{
    [System.Obsolete("All functionaliy has beed moved to CombatController.cs", true)]
    public static TurnManager Instance;
    [SerializeField]
    GameObject hand;
    public int Mana
    {
        get => m_Mana;
        set
        {
            ManaDisplay.Instance.ChangeMana(value, MaxMana);
            m_Mana = value;
        }
    }
    public int MaxMana;
    private int m_Mana;

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EndTurn()
    {
        foreach (Card _card in hand.GetComponentsInChildren<Card>())
        {
            DeckManager.Instance.DiscardCard(_card.gameObject);
        }
        //TODO: Gegner Zug
        StartTurn();
    }

    public void StartTurn()
    {
        Mana = MaxMana;
        DeckManager.Instance.Draw(3);
    }
}
