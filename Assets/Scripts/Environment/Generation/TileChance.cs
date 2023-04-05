using UnityEngine;

namespace Environment.Generation
{
    [System.Serializable]
    public struct TileChance
    {
        public GameObject Tile => m_tile;
        public float Weigth => m_weigth;

        [SerializeField]
        private GameObject m_tile;
        [SerializeField]
        private float m_weigth;
    }
}
