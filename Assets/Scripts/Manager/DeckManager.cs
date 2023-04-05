using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;
    public List<GameObject> deckCards = new List<GameObject>();
    public List<GameObject> handCards = new List<GameObject>();
    public List<GameObject> discardCards = new List<GameObject>();
    public GameObject LastCard;

    [SerializeField]
    GameObject slashPrefab, superSlashPrefab, hand, discardPile, deck;

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
    }

    private void Start()
    {
        InitializeDeck();
        ShuffleDeck();
    }

    public void InitializeDeck()
    {
        deckCards = new List<GameObject>();
        foreach (Card _card in Player.Player.Instance.BasicCards)
        {
            deckCards.Add(Instantiate(_card.gameObject, deck.transform));
        }
        foreach (Card card in Player.Player.Instance.ExtraCards)
        {
            deckCards.Add(Instantiate(card.gameObject, deck.transform));
        }
    }

    public void ShuffleDeck()
    {
        List<GameObject> tmp = new List<GameObject>();
        int deckLength = deckCards.Count;
        for (int i = 0; i < deckLength; i++)
        {
            GameObject temp = deckCards[Random.Range(0, deckCards.Count - 1)];
            tmp.Add(temp);
            deckCards.Remove(temp);
            temp.transform.SetParent(deck.transform);
        }
        deckCards = tmp;
    }

    public void Draw(int _amount)
    {
        for (int i = 0; i < _amount; i++)
        {
            if (deckCards.Count > 0)
            {
                GameObject tmp = deckCards[0];
                handCards.Add(tmp);
                tmp.transform.SetParent(hand.transform);
                tmp.GetComponent<Draggable>().MyParent = hand.transform;
                deckCards.Remove(tmp);
            }
            else if (discardCards.Count > 0)
            {
                TurnDiscardToDeck();
                Draw(1);
            }
        }
    }

    public void DiscardCard(GameObject _cardToDiscard)
    {
        _cardToDiscard.GetComponent<Draggable>().MyParent = discardPile.transform;
        _cardToDiscard.transform.SetParent(discardPile.transform);
        discardCards.Add(_cardToDiscard);
        handCards.Remove(_cardToDiscard);
    }

    public void TurnDiscardToDeck()
    {
        deckCards = discardCards;
        discardCards = new List<GameObject>();
        ShuffleDeck();
    }
}
