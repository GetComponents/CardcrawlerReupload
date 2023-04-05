using Environment.Generation;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Environment
{
    public class ShopTile : MonoBehaviour, ITile
    {
        public Node Node { get => m_node; set => m_node = value; }
        public int Section { get => m_section; set => m_section = value; }

        public GameObject Visuals => gameObject;

        public event Action OnTileFinishedExecution;

        [SerializeField]
        private Light[] m_lights;
        private int m_section;
        private Node m_node;
        private Collider m_collider;

        private void Awake()
        {
            m_collider = GetComponent<Collider>();
        }

        private void Start()
        {
            transform.Find("Root").localScale = Vector3.one * 0.3f;
        }

        public void Activate()
        {
            OverworldUI.Instance.ShowShopUI(this);
        }

        public void ChangeCollisionEnabled(bool _enabled)
        {
            m_collider.enabled = _enabled;
        }

        public void Complete()
        {
            OverworldUI.Instance.HideShopUI();
            OnTileFinishedExecution?.Invoke();
        }

        public void Highlight()
        {
            foreach (Light light in m_lights)
            {
                light.gameObject.SetActive(true);
            }
        }

        public void Unhighlight()
        {
            foreach (Light light in m_lights)
            {
                light.gameObject.SetActive(false);
            }
        }
    }
}
