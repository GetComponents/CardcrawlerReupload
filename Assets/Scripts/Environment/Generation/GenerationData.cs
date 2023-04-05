using UnityEngine;

namespace Environment.Generation
{
    [CreateAssetMenu(fileName = "Empty generation data", menuName = "Data/GenerationData")]
    public class GenerationData : ScriptableObject
    {
        public TileChance[] AvailableTiles => m_availableTiles;
        public Path PathVisuals => m_pathVisuals;
        public Vector2Int MapSize => m_mapSize;
        public Vector2Int NodeSize => m_nodeSize;
        public int PossibleNodeAmount => m_possibleNodeAmount;
        public int NeighbourDistance => m_neighbourDistance;

        [SerializeField]
        private TileChance[] m_availableTiles;
        [SerializeField]
        private Path m_pathVisuals;
        [SerializeField]
        private Vector2Int m_mapSize;
        [SerializeField]
        private Vector2Int m_nodeSize;
        [SerializeField]
        private int m_possibleNodeAmount;
        [SerializeField]
        private int m_neighbourDistance;
    }
}