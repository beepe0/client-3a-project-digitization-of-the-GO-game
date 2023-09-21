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
        
        private Vector3 _offset;
        private Vector3 _offsetHitPoint;
        private Vector2 _convertHitPoint;
        private int _convertMatrixToLine;
        
        private void Awake()
        {
            _goSettings = gameObject.GetComponent<GoSettings>();
        }

        private void Start()
        {
            _goSettings.prefabBoard.transform.localScale = new Vector3(_goSettings.boardSize.x / _goSettings.cellsSize, 1, _goSettings.boardSize.y / _goSettings.cellsSize);
        
            _board = new Node[_goSettings.boardSize.x * _goSettings.boardSize.y];
            _offset = new Vector3(gameObject.transform.localScale.x / 2, 0, -gameObject.transform.localScale.z / 2);
        }

        private void Update()
        {
            RaycastingMouse();
        }

        private void RaycastingMouse()
        {
            if (!Input.GetKeyDown(KeyCode.Mouse0)) return;
        
            Vector3 mousePosition = Input.mousePosition;
            Ray r = LocalPlayer.Instance.localPlayerCamera.ScreenPointToRay(mousePosition);
        
            RaycastHit hit;
            
            if (Physics.Raycast(r, out hit, 100))
            {
                _offsetHitPoint = (hit.point + _offset) * _goSettings.cellsSize;
                _convertHitPoint = new Vector2(Mathf.Floor(_offsetHitPoint.x), Mathf.Ceil(_offsetHitPoint.z));
                _convertMatrixToLine =  (int)((_goSettings.boardSize.x * Mathf.Abs(_convertHitPoint.y)) + (_convertHitPoint.x));

                _board[_convertMatrixToLine] = new Node(this, (NodeType)Random.Range(0,2), _convertMatrixToLine, _convertHitPoint, hit.point);

                Debug.Log($"HIT: {hit.point}, OHIT: {_offsetHitPoint}, CHIT: {_convertHitPoint}, CMTL: {_convertMatrixToLine}");
                Debug.DrawLine(r.origin, hit.point, Color.red, 10);
            }
        }

        public void Initialization(Node n)
        {
            n.pawn = Instantiate(n.pawnType == NodeType.PawnA ? _goSettings.prefabPawnA : _goSettings.prefabPawnB, n.realXY, Quaternion.identity);
        }
    }
}
