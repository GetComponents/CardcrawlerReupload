using Environment.Generation;
using UnityEngine;

namespace Environment
{
    public interface ITile
    {
        public Node Node { get; set; }
        public int Section { get; set; }
        public GameObject Visuals { get; }
        public event System.Action OnTileFinishedExecution;

        public void Highlight();
        public void Unhighlight();
        public void Activate();
        public void Complete();
        public void ChangeCollisionEnabled(bool _enabled);
    }
}
