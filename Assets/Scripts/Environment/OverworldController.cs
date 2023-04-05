using Environment.Generation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Environment
{
    public delegate void OnCurrentTileChangedDelegate(ITile _previous, ITile _current);

    [DisallowMultipleComponent]
    public class OverworldController : MonoBehaviour
    {
        public static OverworldController Instance { get; private set; }

        public EOverworldStates CurrentStatus
        {
            get => m_currentStatus;
            set
            {
                if (m_currentStatus == value)
                    return;
                m_currentStatus = value;
                m_onStateChanged?.Invoke(m_currentStatus);
            }
        }
        public ITile CurrentTile
        {
            get => m_currentTile;
            set
            {
                if (m_currentTile == value)
                    return;
                m_onCurrentTileChanged?.Invoke(m_currentTile, value);
                m_currentTile = value;
            }
        }
        public event System.Action<EOverworldStates> OnStateChanged
        {
            add
            {
                m_onStateChanged -= value;
                m_onStateChanged += value;
            }
            remove
            {
                m_onStateChanged -= value;
            }
        }
        public event OnCurrentTileChangedDelegate OnCurrentTileChanged
        {
            add
            {
                m_onCurrentTileChanged -= value;
                m_onCurrentTileChanged += value;
            }
            remove
            {
                m_onCurrentTileChanged -= value;
            }
        }

        [SerializeField]
        private GameObject m_playerPrefab;
        [SerializeField]
        private Cinemachine.CinemachineVirtualCamera m_followCamera;

        private EOverworldStates m_currentStatus;
        private ITile m_currentTile;
        private List<ITile> m_highlightedTiles = new List<ITile>();
        private Camera m_camera;

        private GameObject m_playerObject;
        private Vector3 m_playerPosition;
        private float m_lerpTime;

        private event System.Action<EOverworldStates> m_onStateChanged;
        private event OnCurrentTileChangedDelegate m_onCurrentTileChanged;

        public void OnSelectTile(InputAction.CallbackContext _context)
        {
            if (_context.started)
            {
                if (CurrentStatus == EOverworldStates.CHOOSE_START_TILE
                    || CurrentStatus == EOverworldStates.CHOOSE_NEXT_TILE)
                {
                    if (RaycastForTile(out ITile tile))
                    {
                        CurrentTile = tile;
                    }
                }
            }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            m_camera = Camera.main;

            OnStateChanged += HighlightTiles;
            OnCurrentTileChanged += PlayerChoseTile;
        }

        private void Start()
        {
            MapCreator.Instance.OnGenerationFinished += SetInitialState;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void Update()
        {
            switch (CurrentStatus)
            {
                case EOverworldStates.MOVE:
                    {
                        if (m_lerpTime > 1.0f)
                        {
                            m_playerObject.transform.position = CurrentTile.Node.Position;
                            m_playerPosition = CurrentTile.Node.Position;
                            m_lerpTime = 0.0f;
                            CurrentStatus = EOverworldStates.ACTIVATE;
                        }
                        else
                        {
                            m_lerpTime += Time.deltaTime;
                            m_playerObject.transform.position =
                                Vector3.Lerp(m_playerPosition, CurrentTile.Node.Position, m_lerpTime);
                        }
                        break;
                    }
                case EOverworldStates.ACTIVATE:
                    {
                        CurrentTile.OnTileFinishedExecution += WaitForTileActivationFinish;
                        CurrentTile.Activate();
                        CurrentStatus = EOverworldStates.WAIT_FOR_RETURN;
                        break;
                    }
                case EOverworldStates.WAIT_FOR_RETURN:
                    {
                        break;
                    }
            }
        }

        private void WaitForTileActivationFinish()
        {
            CurrentTile.Unhighlight();
            CurrentTile.ChangeCollisionEnabled(false);

            if (CurrentTile.Node.IsEnd)
            {
                CurrentStatus = EOverworldStates.REACHED_END;
            }
            else
            {
                CurrentStatus = EOverworldStates.CHOOSE_NEXT_TILE;
            }
        }

        private void SetInitialState()
        {
            CurrentStatus = EOverworldStates.CHOOSE_START_TILE;
        }

        private void HighlightTiles(EOverworldStates _status)
        {
            if (_status != EOverworldStates.CHOOSE_START_TILE
                && _status != EOverworldStates.CHOOSE_NEXT_TILE)
                return;

            if (_status == EOverworldStates.CHOOSE_START_TILE)
            {
                List<Node> startNodes = MapCreator.Instance.GetStartNodes();
                foreach (Node node in startNodes)
                {
                    node.Visuals.Highlight();
                    m_highlightedTiles.Add(node.Visuals);
                }
            }
            else
            {
                // only upper neighbours are valid
                foreach (Node neighbour in CurrentTile.Node.Neighbours)
                {
                    if (IsUpperNeighbour(CurrentTile, neighbour.Visuals))
                    {
                        neighbour.Visuals.Highlight();
                        m_highlightedTiles.Add(neighbour.Visuals);
                    }
                }
            }

            foreach (ITile tile in m_highlightedTiles)
            {
                tile.ChangeCollisionEnabled(true);
            }
        }

        private bool IsUpperNeighbour(ITile _tile, ITile _neighbour)
        {
            if (_tile is null || _neighbour is null)
                return false;
            Vector3 direction = _neighbour.Node.Position - _tile.Node.Position;
            return Vector3.Dot(direction, Vector3.forward) > 0;
        }

        private void PlayerChoseTile(ITile _previous, ITile _new)
        {
            if (CurrentStatus != EOverworldStates.CHOOSE_START_TILE
                && CurrentStatus != EOverworldStates.CHOOSE_NEXT_TILE)
                return;

            if (CurrentStatus == EOverworldStates.CHOOSE_START_TILE)
            {
                SpawnPlayer(_new);
                CurrentStatus = EOverworldStates.ACTIVATE;
            }
            else
            {
                CurrentStatus = EOverworldStates.MOVE;
            }

            foreach (ITile tile in m_highlightedTiles)
            {
                if (tile == _new)
                    continue;
                tile.Unhighlight();
                tile.ChangeCollisionEnabled(false);
            }
            m_highlightedTiles.Clear();
        }

        private bool RaycastForTile(out ITile _tile)
        {
            Ray ray = m_camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            _tile = null;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _tile = hit.collider.GetComponent<ITile>();
                return _tile is object;
            }
            return false;
        }

        private void SpawnPlayer(ITile _tile)
        {
            m_playerObject = Instantiate(m_playerPrefab, _tile.Node.Position, Quaternion.identity);
            m_playerPosition = _tile.Node.Position;

            m_followCamera.Follow = m_playerObject.transform;
        }
    }
}