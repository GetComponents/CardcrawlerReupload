using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Combat;
using Enemies;

public class SelectionManager : MonoBehaviour, IDropHandler//, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void ActionList(int _iteration);
    public ActionList ListOfEffects;
    public bool waitingForEnemySelection, waitingForCardSelection, newCardwasPlayed;
    [SerializeField]
    TextMeshProUGUI selectionText;
    public static SelectionManager Instance;
    public List<ESelectionType> m_effectList;
    public List<ESelectionType> EffectToDoList
    {
        get => m_effectList;
        set
        {
            if (newCardwasPlayed)
            {
                if (value.Count > 0)
                {
                    switch (value[0])
                    {
                        case ESelectionType.ENEMY:
                            if (FindObjectsOfType<Enemy>().Length > 0)
                            {
                                waitingForEnemySelection = true;
                                SelectionText.Instance.selectEnemy();
                            }
                            else
                            {
                                ChangeToDoList(value[0], false);
                                DeckManager.Instance.LastCard.GetComponent<Card>().increaseIteration();
                            }
                            break;
                        case ESelectionType.CARD:
                            waitingForCardSelection = true;
                            SelectionText.Instance.selectCard();
                            break;
                        default:
                            DeckManager.Instance.LastCard.GetComponent<Card>().InvokeEffect();
                            break;
                    }
                    foreach (Draggable _card in hand.GetComponentsInChildren<Draggable>())
                    {
                        _card.enabled = false;
                    }
                }
                else
                {
                    FinishCardDrop();
                    newCardwasPlayed = false;
                    currentCard.GetComponent<Draggable>().enabled = true;
                    currentCard.GetComponent<Card>().enabled = true;
                    //Instance.StopSelection();
                    currentCard = null;
                    //foreach (Draggable _card in hand.GetComponentsInChildren<Draggable>())
                    //{
                    //    _card.enabled = true;
                    //}
                }
            }
            m_effectList = value;
        }
    }
    public Card currentCard;
    [SerializeField]
    GameObject hand;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        EffectToDoList = new List<ESelectionType>();
    }

    public void ChangeToDoList(ESelectionType _selection, bool increase)
    {
        Debug.Log($"TodoList: {_selection}, {increase}");
        List<ESelectionType> tmp = EffectToDoList;
        if (increase)
        {
            tmp.Add(_selection);
        }
        else
        {
            tmp.Remove(_selection);
        }
        EffectToDoList = tmp;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Draggable cardDrag = eventData.pointerDrag.GetComponent<Draggable>();
        Card card = eventData.pointerDrag.GetComponent<Card>();
        if (cardDrag != null && card != null && currentCard == null)
        {
            if (CombatController.Instance.Mana >= card.cost)
            {
                if (card.IsLegalCard())
                {
                    //newCardwasPlayed = true;
                    DeckManager.Instance.LastCard = eventData.pointerDrag;
                    currentCard = card;
                    currentCard.GetComponent<Draggable>().enabled = false;
                    cardDrag.MyParent = transform;
                    currentCard.transform.SetParent(cardDrag.MyParent);
                    card.CheckIfSelectionNeeded();
                }
                else
                {
                    selectionText.text = $"Not enough Cards to play {card.myName}";
                }
            }
            else
            {
                selectionText.text = $"Not enough Mana to play {card.myName}";
            }
        }
    }

    public void StartSelection()
    {
        foreach (Draggable _cardDraggable in hand.GetComponentsInChildren<Draggable>())
        {
            _cardDraggable.enabled = false;
        }
    }

    public void StopSelection()
    {
        foreach (Draggable _cardDraggable in hand.GetComponentsInChildren<Draggable>())
        {
            _cardDraggable.enabled = true;
        }
    }

    public void SelectEnemy(Enemy _enemy)
    {
        if (waitingForEnemySelection)
        {
            currentCard.GiveEnemy(_enemy);
            waitingForEnemySelection = false;
            SelectionText.Instance.ResetText();
            ChangeToDoList(EffectToDoList[0], false);
        }
    }

    public void SelectCard(Card _selectedCard)
    {
        if (waitingForCardSelection)
        {
            currentCard.DiscardCard(_selectedCard);
            foreach (Draggable _cardDraggable in hand.GetComponentsInChildren<Draggable>())
            {
                _cardDraggable.enabled = true;
            }
            waitingForCardSelection = false;
            SelectionText.Instance.ResetText();
            ChangeToDoList(EffectToDoList[0], false);
        }
    }

    public void FinishCardDrop()
    {
        StopSelection();
        DeckManager.Instance.DiscardCard(DeckManager.Instance.LastCard);
        CombatController.Instance.Mana -= DeckManager.Instance.LastCard.GetComponent<Card>().cost;
    }
}
