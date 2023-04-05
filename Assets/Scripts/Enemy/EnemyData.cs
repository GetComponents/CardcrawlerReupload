using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "EnemyName", menuName = "Data/EnemyOption/Enemy", order = 1), System.Serializable]
    public class EnemyData : ScriptableObject
    {
        [SerializeField]
        private string enemyName = "EnemyName";
        [SerializeField]
        private string description = "unkown";

        [SerializeField, Range(0, 100)]
        private int hitPoints = 0;
        [SerializeField, Range(0, 10)]
        private int attackDamageOne = 0;
        [SerializeField, Range(0, 10)]
        private int attackDamageTwo = 0;
        [SerializeField, Range(0, 10)]
        private int attackDamageThree = 0;

        [SerializeField]
        private Sprite modell;

        #region Getter
        public string Name { get { return enemyName; } }
        public string Description { get { return description; } }
        public int HitPoints { get { return hitPoints; } }
        public int AttackDamageOne { get { return attackDamageOne; } }
        public int AttackDamageTwo { get { return attackDamageTwo; } }
        public int AttackDamageThree { get { return attackDamageThree; } }

        public Sprite Modell { get { return modell; } }
        #endregion
    }
}