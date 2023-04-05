
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Environment.Generation
{
    public class MapCreator : MonoBehaviour
    {
        public static MapCreator Instance { get; private set; }
        
        public event System.Action OnGenerationFinished
        {
            add
            {
                m_onGenerationFinished -= value;
                m_onGenerationFinished += value;
            }
            remove
            {
                m_onGenerationFinished -= value;
            }
        }

        [SerializeField]
        private GameObject m_mapBoundsPrefab;
        [SerializeField]
        private Bubble m_bubblePrefab;
        [SerializeField]
        private GenerationData m_generationData;

        private List<Node> m_nodes = new List<Node>();
        private List<List<Node>> m_paths = new List<List<Node>>();

        private event System.Action m_onGenerationFinished;

        /// <summary>
        /// Cache this value!
        /// </summary>
        /// <returns></returns>
        public List<Node> GetStartNodes()
        {
            if (m_nodes is null)
                return null;

            return m_nodes.Where(o => o.IsStart).ToList();
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void Start()
        {
            StartCoroutine(SimulateSpheres());
        }

        private IEnumerator SimulateSpheres()
        {
            GameObject mapBounds = Instantiate(m_mapBoundsPrefab);
            mapBounds.transform.localScale = new Vector3(m_generationData.MapSize.x,
                                                         20,
                                                         m_generationData.MapSize.y);
            mapBounds.transform.position = Vector3.forward * m_generationData.MapSize.y * 0.5f;
            Bubble tempBubble;
            List<Bubble> allBubbles = new List<Bubble>();

            for (int i = 0; i < m_generationData.PossibleNodeAmount; i++)
            {
                tempBubble = Instantiate(m_bubblePrefab);
                tempBubble.transform.position = Vector3.forward * m_generationData.MapSize.y * 0.5f;
                tempBubble.SetRadius(Random.Range(m_generationData.NodeSize.x, m_generationData.NodeSize.y));
                tempBubble.StartMovement(Vector3.Scale(Random.onUnitSphere, new Vector3(1, 0, 1)));
                tempBubble.MaxNeighbourDistance = m_generationData.NeighbourDistance;
                tempBubble.MapSize = m_generationData.MapSize;
                allBubbles.Add(tempBubble);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(5);

            foreach (Bubble bubble in allBubbles)
            {
                bubble.StopMovement();
                bubble.CreateNode();
            }

            foreach (Bubble bubble in allBubbles)
            {
                bubble.GatherNeighbours();
            }

            m_nodes = allBubbles.Select(o => o.Node).ToList();

            foreach (Bubble bubble in allBubbles)
            {
                Destroy(bubble.gameObject);
            }
            Destroy(mapBounds);

            for (int i = m_nodes.Count - 1; i >= 0; i--)
            {
                if (m_nodes[i].Neighbours.Count == 0)
                {
                    m_nodes.RemoveAt(i);
                }
            }
            RemoveNodesWithOnlyEnds();
            RemoveNodesWithOnlyStarts();
            CreatePaths();
            if (m_paths.Count == 0)
            {
                SceneManager.LoadScene("MainMenue");
                yield break;
            }
            RemoveNodesNotOnPath();
            DisplayTiles();
            DisplayPaths();

            m_onGenerationFinished?.Invoke();
        }

        private void CreatePaths()
        {
            List<Node> availableEndNodes = m_nodes.Where(o => o.IsEnd).ToList();
            List<Node> availableStartNodes = m_nodes.Where(o => o.IsStart).ToList();

            int startIndex;
            int endIndex;
            List<Node> tmpPath;
            int tries = 0;
            while (availableStartNodes.Count > 0 && availableEndNodes.Count > 0)
            {
                startIndex = Random.Range(0, availableStartNodes.Count);
                endIndex = Random.Range(0, availableEndNodes.Count);

                tmpPath = GetPath(availableStartNodes[startIndex], availableEndNodes[endIndex]);
                if (tmpPath is null || tmpPath.Count == 0)
                {
                    tries++;
                    if (tries == 3)
                    {
                        availableStartNodes.RemoveAt(startIndex);
                        availableEndNodes.RemoveAt(endIndex);
                        tries = 0;
                    }
                }
                else
                {
                    m_paths.Add(tmpPath);
                    availableStartNodes.RemoveAt(startIndex);
                    availableEndNodes.RemoveAt(endIndex);
                    tries = 0;
                }
            }
        }

        private void RemoveNodesNotOnPath()
        {
            List<Node> nodesOnPath = new List<Node>();
            foreach (List<Node> path in m_paths)
            {
                foreach (Node node in path)
                {
                    if (!nodesOnPath.Contains(node))
                    {
                        nodesOnPath.Add(node);
                    }
                }
            }

            for (int i = m_nodes.Count - 1; i >= 0; i--)
            {
                if (!nodesOnPath.Contains(m_nodes[i]))
                {
                    m_nodes.RemoveAt(i);
                }
            }

            foreach (Node node in m_nodes)
            {
                for (int i = node.Neighbours.Count - 1; i >= 0; i--)
                {
                    if (!nodesOnPath.Contains(node.Neighbours[i]))
                    {
                        node.Neighbours.RemoveAt(i);
                    }
                }
            }
        }

        private void RemoveNodesWithOnlyEnds()
        {
            List<Node> toRemove = new List<Node>();
            bool canRemove;
            foreach (Node node in m_nodes)
            {
                canRemove = true;
                foreach (Node neighbour in node.Neighbours)
                {
                    if (!neighbour.IsEnd)
                    {
                        canRemove = false;
                        break;
                    }
                }
                if (canRemove)
                {
                    toRemove.Add(node);
                }
            }

            foreach (Node node in m_nodes)
            {
                for (int i = node.Neighbours.Count - 1; i >= 0; i--)
                {
                    if (toRemove.Contains(node.Neighbours[i]))
                    {
                        node.Neighbours.RemoveAt(i);
                    }
                }
            }

            foreach (Node node in toRemove)
            {
                m_nodes.Remove(node);
            }
        }

        private void RemoveNodesWithOnlyStarts()
        {
            List<Node> toRemove = new List<Node>();
            bool canRemove;
            foreach (Node node in m_nodes)
            {
                canRemove = true;
                foreach (Node neighbour in node.Neighbours)
                {
                    if (!neighbour.IsStart)
                    {
                        canRemove = false;
                        break;
                    }
                }
                if (canRemove)
                {
                    toRemove.Add(node);
                }
            }

            foreach (Node node in m_nodes)
            {
                for (int i = node.Neighbours.Count - 1; i >= 0; i--)
                {
                    if (toRemove.Contains(node.Neighbours[i]))
                    {
                        node.Neighbours.RemoveAt(i);
                    }
                }
            }

            foreach (Node node in toRemove)
            {
                m_nodes.Remove(node);
            }
        }

        private void DisplayTiles()
        {
            ITile tile;
            GameObject go;
            List<GameObject> availableTiles = new List<GameObject>();

            foreach (TileChance tileChance in m_generationData.AvailableTiles)
            {
                for (int i = 0; i < tileChance.Weigth; i++)
                {
                    availableTiles.Add(tileChance.Tile);
                }
            }

            foreach (Node node in m_nodes)
            {
                go = Instantiate(availableTiles.Random(), transform);
                tile = go.GetComponent<ITile>();
                if (tile is null)
                {
                    Debug.LogError($"The tile {go} does not implement ITile!", 
                        m_generationData.AvailableTiles[0].Tile);
                }
                else
                {
                    tile.Node = node;
                    node.Visuals = tile;
                    go.transform.position = node.Position;
                    tile.Section = Mathf.FloorToInt(node.Position.z / (m_generationData.MapSize.y / 3.0f));
                    tile.ChangeCollisionEnabled(false);
                }
            }
        }

        private void DisplayPaths()
        {
            Path tmp;
            List<Node> closedList = new List<Node>();
            foreach (Node node in m_nodes)
            {
                foreach (Node neighbour in node.Neighbours)
                {
                    if (closedList.Contains(neighbour))
                        continue;
                    tmp = Instantiate(m_generationData.PathVisuals, transform);
                    tmp.Setup(node.Position, neighbour.Position);
                }
                closedList.Add(node);
            }
        }

        private List<Node> GetPath(Node _start, Node _end)
        {
            foreach (Node node in m_nodes)
            {
                node.Reset();
            }

            _start.CurrentPathLength = 0;
            List<Node> openList = new List<Node> { _start };
            List<Node> closedList = new List<Node>();
            Node tmp;

            while (openList.Count > 0)
            {
                openList = openList.OrderBy(o => o.CurrentPathLength + o.DistanceToGoal).ToList();
                tmp = openList[0];

                if (tmp == _end)
                {
                    List<Node> path = new List<Node>();
                    while (tmp.Previous != null)
                    {
                        path.Add(tmp);
                        tmp = tmp.Previous;
                    }
                    path.Add(tmp);
                    path.Reverse();
                    return path;
                }

                foreach (Node neighbour in tmp.Neighbours)
                {
                    if (closedList.Contains(neighbour))
                        continue;
                    float distance = Vector3.Distance(tmp.Position, neighbour.Position);
                    if (tmp.CurrentPathLength + distance < neighbour.CurrentPathLength)
                    {
                        neighbour.CurrentPathLength = tmp.CurrentPathLength + distance;
                        neighbour.Previous = tmp;

                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                            neighbour.DistanceToGoal =
                                Vector3.Distance(neighbour.Position, _end.Position);
                        }
                    }
                }
                closedList.Add(tmp);
                openList.Remove(tmp);
            }

            return null;
        }

        private void OnDrawGizmos()
        {
            return;
            if (m_nodes is null)
                return;
            //if (m_paths is null)
            //    return;

            //int index = -1;
            //foreach (List<Node> path in m_paths)
            //{
            //    index++;
            //    switch (index)
            //    {
            //        case 0:
            //            Gizmos.color = Color.green;
            //            break;
            //        case 1:
            //            Gizmos.color = Color.blue;
            //            break;
            //        case 2:
            //            Gizmos.color = Color.magenta;
            //            break;
            //        case 3:
            //            Gizmos.color = Color.yellow;
            //            break;
            //        case 4:
            //            Gizmos.color = Color.red;
            //            break;
            //        case 5:
            //            Gizmos.color = Color.cyan;
            //            break;
            //        case 6:
            //            Gizmos.color = Color.white;
            //            break;
            //        case 7:
            //            Gizmos.color = Color.grey;
            //            break;
            //    }
            //    for (int i = 0; i < path.Count - 1; i++)
            //    {
            //        Gizmos.DrawLine(path[i].Position, path[i + 1].Position);
            //    }
            //}

            foreach (Node node in m_nodes)
            {
                Gizmos.color = Color.white;
                if (node.IsEnd && node.IsStart)
                {
                    Gizmos.color = Color.magenta;
                }
                else if (node.IsStart)
                {
                    Gizmos.color = Color.green;
                }
                else if (node.IsEnd)
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawWireSphere(node.Position, 1.0f);

                foreach (Node neighbour in node.Neighbours)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(node.Position, neighbour.Position);
                }
            }
        }
    }
}