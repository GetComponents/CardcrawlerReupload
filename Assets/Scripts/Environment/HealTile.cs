using Environment.Generation;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Environment
{
    public class HealTile : MonoBehaviour, ITile
    {
        public Node Node { get => m_node; set => m_node = value; }
        public int Section { get => m_section; set => m_section = value; }
        public int HealAmount => m_healAmount;

        public GameObject Visuals => gameObject;

        public event Action OnTileFinishedExecution;

        [SerializeField]
        private int m_healAmount = 2;
        [SerializeField]
        private ParticleSystem m_fireEffect;
        private Node m_node;
        private int m_section;
        private Collider m_collider;

        private void Awake()
        {
            m_collider = GetComponent<Collider>();
        }

        public void Activate()
        {
            OverworldUI.Instance.ShowHealTileUI(this);
        }

        public void ChangeCollisionEnabled(bool _enabled)
        {
            m_collider.enabled = _enabled;
        }

        public void Complete()
        {
            OverworldUI.Instance.HideHealTileUI();
            OnTileFinishedExecution?.Invoke();
        }

        public void Highlight()
        {
            m_fireEffect.Play();
        }

        public void Unhighlight()
        {
            m_fireEffect.Stop();
        }
    }
}