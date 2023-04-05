using Cards;
using Environment;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_text;
        [SerializeField]
        private CardsPerSection m_cardsPerSection;
        [SerializeField]
        private GameObject m_playerCardDisplay;
        [SerializeField]
        private GameObject m_newCardDisplay;

        private ShopTile m_shopTile;
        private Card m_playerCard;
        private Card m_newCard;
        private Card m_playerCardInstance;
        private Card m_newCardInstance;

        public void AcceptOffer()
        {
            if (!Player.Player.Instance.ExtraCards.Remove(m_playerCard))
            {
                Player.Player.Instance.BasicCards.Remove(m_playerCard);
            }
            Player.Player.Instance.ExtraCards.Add(m_newCard);

            Destroy(m_newCardInstance);
            Destroy(m_playerCardInstance);

            m_shopTile.Complete();
        }

        public void RefuseOffer()
        {
            m_shopTile.Complete();
        }

        public void Init(ShopTile _shopTile)
        {
            m_shopTile = _shopTile;
        }

        private void OnEnable()
        {
            List<Card> cards = new List<Card>();
            cards.AddRange(Player.Player.Instance.BasicCards);
            cards.AddRange(Player.Player.Instance.ExtraCards);

            m_playerCard = cards.Random();
            m_playerCardInstance = Instantiate(m_playerCard, m_playerCardDisplay.transform);
            m_playerCardInstance.transform.localPosition = Vector3.zero;
            m_playerCardInstance.enabled = false;
            m_playerCardInstance.GetComponent<Draggable>().enabled = false;
            RectTransform playerTrans = m_playerCardInstance.transform as RectTransform;
            playerTrans.anchorMin = Vector2.zero;
            playerTrans.anchorMax = Vector2.one;
            playerTrans.offsetMin = Vector2.zero;
            playerTrans.offsetMax = Vector2.zero;

            if (m_shopTile.Section == 0)
            {
                m_newCard = m_cardsPerSection.CardsForSectionOne.Random();
            }
            else if (m_shopTile.Section == 1)
            {
                m_newCard = m_cardsPerSection.CardsForSectionTwo.Random();
            }
            else
            {
                m_newCard = m_cardsPerSection.CardsForSectionThree.Random();
            }
            m_newCardInstance = Instantiate(m_newCard, m_newCardDisplay.transform);
            m_newCardInstance.InitiateCard();
            m_newCardInstance.transform.localPosition = Vector3.zero;
            m_newCardInstance.enabled = false;
            m_newCardInstance.GetComponent<Draggable>().enabled = false;
            RectTransform newTrans = m_newCardInstance.transform as RectTransform;
            newTrans.anchorMin = Vector2.zero;
            newTrans.anchorMax = Vector2.one;
            newTrans.offsetMin = Vector2.zero;
            newTrans.offsetMax = Vector2.zero;

            m_text.text = "Can I interest you in a trade?";
        }
    }
}