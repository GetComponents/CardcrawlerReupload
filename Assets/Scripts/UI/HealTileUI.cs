using Environment;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HealTileUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_cardDisplay;
        [SerializeField]
        private TextMeshProUGUI m_text;
        [SerializeField]
        private GameObject m_acceptButton;

        private HealTile m_healTile;
        private Card m_offeredCard;
        private Card m_displayedCard;

        public void TradeCardForLife()
        {
            if (Player.Player.Instance.ExtraCards.Contains(m_offeredCard))
            {
                Player.Player.Instance.ExtraCards.Remove(m_offeredCard);
            }
            else
            {
                Player.Player.Instance.BasicCards.Remove(m_offeredCard);
            }

            Player.Player.Instance.SetHealing(m_healTile.HealAmount);
            Destroy(m_displayedCard.gameObject);

            OnEnable();
        }

        public void RefuseOffer()
        {
            m_healTile.Complete();
        }

        public void Init(HealTile _healTile)
        {
            m_healTile = _healTile;
        }

        private void OnEnable()
        {
            List<Card> cards = new List<Card>();
            cards.AddRange(Player.Player.Instance.BasicCards);
            cards.AddRange(Player.Player.Instance.ExtraCards);
          
            if (cards.Count == 1)
            {
                m_acceptButton.SetActive(false);
                m_text.text = "You cannot afford to burn more cards. You leave this place....";
            }
            else
            {
                m_text.text = $"You can burn this card to heal yourself. ({m_healTile.HealAmount})";
                m_offeredCard = cards.Random();

                m_displayedCard = Instantiate(m_offeredCard);
                m_displayedCard.enabled = false;
                m_displayedCard.GetComponent<Draggable>().enabled = false;
                m_displayedCard.transform.SetParent(m_cardDisplay.transform);
                m_displayedCard.transform.localPosition = Vector3.zero;
                RectTransform newTrans = m_displayedCard.transform as RectTransform;
                newTrans.anchorMin = Vector2.zero;
                newTrans.anchorMax = Vector2.one;
                newTrans.offsetMin = Vector2.zero;
                newTrans.offsetMax = Vector2.zero;
            }
        }

        
    }
}
