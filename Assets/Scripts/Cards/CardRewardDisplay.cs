using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CardRewardDisplay : MonoBehaviour
    {
        public event System.Action<Card> OnPlayerChoseCard
        {
            add
            {
                m_onPlayerChoseCard -= value;
                m_onPlayerChoseCard += value;
            }
            remove
            {
                m_onPlayerChoseCard -= value;
            }
        }

        [SerializeField]
        private Transform[] m_cardAttachPoints;

        private Card[] m_availableCards = new Card[3];
        private event System.Action<Card> m_onPlayerChoseCard;

        public void DisplayCard(Card _card, int _index)
        {
            Card tmp = Instantiate(_card);
            tmp.transform.SetParent(m_cardAttachPoints[_index], false);
            tmp.transform.localPosition = Vector3.zero;
            tmp.InitiateCard();
            RectTransform rect = tmp.transform as RectTransform;
            rect.sizeDelta = new Vector2(360, 500);
            m_availableCards[_index] = _card;
        }

        public void ChooseCard(int _index)
        {
            m_onPlayerChoseCard?.Invoke(m_availableCards[_index]);
        }
    }
}