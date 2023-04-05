using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "GroupName", menuName = "Data/EnemyOption/EnemyGroup", order = 2), System.Serializable]
    public class EnemyGroupData : ScriptableObject
    {
        [SerializeField]
        private EnemyData enemyOne = null;
        [SerializeField]
        private EnemyData enemyTwo = null;
        [SerializeField]
        private EnemyData enemyThree = null;
        [SerializeField]
        private EnemyData enemyFour = null;

        #region Getter
        public EnemyData EnemyOne { get { return enemyOne; } }
        public EnemyData EnemyTwo { get { return enemyTwo; } }
        public EnemyData EnemyThree { get { return enemyThree; } }
        public EnemyData EnemyFour { get { return enemyFour; } }
        #endregion
    }
}