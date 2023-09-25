using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Go
{
    [RequireComponent(typeof(GoSettings))]
    public class GoGame : MonoBehaviour
    {
        [SerializeField] private GoSettings goSettings;
        [SerializeField] private GoPawn[] board;

        private GameObject _pawnCursor;
        
        private Vector3 _offset;
        private Vector3 _pawnOffset;
        
        private void Awake()
        {
            goSettings = gameObject.GetComponent<GoSettings>();
        }

        private void Start()
        {
            BoardInitialization();
        }
        
        private void BoardInitialization()
        {
            goSettings.prefabBoard.transform.localScale = new Vector3((goSettings.boardSize.x - 1) / goSettings.cellsSize, 1, (goSettings.boardSize.y - 1) / goSettings.cellsSize);
            goSettings.boardMaterial.mainTextureScale = new Vector2((goSettings.boardSize.x - 1), (goSettings.boardSize.y - 1));
            goSettings.pawnsSize = (20 / goSettings.cellsSize) / goSettings.cellsCoefSize;

            _pawnCursor = Instantiate(goSettings.prefabPawnCursor, gameObject.transform);
            
            _offset = new Vector3(goSettings.prefabBoard.transform.localScale.x / 2, 0, -goSettings.prefabBoard.transform.localScale.z / 2);
            _pawnOffset = new Vector3(_offset.x, 0, -_offset.z);
            
            board = new GoPawn[goSettings.boardSize.x * goSettings.boardSize.y];

            for (int x = 0; x < goSettings.boardSize.x; x++)
            {
                for (int y = 0; y > -goSettings.boardSize.y; y--)
                {
                    int convertMatrixToLine = ConvertMatrixToLine(new Vector2(x, y));
                    
                    Vector3 newPos = new Vector3(x / goSettings.cellsSize - (_pawnOffset.x), 0.5f, y / goSettings.cellsSize + (_pawnOffset.z));
                    GameObject pawnObject = Instantiate(goSettings.prefabPawnAB, newPos, Quaternion.identity, gameObject.transform);
                    GoPawn node = new GoPawn(this, convertMatrixToLine, pawnObject);

                    pawnObject.SetActive(false);
                    pawnObject.name = $"xyz: {newPos}";
                    pawnObject.transform.SetParent(gameObject.transform);
                    
                    for(ushort i = 0; i < 4; i++)
                    {
                        short mtl = ConvertMatrixToLine(new Vector2(x + GoPawn.OffsetNeigh[i].x, y + GoPawn.OffsetNeigh[i].y));
                        node.Neighbours[i] = mtl >= 0 && mtl < board.Length ? board[mtl] : null;
                    }
                    
                    board[convertMatrixToLine] = node;
                }
            }
        }
        
        private short ConvertMatrixToLine(Vector2 xy) => (short)((goSettings.boardSize.x * (short)Mathf.Abs(xy.y)) + (short)(xy.x));
        private short ConvertRayToLine(Ray r, RaycastHit info)
        {
            Vector3 offsetHitPoint = (info.point + _offset) * goSettings.cellsSize;

            return ConvertMatrixToLine(new Vector2(Mathf.Floor(offsetHitPoint.x), Mathf.Ceil(offsetHitPoint.z)));
        }

        public void ShowCursor(Ray r, RaycastHit info)
        {
            short convertMatrixToLine = ConvertRayToLine(r, info);

            if (convertMatrixToLine < board.Length && !board[convertMatrixToLine].isClosed)
            {
                _pawnCursor.SetActive(true);
                _pawnCursor.transform.position = board[convertMatrixToLine].pawnPosition;
                _pawnCursor.transform.localScale = new Vector3(goSettings.pawnsSize, 0.5f, goSettings.pawnsSize);
            }
            else
            {
                _pawnCursor.SetActive(false);
            }
        }
        public void PawnInitialization(Ray r, RaycastHit info)
        {
            short convertMatrixToLine = ConvertRayToLine(r, info);
            GoPawn n = board[convertMatrixToLine];

            if(convertMatrixToLine > board.Length || n.isClosed) return;

            n.isClosed = true;
            n.pawnType = (NodeType)Random.Range(1, 3);
            n.pawnMeshRenderer.material = n.pawnType == NodeType.PawnA ? goSettings.materialPawnA : goSettings.materialPawnB;
            n.pawnObject.transform.localScale = new Vector3(goSettings.pawnsSize, 0.5f, goSettings.pawnsSize);
            n.pawnObject.SetActive(true);
        }
    }
}
