using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace Enemies
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private EnemyData enemyData;

        Camera mainCam;

        [SerializeField]
        TextMeshProUGUI text;

        #region Variablen
        [SerializeField]
        private int m_hp = 10;
        [Space(), Header("Attack One Data")]
        [SerializeField, Tooltip("Kann Negativ wie Positiv sein")]
        private int attackDamageOneModifire = 1;
        [SerializeField]
        private int currAttackDamageOne = 0;
        [Space(), Header("Attack Two Data")]
        [SerializeField, Tooltip("Kann Negativ wie Positiv sein")]
        private int attackDamageTwoModifire = 0;
        [SerializeField]
        private int currAttackDamageTwo = 0;
        [Header("Attack Three Data")]
        [SerializeField, Tooltip("Kann Negativ wie Positiv sein")]
        private int attackDamageThreeModifire = 0;
        [SerializeField]
        private int currAttackDamageThree = 0;

        public GameObject WeakUI, StrongUI;
        private int m_weakAmount, m_strongAmount;
        public int weakAmount
        {
            get => m_weakAmount;
            set
            {
                m_weakAmount = value;
                //if (value > 0)
                //{
                //    WeakUI.SetActive(true);
                //    WeakUI.GetComponentInChildren<TextMeshProUGUI>().text = $"Weak for {value} round(s)";
                //}
                //else
                //{
                //    WeakUI.SetActive(false);
                //}
            }
        }
        public int strongAmount
        {
            get => m_strongAmount;
            set
            {
                //m_strongAmount = value;
                //if (value > 0)
                //{
                //    StrongUI.SetActive(true);
                //    StrongUI.GetComponentInChildren<TextMeshProUGUI>().text = $"Strong for {value} round(s)";
                //}
                //else
                //{
                //    StrongUI.SetActive(false);
                //}
            }
        }

        private EnemyAttackType lastAttack = EnemyAttackType.NONE;
        #endregion

        #region Propertys
        public string Name { get { return enemyData.Name; } }
        public string Description { get { return enemyData.Description; } }
        public int HP
        {
            get => m_hp;
            set
            {
                //Um Overheal zu verhindern
                m_hp = (value > enemyData.HitPoints) ? enemyData.HitPoints : value;
                if (m_hp <= 0)
                    Destroy(this.gameObject);
                text.text = m_hp.ToString();
            }
        }

        public int CurrAttackDamageOne
        {
            get
            {
                currAttackDamageOne = enemyData.AttackDamageOne * attackDamageOneModifire;
                return (currAttackDamageOne < 0) ? 0 : currAttackDamageOne;
            }
        }

        public int SetAttackDamageOneModifire
        {
            set
            {
                attackDamageOneModifire = value;
                currAttackDamageOne = CurrAttackDamageOne;
            }
        }

        public int CurrAttackDamageTwo
        {
            get
            {
                currAttackDamageTwo = enemyData.AttackDamageTwo + attackDamageTwoModifire;
                return (currAttackDamageTwo < 0) ? 0 : currAttackDamageTwo;
            }
        }

        public int SetAttackDamageTwoModifire
        {
            set
            {
                attackDamageTwoModifire = value;
                currAttackDamageTwo = CurrAttackDamageTwo;
            }
        }

        public int CurrAttackDamageThree
        {
            get
            {
                currAttackDamageThree = enemyData.AttackDamageThree + attackDamageThreeModifire;
                return (currAttackDamageThree < 0) ? 0 : currAttackDamageThree;
            }
        }

        public int SetAttackDamageThreeModifire
        {
            set
            {
                attackDamageThreeModifire = value;
                currAttackDamageThree = CurrAttackDamageThree;
            }
        }
        #endregion

        private void Start()
        {
            text.text = HP.ToString();
            mainCam = Camera.main;
        }

        public void InitEnemy(EnemyData _enemyData)
        {
            enemyData = _enemyData;

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = enemyData.Modell;

            HP = enemyData.HitPoints;
        }

        void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        if (SelectionManager.Instance.waitingForEnemySelection)
                        {
                            Debug.Log("An enemy was selected");
                            SelectionManager.Instance.SelectEnemy(this);
                        }
                    }
                }
            }
        }

        public void ReduceBuffs()
        {
            if (weakAmount > 0)
            {
                weakAmount--;
            }
            if (strongAmount > 0)
            {
                strongAmount--;
            }
        }

        public void Attack()
        {
            switch (RandomAttack(lastAttack))
            {
                case EnemyAttackType.LIGHT:
                    lastAttack = EnemyAttackType.LIGHT;
                    AttackOne();
                    break;
                case EnemyAttackType.NORMAL:
                    lastAttack = EnemyAttackType.NORMAL;
                    AttackTwo();
                    break;
                case EnemyAttackType.HARD:
                    lastAttack = EnemyAttackType.HARD;
                    AttackThree();
                    break;
            }
        }

        public EnemyAttackType RandomAttack(EnemyAttackType _last)
        {
            EnemyAttackType currentType = (EnemyAttackType)Random.Range(1, 3);
            return (currentType == _last) ? RandomAttack(_last) : currentType;
        }

        private void AttackOne()
        {
            float multiplier = 1;
            if (weakAmount > 0)
            {
                multiplier *= 0.66f;
            }
            if (strongAmount > 0)
            {
                multiplier *= 1.5f;
            }
            foreach (Enemy _enemy in FindObjectsOfType<Enemy>())
            {
                Player.Player.Instance.SetDamage((int)Mathf.Ceil(CurrAttackDamageOne * multiplier));
            }
        }

        private void AttackTwo()
        {
            float multiplier = 1;
            if (weakAmount > 0)
            {
                multiplier *= 0.66f;
            }
            if (strongAmount > 0)
            {
                multiplier *= 1.5f;
            }
            foreach (Enemy _enemy in FindObjectsOfType<Enemy>())
            {
                Player.Player.Instance.SetDamage((int)Mathf.Ceil(CurrAttackDamageTwo * multiplier));
            }
        }

        private void AttackThree()
        {
            float multiplier = 1;
            if (weakAmount > 0)
            {
                multiplier *= 0.66f;
            }
            if (strongAmount > 0)
            {
                multiplier *= 1.5f;
            }
            foreach (Enemy _enemy in FindObjectsOfType<Enemy>())
            {
                Player.Player.Instance.SetDamage((int)Mathf.Ceil(CurrAttackDamageThree * multiplier));
            }
        }
    }

    public enum EnemyAttackType
    {
        NONE = 0,
        LIGHT = 1,
        NORMAL = 2,
        HARD = 3,
    }
}