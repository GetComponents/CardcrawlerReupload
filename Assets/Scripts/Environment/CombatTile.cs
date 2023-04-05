using Combat;
using Enemies;
using Environment.Generation;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Environment
{
    [DisallowMultipleComponent]

    public class CombatTile : MonoBehaviour, ITile
    {
        public Node Node { get => m_node; set => m_node = value; }
        public int Section { get => m_section; set => m_section = value; }
        public EnemyGroupData Encounter => m_encounter;
        public GameObject Visuals => gameObject;

        [SerializeField]
        private List<string> m_sectionMaps;
        [SerializeField]
        private EncounterSettings m_encounterSettings;
        [SerializeField]
        private SpriteRenderer[] m_enemyPositions;

        private int m_section;
        private Node m_node;
        private Collider m_collider;
        private EnemyGroupData m_encounter;

        public event Action OnTileFinishedExecution;

        private void Awake()
        {
            m_collider = GetComponent<Collider>();
        }

        public void Activate()
        {
            SceneManager.LoadScene(m_sectionMaps[m_section], LoadSceneMode.Additive);
            SceneManager.sceneLoaded += MarkCombatSceneAsActive;
        }

        private void MarkCombatSceneAsActive(Scene _scene, LoadSceneMode _mode)
        {
            SceneManager.SetActiveScene(_scene);
            SceneManager.sceneLoaded -= MarkCombatSceneAsActive;
        }

        public void Highlight()
        {
            if (m_encounter == null)
            {
                switch (m_section)
                {
                    case 0:
                        m_encounter = m_encounterSettings.EncountersSectionOne.Random();
                        break;
                    case 1:
                        m_encounter = m_encounterSettings.EncountersSectionTwo.Random();
                        break;
                    case 2:
                        m_encounter = m_encounterSettings.EncountersSectionThree.Random();
                        break;
                }
            }

            if (m_encounter.EnemyOne != null)
            {
                m_enemyPositions[0].sprite = m_encounter.EnemyOne.Modell;
            }
            if (m_encounter.EnemyTwo != null)
            {
                m_enemyPositions[1].sprite = m_encounter.EnemyTwo.Modell;
            }
            if (m_encounter.EnemyThree != null)
            {
                m_enemyPositions[2].sprite = m_encounter.EnemyThree.Modell;
            }
            if (m_encounter.EnemyFour != null)
            {
                m_enemyPositions[3].sprite = m_encounter.EnemyFour.Modell;
            }
        }

        public void Unhighlight()
        {
            foreach (SpriteRenderer renderer in m_enemyPositions)
            {
                renderer.sprite = null;
            }
        }

        public void ChangeCollisionEnabled(bool _enabled)
        {
            m_collider.enabled = _enabled;
        }

        public void Complete()
        {
            SceneManager.UnloadSceneAsync(m_sectionMaps[m_section]);
            OnTileFinishedExecution?.Invoke();
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
    }
}
