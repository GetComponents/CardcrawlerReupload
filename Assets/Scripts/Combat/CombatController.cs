using Cards;
using Enemies;
using Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Combat
{
    public class CombatController : MonoBehaviour
    {
        public static CombatController Instance { get; private set; }
        public int Mana
        {
            get => m_mana;
            set
            {
                ManaDisplay.Instance.ChangeMana(value, m_rules.MaxMana);
                m_mana = value;
            }
        }
        public ECombatState CurrentState
        {
            get => m_currentState;
            set
            {
                if (m_currentState == value)
                    return;
                m_currentState = value;
                m_onCurrentStateChanged?.Invoke(m_currentState);
            }
        }
        public event System.Action OnCombatEnds
        {
            add
            {
                m_onCombatEnds -= value;
                m_onCombatEnds += value;
            }
            remove
            {
                m_onCombatEnds -= value;
            }
        }
        public event System.Action<ECombatState> OnCurrentStateChanged
        {
            add
            {
                m_onCurrentStateChanged -= value;
                m_onCurrentStateChanged += value;
            }
            remove
            {
                m_onCurrentStateChanged -= value;
            }
        }

        [SerializeField]
        private EnemyGroup m_enemyGroupPrefab;
        [SerializeField]
        private GameRules m_rules;
        [SerializeField]
        private GameObject m_hand;
        [SerializeField]
        private CardRewardDisplay m_rewardDisplay;
        [SerializeField]
        private Transform[] m_enemiesSpawnPoints;
        [SerializeField]
        private Transform m_playerSpawn;
        [SerializeField]
        private CardsPerSection m_cardsPerSection;

        private int m_mana;
        private EnemyGroup m_enemyGroupInstance;
        private ECombatState m_currentState;
        private CombatTile m_tile;
        private event System.Action m_onCombatEnds;
        private event System.Action<ECombatState> m_onCurrentStateChanged;

        public void EndTurn()
        {
            if (!SelectionManager.Instance.newCardwasPlayed)
            CurrentState = ECombatState.PLAYER_TURN_END;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;

            OnCurrentStateChanged += ReactToStateChange;
        }

        private void Start()
        {
            CombatTile tile = OverworldController.Instance.CurrentTile.Visuals.GetComponent<CombatTile>();
            SetTriggeredTile(tile);
            CurrentState = ECombatState.INITIALIZE;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void SetTriggeredTile(CombatTile _tile)
        {
            m_tile = _tile;
            m_onCombatEnds += m_tile.Complete;
        }

        private void ReactToStateChange(ECombatState _state)
        {
            switch (_state)
            {
                case ECombatState.INITIALIZE:
                    SpawnEncounter();
                    DrawInitialHand();
                    Player.Player.Instance.transform.parent = m_playerSpawn;
                    Player.Player.Instance.transform.localPosition = Vector3.zero;
                    Player.Player.Instance.transform.localEulerAngles = Vector3.zero;
                    Player.Player.Instance.transform.localScale = Vector3.one;
                    CurrentState = ECombatState.PLAYER_TURN_START;
                    break;
                case ECombatState.PLAYER_TURN_START:
                    DrawCards();
                    Mana = m_rules.MaxMana;
                    CombineManager.Instance.CombinedThisTurn = false;
                    break;
                case ECombatState.PLAYER_TURN_END:
                    DiscardRemainingHand();
                    EffectManager.Instance.ReduceBuffs();
                    CurrentState = ECombatState.ENEMY_TURN_START;
                    break;
                case ECombatState.ENEMY_TURN_START:
                    m_enemyGroupInstance.OnEnemyGroupAttackEnd += WaitForEnemyAttackEnd;
                    m_enemyGroupInstance.Action();
                    break;
                case ECombatState.ENEMY_TURN_END:
                    foreach (Enemy _enemy in GameObject.FindObjectsOfType<Enemy>())
                    {
                        _enemy.ReduceBuffs();
                    }
                    CurrentState = ECombatState.PLAYER_TURN_START;
                    break;
                case ECombatState.CHOOSE_REWARD:
                    DisplayRewards();
                    break;
                case ECombatState.COMBAT_END:
                    Player.Player.Instance.transform.parent = null;
                    SceneManager.MoveGameObjectToScene(Player.Player.Instance.gameObject,
                        SceneManager.GetSceneByName("Overworld"));
                    m_onCombatEnds?.Invoke();
                    break;
            }
        }

        private void WaitForEnemyAttackEnd()
        {
            CurrentState = ECombatState.ENEMY_TURN_END;
        }

        private void SpawnEncounter()
        {
            EnemyGroupData encounter = m_tile.Encounter;
            m_enemyGroupInstance = Instantiate(m_enemyGroupPrefab);
            m_enemyGroupInstance.InitEnemyGroup(encounter, m_enemiesSpawnPoints);

            m_enemyGroupInstance.OnAllEnemysDead += WaitForEnemyDeath;
        }

        private void WaitForEnemyDeath()
        {
            CurrentState = ECombatState.CHOOSE_REWARD;
        }

        private void DrawInitialHand()
        {
            DeckManager.Instance.InitializeDeck();
            DeckManager.Instance.ShuffleDeck();
            DeckManager.Instance.Draw(m_rules.InitialHandSize);
        }

        private void DiscardRemainingHand()
        {
            foreach (Card _card in m_hand.GetComponentsInChildren<Card>())
            {
                DeckManager.Instance.DiscardCard(_card.gameObject);
            }
        }

        private void DrawCards()
        {
            DeckManager.Instance.Draw(m_rules.RoundHandSize);
        }

        private void DisplayRewards()
        {
            List<Card> rewards = new List<Card>();
            Card reward;
            while (rewards.Count < 3)
            {
                if (m_tile.Section == 0)
                {
                    reward = m_cardsPerSection.CardsForSectionOne.Random();
                }
                else if (m_tile.Section == 1)
                {
                    reward = m_cardsPerSection.CardsForSectionTwo.Random();
                }
                else
                {
                    reward = m_cardsPerSection.CardsForSectionThree.Random();
                }

                if (rewards.Contains(reward))
                    continue;
                rewards.Add(reward);
            }

            for (int i = 0; i < rewards.Count; i++)
            {
                m_rewardDisplay.DisplayCard(rewards[i], i);
            }

            m_rewardDisplay.transform.parent.gameObject.SetActive(true);
            m_rewardDisplay.OnPlayerChoseCard += RewardWasChoosen;
        }

        private void RewardWasChoosen(Card _card)
        {
            Player.Player.Instance.AddExtraCard(_card);
            
            CurrentState = ECombatState.COMBAT_END;
        }
    }
}