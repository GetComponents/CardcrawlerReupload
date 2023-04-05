using Environment;
using Environment.Generation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class OverworldUI : MonoBehaviour
    {
        public static OverworldUI Instance { get; private set; }

        [SerializeField]
        private string m_menuScene = "MainMenue";
        [SerializeField]
        private GameObject m_winPanel;
        [SerializeField]
        private GameObject m_losePanel;
        [SerializeField]
        private GameObject m_mainPanel;
        [SerializeField]
        private GameObject m_deckDisplay;
        [SerializeField]
        private GameObject m_scrollView;
        [SerializeField]
        private HealTileUI m_healTileUI;
        [SerializeField]
        private ShopUI m_shopUI;

        private List<Card> m_displayedCards = new List<Card>();

        public void ShowHealTileUI(HealTile _requestingTile)
        {
            m_healTileUI.Init(_requestingTile);
            m_healTileUI.gameObject.SetActive(true);
        }

        public void HideHealTileUI()
        {
            m_healTileUI.gameObject.SetActive(false);
        }

        public void ShowShopUI(ShopTile _requestingTile)
        {
            m_shopUI.Init(_requestingTile);
            m_shopUI.gameObject.SetActive(true);
        }

        public void HideShopUI()
        {
            m_shopUI.gameObject.SetActive(false);
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene(m_menuScene);
        }

        public void ToggleDeckView()
        {
            if (m_scrollView.activeInHierarchy)
            {
                HideDeck();
                m_scrollView.SetActive(false);
            }
            else
            {
                ShowDeck();
                m_scrollView.SetActive(true);
            }
        }

        private void ShowDeck()
        {
            List<Card> cards = new List<Card>();
            cards.AddRange(Player.Player.Instance.BasicCards);
            cards.AddRange(Player.Player.Instance.ExtraCards);
            cards = cards.OrderBy(o => o.myName).ToList();

            Card tmp;
            foreach (Card card in cards)
            {
                tmp = Instantiate(card, m_deckDisplay.transform);
                tmp.transform.localPosition = Vector3.zero;
                tmp.gameObject.GetComponent<Draggable>().enabled = false;
                tmp.enabled = false;
                m_displayedCards.Add(tmp);
            }
        }

        private void HideDeck()
        {
            foreach (Card card in m_displayedCards)
            {
                Destroy(card.gameObject);
            }
            m_displayedCards.Clear();
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
        }
        private void Start()
        {
            OverworldController.Instance.OnStateChanged += DisplayWinPanelOnEnd;
            MapCreator.Instance.OnGenerationFinished += DisplayUI;
            Player.Player.Instance.OnPlayerHPChanged += DisplayLosePanelOnDeath;
        }

        private void DisplayUI()
        {
            m_mainPanel.gameObject.SetActive(true);
        }

        private void DisplayLosePanelOnDeath(int _current, int _max)
        {
            if (_current == 0)
            {
                m_losePanel.SetActive(true);
            }
        }

        private void DisplayWinPanelOnEnd(EOverworldStates _state)
        {
            if (_state == EOverworldStates.REACHED_END)
            {
                m_winPanel.SetActive(true);
            }
        }
    }
}