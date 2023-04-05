using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public delegate void OnPlayerHPChangedDelegate(int _current, int _max);

    public class Player : MonoBehaviour
    {
        public static Player Instance = null;

        public GameObject WeakUI, StrongUI;

        public event OnPlayerHPChangedDelegate OnPlayerHPChanged
        {
            add
            {
                m_onPlayerHPChanged -= value;
                m_onPlayerHPChanged += value;
            }
            remove
            {
                m_onPlayerHPChanged -= value;
            }
        }

        #region Values
        [SerializeField, Range(0, 100)]
        private int m_maxHP = 1;
        [SerializeField]
        private TextMeshProUGUI m_hpText;
        [SerializeField]
        private Gradient m_hpColor;
        [SerializeField]
        private Slider m_hpSlider;

        private int m_currentHP;
        private List<Card> m_extraCards = new List<Card>();
        [SerializeField]
        private List<Card> m_basicCards = new List<Card>();

        private event OnPlayerHPChangedDelegate m_onPlayerHPChanged;
        #endregion

        #region Propertys
        public int CurrentHP
        {
            get
            {
                return m_currentHP;
            }
            set
            {
                m_currentHP = Mathf.Clamp(value, 0, m_maxHP);
                m_hpText.text = m_currentHP.ToString();
                m_hpSlider.value = m_currentHP / (float)m_maxHP;
                m_hpSlider.targetGraphic.color = m_hpColor.Evaluate(m_currentHP / (float)m_maxHP);

                m_onPlayerHPChanged?.Invoke(m_currentHP, m_maxHP);
            }
        }
        public int MaxHP => m_maxHP;
        public List<Card> ExtraCards => m_extraCards;
        public List<Card> BasicCards => m_basicCards;
        #endregion

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            CurrentHP = m_maxHP;
            foreach (Card _card in BasicCards)
            {
                _card.InitiateCard();
            }
        }

        public void SetDamage(int _damage)
        {
            CurrentHP -= _damage;
        }

        public void SetHealing(int _healAmount)
        {
            CurrentHP += _healAmount;
        }

        public void AddExtraCard(Card _card)
        {
            m_extraCards.Add(_card);
        }
    }
}
