using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyGroup : MonoBehaviour
    {
        [SerializeField]
        private Enemy m_enemyPrefab;

        private EnemyGroupData enemyGroupData;

        #region Values
        private Enemy enemyOne = null;
        private Enemy enemyTwo = null;
        private Enemy enemyThree = null;
        private Enemy enemyFour = null;
        #endregion

        #region Events
        public event System.Action OnEnemyGroupAttackEnd
        {
            add
            {
                enemyGroupAttackEnd -= value;
                enemyGroupAttackEnd += value;
            }
            remove
            {
                enemyGroupAttackEnd -= value;
            }
        }
        private event System.Action enemyGroupAttackEnd;

        public event System.Action OnAllEnemysDead
        {
            add
            {
                allEnemysDead -= value;
                allEnemysDead += value;
            }
            remove
            {
                allEnemysDead -= value;
            }
        }
        private event System.Action allEnemysDead;
        #endregion

        #region Propertys
        public Enemy EnemyOne
        {
            get
            {
                return enemyOne;
            }
        }
        public Enemy EnemyTwo
        {
            get
            {
                return enemyTwo;
            }
        }
        public Enemy EnemyThree
        {
            get
            {
                return enemyThree;
            }
        }
        public Enemy EnemyFour
        {
            get
            {
                return enemyFour;
            }
        }
        #endregion
        public void InitEnemyGroup(EnemyGroupData _enemyGroupData, Transform[] _spawnPoints)
        {
            enemyGroupData = _enemyGroupData;

            if (enemyGroupData.EnemyOne != null)
            {
                enemyOne = Instantiate(m_enemyPrefab);
                enemyOne.transform.parent = _spawnPoints[0];
                enemyOne.transform.localPosition = Vector3.zero;
                enemyOne.transform.localEulerAngles = Vector3.zero;
                enemyOne.transform.localScale = Vector3.one;
                enemyOne.InitEnemy(enemyGroupData.EnemyOne);
            }

            if (enemyGroupData.EnemyTwo != null)
            {
                enemyTwo = Instantiate(m_enemyPrefab);
                enemyTwo.transform.parent = _spawnPoints[1];
                enemyTwo.transform.localPosition = Vector3.zero;
                enemyTwo.transform.localEulerAngles = Vector3.zero;
                enemyTwo.transform.localScale = Vector3.one;
                enemyTwo.InitEnemy(enemyGroupData.EnemyTwo);
            }

            if (enemyGroupData.EnemyThree != null)
            {
                enemyThree = Instantiate(m_enemyPrefab);
                enemyThree.transform.parent = _spawnPoints[2];
                enemyThree.transform.localPosition = Vector3.zero;
                enemyThree.transform.localEulerAngles = Vector3.zero;
                enemyThree.transform.localScale = Vector3.one;
                enemyThree.InitEnemy(enemyGroupData.EnemyThree);
            }

            if (enemyGroupData.EnemyFour != null)
            {
                enemyFour = Instantiate(m_enemyPrefab);
                enemyFour.transform.parent = _spawnPoints[3];
                enemyFour.transform.localPosition = Vector3.zero;
                enemyFour.transform.localEulerAngles = Vector3.zero;
                enemyFour.transform.localScale = Vector3.one;
                enemyFour.InitEnemy(enemyGroupData.EnemyFour);
            }
        }

        public void Action()
        {
            if (EnemyOne == null && EnemyTwo == null && EnemyThree == null && EnemyFour == null)
            {
                allEnemysDead?.Invoke();
            }
            else
            {
                if (EnemyOne != null)
                {
                    EnemyOne.Attack();

                    if (EnemyTwo != null)
                    {
                        EnemyTwo.Attack();

                        if (EnemyThree != null)
                        {
                            EnemyThree.Attack();

                            if (EnemyFour != null)
                            {
                                EnemyFour.Attack();
                            }
                        }
                    }
                }
                enemyGroupAttackEnd?.Invoke();
            }
        }
    }
}