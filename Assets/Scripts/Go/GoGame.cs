using Network.Connection;
using UnityEngine;

namespace Go
{
    [RequireComponent(typeof(GoSettings))]
    [RequireComponent(typeof(GoRules))]
    [RequireComponent(typeof(GoBoard))]
    public class GoGame : MonoBehaviour
    {
        [SerializeField] private Connection conn;

        [SerializeField] private GoSettings goSettings;
        [SerializeField] private GoRules goRules;
        [SerializeField] private GoBoard goBoard;
        
        public Connection Conn => conn;
        public GoSettings Settings => goSettings;
        public GoRules Rules => goRules;
        public GoBoard Board => goBoard;

        private void Awake()
        {
            conn = GameObject.FindWithTag("Network").GetComponent<Connection>();
            goSettings = gameObject.GetComponent<GoSettings>();
            goRules = gameObject.GetComponent<GoRules>();
            goBoard = gameObject.GetComponent<GoBoard>();
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
