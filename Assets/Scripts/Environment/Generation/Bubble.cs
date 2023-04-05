using UnityEngine;

namespace Environment.Generation
{
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
    [DisallowMultipleComponent]
    public class Bubble : MonoBehaviour
    {
        public Node Node => m_node;
        public float MaxNeighbourDistance { get; set; }
        public Vector2Int MapSize { get; set; }

        private Rigidbody m_rigidbody;
        private SphereCollider m_circleCollider;
        private Node m_node;

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_rigidbody.useGravity = false;
            m_circleCollider = GetComponent<SphereCollider>();
        }

        public void StartMovement(Vector3 _impulse)
        {
            m_rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            m_rigidbody.AddForce(_impulse, ForceMode.Impulse);
        }

        public void StopMovement()
        {
            m_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        public void SetRadius(float _radius)
        {
            transform.localScale = Vector3.one * _radius;
        }

        public void CreateNode()
        {
            m_node = new Node(transform.position);
        }

        public void GatherNeighbours()
        {
            m_circleCollider.enabled = false;
            float raycastIntervalls = 90.0f;
            int steps = Mathf.FloorToInt(360.0f / raycastIntervalls);

            Vector3 castDirection;
            RaycastHit hit;
            Bubble other;
            bool hasUpperNeighbour = false;
            bool hasLowerNeighbour = false;
            for (int i = 0; i <= steps; i++)
            {
                castDirection = Matrix4x4.Rotate(Quaternion.Euler(0, i * raycastIntervalls, 0)) * Vector3.forward;
                
                if (Physics.Raycast(transform.position, castDirection, out hit, MaxNeighbourDistance))
                {
                    other = hit.collider.GetComponent<Bubble>();
                    if (other != null)
                    {
                        if (m_node.Neighbours.Contains(other.m_node))
                            continue;
                        m_node.Neighbours.Add(other.m_node);

                        if (Vector3.Dot(other.m_node.Position - m_node.Position, Vector3.forward) > 0.5)
                        {
                            hasUpperNeighbour = true;
                        }
                        else if (Vector3.Dot(other.m_node.Position - m_node.Position, Vector3.back) > 0.5)
                        {
                            hasLowerNeighbour = true;
                        }
                    }
                }
            }
            // node is in the upper 10%
            if (transform.position.z > MapSize.y * 0.7f)
            {
                m_node.IsEnd = !hasUpperNeighbour;
            }
            // node is in the lower 10%
            else if (transform.position.z < MapSize.y * 0.3f)
            {
                m_node.IsStart = !hasLowerNeighbour;
            }
            m_circleCollider.enabled = true;
        }
    }
}
