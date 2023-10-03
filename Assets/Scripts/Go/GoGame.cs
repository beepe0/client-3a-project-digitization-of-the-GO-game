
using UnityEngine;

namespace Go
{
    [RequireComponent(typeof(GoSettings))]
    [RequireComponent(typeof(GoRules))]
    [RequireComponent(typeof(GoBoard))]
    public class GoGame : MonoBehaviour
    {
        [SerializeField] private GoSettings goSettings;
        [SerializeField] private GoRules goRules;
        [SerializeField] private GoBoard goBoard;

        private void Awake()
        {
            goSettings = gameObject.GetComponent<GoSettings>();
            goRules = gameObject.GetComponent<GoRules>();
            goBoard = gameObject.GetComponent<GoBoard>();
        }

        private void Start()
        {
            goRules.GameInitialization(this);
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

        public GoSettings Settings { get => goSettings; }
        public GoRules Rules { get => goRules; }
        public GoBoard Board { get => goBoard; }
    }
}
