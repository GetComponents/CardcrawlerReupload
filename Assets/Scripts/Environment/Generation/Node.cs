using System.Collections.Generic;
using UnityEngine;

namespace Environment.Generation
{
    public class Node
    {
        public Vector3 Position { get; }
        public List<Node> Neighbours { get; } = new List<Node>();
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }

        public float CurrentPathLength { get; set; }
        public float DistanceToGoal { get; set; }
        public Node Previous { get; set; }

        public ITile Visuals { get; set; }

        public Node(Vector3 _position)
        {
            Position = _position;
        }
        public void Reset()
        {
            CurrentPathLength = float.PositiveInfinity;
            Previous = null;
        }
    }
}
