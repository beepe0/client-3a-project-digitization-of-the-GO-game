using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Go
{
    public class GoRules : MonoBehaviour
    {
        private GoSettings goSettings;
        private GoBoard goBoard;

        public void GameInitialization(GoGame goGame)
        {
            goSettings = goGame.Settings;
            goBoard = goGame.Board;
            
            goSettings.prefabBoard.transform.localScale = new Vector3((goSettings.boardSize.x - 1) / goSettings.cellsSize, 1, (goSettings.boardSize.y - 1) / goSettings.cellsSize);
            goSettings.boardMaterial.mainTextureScale = new Vector2((goSettings.boardSize.x - 1), (goSettings.boardSize.y - 1));
            goSettings.pawnsSize = (20 / goSettings.cellsSize) / goSettings.cellsCoefSize;
            
            goBoard.pawnCursor = Instantiate(goSettings.prefabPawnCursor, gameObject.transform);
            goBoard.offset = new Vector2(goSettings.prefabBoard.transform.localScale.x / 2, -goSettings.prefabBoard.transform.localScale.z / 2);
            goBoard.pawnOffset = new Vector2(goBoard.offset.x, -goBoard.offset.y);
            goBoard.pawns = new GoPawn[goSettings.boardSize.x * goSettings.boardSize.y];

            for (int x = 0; x < goSettings.boardSize.x; x++)
            {
                for (int y = 0; y > -goSettings.boardSize.y; y--)
                {
                    int convertMatrixToLine = GoTools.ConvertMatrixToLine(goSettings.boardSize, new Vector2(x, y));
                    
                    Vector3 newPos = new Vector3(x / goSettings.cellsSize - (goBoard.pawnOffset.x), 0.5f, y / goSettings.cellsSize + (goBoard.pawnOffset.y));
                    GameObject pawnObject = Instantiate(goSettings.prefabPawnAB, newPos, Quaternion.identity, gameObject.transform);
                    GoPawn node = new GoPawn(goGame, convertMatrixToLine, pawnObject);

                    pawnObject.SetActive(false);
                    pawnObject.name = $"xyz: {newPos}";
                    pawnObject.transform.SetParent(gameObject.transform);
                    
                    for(ushort i = 0; i < 4; i++)
                    {
                        short mtl = GoTools.ConvertMatrixToLine(goSettings.boardSize, new Vector2(x + GoPawn.OffsetNeighbours[i].x, y + GoPawn.OffsetNeighbours[i].y));
                        node.Neighbours[i] = mtl >= 0 && mtl < goBoard.pawns.Length ? goBoard.pawns[mtl] : null;
                    }
                    
                    goBoard.pawns[convertMatrixToLine] = node;
                }
            }
            
            for (int x = 0; x < goSettings.boardSize.x; x++)
            {
                for (int y = 0; y > -goSettings.boardSize.y; y--)
                {
                    int convertMatrixToLine = GoTools.ConvertMatrixToLine(goSettings.boardSize, new Vector2(x, y));
                    
                    for(ushort i = 0; i < 4; i++)
                    {
                        short mtl = GoTools.ConvertMatrixToLine(goSettings.boardSize, new Vector2(x + GoPawn.OffsetNeighbours[i].x, y + GoPawn.OffsetNeighbours[i].y));
                        goBoard.pawns[convertMatrixToLine].Neighbours[i] = mtl >= 0 && mtl < goBoard.pawns.Length ? goBoard.pawns[mtl] : null;
                    }
                }
            }
        }
        
        public void PawnInitialization(NodeType pawnType, Vector2 xy)
        {
            short convertMatrixToLine = GoTools.ConvertRayToLine(xy, goBoard.offset, goSettings.boardSize, goSettings.cellsSize);

            if (convertMatrixToLine >= 0 && convertMatrixToLine < goBoard.pawns.Length)
            {
                goBoard.pawns[convertMatrixToLine].OpenMe(pawnType);
            }
        }

        public void UpdateBoard()
        {
            List<GoPawn> tempOfMyNeighbours = new List<GoPawn>();
            foreach (GoPawn goPawn in goBoard.pawns)
            {
                ushort numberOfEmptyNeighbours = goPawn.GetNumberOfEmptyNeighbours();
                ushort numberOfMyNeighbours = goPawn.GetNumberOfMyNeighbours();
                ushort numberOfEnemyNeighbours = goPawn.GetNumberOfEnemyNeighbours();
                ushort numberOfNeighbours = goPawn.GetNumberOfNeighbours();
                
                if (goPawn.isClosed && goPawn.listOfConnectedNeighbours == null && (numberOfEmptyNeighbours == numberOfNeighbours || (numberOfEmptyNeighbours + numberOfEnemyNeighbours) == numberOfNeighbours))
                {
                    goPawn.listOfConnectedNeighbours = new List<GoPawn>{goPawn};
                    goPawn.lider = goPawn;
                }
                else if (goPawn.isClosed && goPawn.lider == null && numberOfMyNeighbours > 0)
                {
                    GoPawn betterOption = goPawn.GetBetterMyNeighbourOption();
                    betterOption.lider.listOfConnectedNeighbours.Add(goPawn);
                    goPawn.lider = betterOption.lider;
                    // foreach (var item in tempOfMyNeighbours.Select((value, i) => new { i, value }))
                    // {
                    //     var value = item.value;
                    //     var index = item.i;
                    //     Debug.Log(index);
                    // }
                }
				
				if(goPawn.lider != null && goPawn.lider.listOfConnectedNeighbours != null && goPawn.lider.listOfConnectedNeighbours.Count > 5)
                {
                    goPawn.lider.RemoveAllFromListOfConnectedNeighbours();
                }
            }
        }
    }
}