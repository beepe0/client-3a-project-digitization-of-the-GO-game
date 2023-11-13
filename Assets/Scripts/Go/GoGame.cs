using Network.Connection;
using Network.UnityTools;
using UnityEngine;

namespace Go
{
    [RequireComponent(typeof(GoSettings))]
    [RequireComponent(typeof(GoBoard))]
    public class GoGame : MonoBehaviour
    {
        [SerializeField] private Connection conn;

        [SerializeField] private GoSettings goSettings;
        [SerializeField] private GoBoard goBoard;
        
        public Connection Conn => conn;
        public GoSettings Settings => goSettings;
        public GoBoard Board => goBoard;

        private void Awake()
        {
            conn = GameObject.FindWithTag("Network").GetComponent<Connection>();
            goSettings = gameObject.GetComponent<GoSettings>();
            goBoard = gameObject.GetComponent<GoBoard>();
        }
        public void InitializingGame()
        {
            gameObject.transform.localScale = new Vector3((Settings.boardSize.x - 1) / Settings.cellsSize, 1, (Settings.boardSize.y - 1) / Settings.cellsSize);
            Settings.boardMaterial.mainTextureScale = new Vector2((Settings.boardSize.x - 1), (Settings.boardSize.y - 1));
            Settings.pawnsSize = (20 / Settings.cellsSize) / Settings.cellsCoefSize;
            
            Board.pawnCursor = Instantiate(Settings.prefabPawnCursor, gameObject.transform);
            Board.offset = new Vector2(gameObject.transform.localScale.x / 2, -gameObject.transform.localScale.z / 2);
            Board.pawnOffset = new Vector2(Board.offset.x, -Board.offset.y);
            Board.pawns = new GoPawn[Settings.boardSize.x * Settings.boardSize.y];

            for (int x = 0; x < Settings.boardSize.x; x++)
            {
                for (int y = 0; y > -Settings.boardSize.y; y--)
                {
                    int convertMatrixToLine = GoTools.ConvertMatrixToLine(Settings.boardSize, new Vector2(x, y));
                    
                    Vector3 newPos = new Vector3(x / Settings.cellsSize - (Board.pawnOffset.x), 0.5f, y / Settings.cellsSize + (Board.pawnOffset.y));
                    GameObject pawnObject = Instantiate(Settings.prefabPawnAB, newPos, Quaternion.identity, gameObject.transform);
                    GoPawn node = new GoPawn(this, convertMatrixToLine, pawnObject);

                    pawnObject.SetActive(false);
                    pawnObject.name = $"xyz: {newPos}";
                    pawnObject.transform.SetParent(gameObject.transform);
                    
                    Board.pawns[convertMatrixToLine] = node;
                }
            }
        }
        public void InitializingPawn(Vector2 xy)
        {
            UNetworkIOPacket packet = new UNetworkIOPacket((ushort)Connection.PacketType.PawnOpen);
            packet.Write(xy.x);
            packet.Write(xy.y);
            Conn.DataHandler.SendDataTcp(packet);
        }
        public void PawnPass()
        {
            UNetworkIOPacket packet = new UNetworkIOPacket((ushort)Connection.PacketType.PawnPass);

            Conn.DataHandler.SendDataTcp(packet);
        }
        public void ShowCursor(Vector2 xy)
        {
            short convertMatrixToLine = GoTools.ConvertRayToLine(xy, goBoard.offset, goSettings.boardSize, goSettings.cellsSize);

            if (convertMatrixToLine >= 0 && convertMatrixToLine < goBoard.pawns.Length && !goBoard.pawns[convertMatrixToLine].isClosed)
            {
                goBoard.pawnCursor.SetActive(true);
                goBoard.pawnCursor.transform.position = goBoard.pawns[convertMatrixToLine].pawnPosition;
                goBoard.pawnCursor.transform.localScale = new Vector3(goSettings.pawnsSize, 0.5f, goSettings.pawnsSize);
            }
            else
            {
                goBoard.pawnCursor.SetActive(false);
            }
        }
    }
}
