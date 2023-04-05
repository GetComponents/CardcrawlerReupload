using Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "Empty Encounter Settings", menuName = "Data/Encounter Settings")]
    public class EncounterSettings : ScriptableObject
    {
        public List<EnemyGroupData> EncountersSectionOne => m_encountersSectionOne;
        public List<EnemyGroupData> EncountersSectionTwo => m_encountersSectionTwo;
        public List<EnemyGroupData> EncountersSectionThree => m_encountersSectionThree;

        [SerializeField]
        private List<EnemyGroupData> m_encountersSectionOne;
        [SerializeField]
        private List<EnemyGroupData> m_encountersSectionTwo;
        [SerializeField]
        private List<EnemyGroupData> m_encountersSectionThree;
    }
}