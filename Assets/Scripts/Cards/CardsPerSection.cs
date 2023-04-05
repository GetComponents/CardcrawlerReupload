using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "Empty Card Section Data", menuName = "Data/Cards per Section")]
    public class CardsPerSection : ScriptableObject
    {
        public List<Card> CardsForSectionOne => m_cardsForSectionOne;
        public List<Card> CardsForSectionTwo => m_cardsForSectionTwo;
        public List<Card> CardsForSectionThree => m_cardsForSectionThree;

        [SerializeField]
        private List<Card> m_cardsForSectionOne = new List<Card>();
        [SerializeField]
        private List<Card> m_cardsForSectionTwo = new List<Card>();
        [SerializeField]
        private List<Card> m_cardsForSectionThree = new List<Card>();
    }
}
