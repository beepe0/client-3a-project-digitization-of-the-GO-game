using System;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Go
{
    [RequireComponent(typeof(GoSettings))]
    public class Go : MonoBehaviour
    {
        [SerializeField] private GoSettings _goSettings;
        [SerializeField] private Node[] _board;

        private GameObject _pawnCursor;
        
        private Vector3 _offset;
        private Vector3 _pawnOffset;
        
        private void Awake()
        {
            _goSettings = gameObject.GetComponent<GoSettings>();
        }

        private void Start()
        {
            BoardInitialization();
        }

        private void Update()
        {
            RaycastingMouse();
        }

        private void RaycastingMouse()
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray r = LocalPlayer.Instance.localPlayerCamera.ScreenPointToRay(mousePosition);
        
            RaycastHit hit;
            
            if (Physics.Raycast(r, out hit, 100))
            {
                Vector3 _offsetHitPoint = (hit.point + _offset) * _goSettings.cellsSize;
                Vector2 _convertHitPoint = new Vector2(Mathf.Floor(_offsetHitPoint.x), Mathf.Ceil(_offsetHitPoint.z));
       
                ushort _convertMatrixToLine = (ushort)((_goSettings.boardSize.x * Mathf.Abs(_convertHitPoint.y)) + (_convertHitPoint.x));
                
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if(_convertMatrixToLine < _board.Length) PawnInitialization(_board[_convertMatrixToLine]);
                
                    Debug.DrawLine(r.origin, hit.point, Color.red, 10);
                }
                else if (_convertMatrixToLine < _board.Length)
                {
                    _pawnCursor.SetActive(true);
                    _pawnCursor.transform.position = _board[_convertMatrixToLine].pawnPosition.transform.position;
                }
                else
                {
                    _pawnCursor.SetActive(false);
                }
                Debug.DrawLine(r.origin, hit.point, Color.blue, 0);
            }
            else
            {
                _pawnCursor.SetActive(false);
            }
        }
        
        private void BoardInitialization()
        {
            _goSettings.prefabBoard.transform.localScale = new Vector3((_goSettings.boardSize.x - 1) / _goSettings.cellsSize, 1, (_goSettings.boardSize.y - 1) / _goSettings.cellsSize);
            _goSettings.boardMaterial.mainTextureScale = new Vector2((_goSettings.boardSize.x - 1), (_goSettings.boardSize.y - 1));
            _goSettings.pawnsSize = ((_goSettings.boardSize.x - 1) / 10f) + 1;

            _pawnCursor = Instantiate(_goSettings.prefabPawnCursor, gameObject.transform);
            
            _offset = new Vector3(_goSettings.prefabBoard.transform.localScale.x / 2, 0, -_goSettings.prefabBoard.transform.localScale.z / 2);
            _pawnOffset = new Vector3(_offset.x, 0, -_offset.z);
            _board = new Node[_goSettings.boardSize.x * _goSettings.boardSize.y];

            for (int x = 0; x < _goSettings.boardSize.x; x++)
            {
                for (int y = 0; y > -_goSettings.boardSize.y; y--)
                {
                    int _convertMatrixToLine = (_goSettings.boardSize.x * Mathf.Abs(y)) + x;
                    Vector3 newPos = new Vector3(x / _goSettings.cellsSize - (_pawnOffset.x), 0.5f, y / _goSettings.cellsSize + (_pawnOffset.z));
                    GameObject t = Instantiate(_goSettings.prefabPositionAB, newPos, Quaternion.identity, gameObject.transform);
                    
                    t.name = $"xyz: {newPos}";
                    t.transform.position = newPos;
                    t.transform.SetParent(gameObject.transform);

                    _board[_convertMatrixToLine] = new Node(this, _convertMatrixToLine, t);
                }
            }
        }
        
        public void PawnInitialization(Node n)
        {
            if(n.isClosed) return;

            n.isClosed = true;
            n.pawnType = (NodeType)Random.Range(1, 3);
            n.pawn = Instantiate(n.pawnType == NodeType.PawnA ? _goSettings.prefabPawnA : _goSettings.prefabPawnB, n.pawnPosition.transform.position, Quaternion.identity, n.pawnPosition.transform);
            n.pawn.transform.localScale = new Vector3(_goSettings.pawnsSize, 0.5f, _goSettings.pawnsSize);
        }
    }
}
