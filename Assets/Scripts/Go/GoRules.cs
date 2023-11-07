using System.Collections.Generic;
using Network.Connection;
using Network.UnityTools;
using UnityEngine;

namespace Go
{
    public class GoRules : MonoBehaviour
    {
        private GoGame _goGame;

        public void GameInitialization(GoGame goGame)
        {
            _goGame = goGame;
            
            _goGame.Settings.prefabBoard.transform.localScale = new Vector3((_goGame.Settings.boardSize.x - 1) / _goGame.Settings.cellsSize, 1, (_goGame.Settings.boardSize.y - 1) / _goGame.Settings.cellsSize);
            _goGame.Settings.boardMaterial.mainTextureScale = new Vector2((_goGame.Settings.boardSize.x - 1), (_goGame.Settings.boardSize.y - 1));
            _goGame.Settings.pawnsSize = (20 / _goGame.Settings.cellsSize) / _goGame.Settings.cellsCoefSize;
            
            _goGame.Board.pawnCursor = Instantiate(_goGame.Settings.prefabPawnCursor, gameObject.transform);
            _goGame.Board.offset = new Vector2(_goGame.Settings.prefabBoard.transform.localScale.x / 2, -_goGame.Settings.prefabBoard.transform.localScale.z / 2);
            _goGame.Board.pawnOffset = new Vector2(_goGame.Board.offset.x, -_goGame.Board.offset.y);
            _goGame.Board.pawns = new GoPawn[_goGame.Settings.boardSize.x * _goGame.Settings.boardSize.y];

            for (int x = 0; x < _goGame.Settings.boardSize.x; x++)
            {
                for (int y = 0; y > -_goGame.Settings.boardSize.y; y--)
                {
                    int convertMatrixToLine = GoTools.ConvertMatrixToLine(_goGame.Settings.boardSize, new Vector2(x, y));
                    
                    Vector3 newPos = new Vector3(x / _goGame.Settings.cellsSize - (_goGame.Board.pawnOffset.x), 0.5f, y / _goGame.Settings.cellsSize + (_goGame.Board.pawnOffset.y));
                    GameObject pawnObject = Instantiate(_goGame.Settings.prefabPawnAB, newPos, Quaternion.identity, gameObject.transform);
                    GoPawn node = new GoPawn(goGame, convertMatrixToLine, pawnObject);

                    pawnObject.SetActive(false);
                    pawnObject.name = $"xyz: {newPos}";
                    pawnObject.transform.SetParent(gameObject.transform);
                    
                    for(ushort i = 0; i < 4; i++)
                    {
                        short mtl = GoTools.ConvertMatrixToLine(_goGame.Settings.boardSize, new Vector2(x + GoPawn.OffsetNeighbours[i].x, y + GoPawn.OffsetNeighbours[i].y));
                        node.Neighbours[i] = mtl >= 0 && mtl < _goGame.Board.pawns.Length ? _goGame.Board.pawns[mtl] : null;
                    }
                    
                    _goGame.Board.pawns[convertMatrixToLine] = node;
                }
            }
            
            for (int x = 0; x < _goGame.Settings.boardSize.x; x++)
            {
                for (int y = 0; y > -_goGame.Settings.boardSize.y; y--)
                {
                    int convertMatrixToLine = GoTools.ConvertMatrixToLine(_goGame.Settings.boardSize, new Vector2(x, y));
                    
                    for(ushort i = 0; i < 4; i++)
                    {
                        short mtl = GoTools.ConvertMatrixToLine(_goGame.Settings.boardSize, new Vector2(x + GoPawn.OffsetNeighbours[i].x, y + GoPawn.OffsetNeighbours[i].y));
                        _goGame.Board.pawns[convertMatrixToLine].Neighbours[i] = mtl >= 0 && mtl < _goGame.Board.pawns.Length ? _goGame.Board.pawns[mtl] : null;
                    }
                }
            }
        }
        
        public void PawnInitialization(Vector2 xy)
        {
            UNetworkIOPacket packet = new UNetworkIOPacket((ushort)Connection.PacketType.PawnOpen);
            packet.Write(xy.x);
            packet.Write(xy.y);
            _goGame.Conn.DataHandler.SendDataTcp(packet);
            
            // if (convertMatrixToLine >= 0 && convertMatrixToLine < _goGame.Board.pawns.Length)
            // {
            //     goPawn = _goGame.Board.pawns[convertMatrixToLine].OpenMe(pawnType);
            //     if (goPawn == null) return;
            //     
            //     ushort numberOfEmptyNeighbours = goPawn.GetNumberOfEmptyNeighbours();
            //     ushort numberOfMyNeighbours = goPawn.GetNumberOfMyNeighbours();
            //     ushort numberOfEnemyNeighbours = goPawn.GetNumberOfEnemyNeighbours();
            //     ushort numberOfNeighbours = goPawn.GetNumberOfNeighbours();
            //     
            //     if (goPawn.lider == null && (numberOfEmptyNeighbours == numberOfNeighbours || (numberOfEmptyNeighbours + numberOfEnemyNeighbours) == numberOfNeighbours))
            //     {
            //         goPawn.listOfConnectedNeighbours = new List<GoPawn>{goPawn};
            //         goPawn.lider = goPawn;
            //     }
            //     else if (goPawn.lider == null && numberOfMyNeighbours > 0)
            //     {
            //         GoPawn betterOption = goPawn.GetBetterMyNeighbourOption();
            //         betterOption.lider.listOfConnectedNeighbours.Add(goPawn);
            //         goPawn.lider = betterOption.lider;
            //     }
            //     
            //     if (goPawn.lider != null)
            //     { 
            //         if (!goPawn.CanLive() && goPawn.lider.listOfConnectedNeighbours.Count > 1)
            //         {
            //             goPawn.lider.listOfConnectedNeighbours.Remove(goPawn);
            //             goPawn.CloseMe();
            //         }else _goGame.Board.numberOfSteps++;
            //     }
            // }
            //
            //UpdateBoard();
        }

        public void PawnPass() => _goGame.Board.numberOfSteps++;

        public void UpdateBoard()
        {
            for (int i = 0; i < _goGame.Board.openPawns.Count && _goGame.Board.openPawns.Count > 0; i++)
            {
                GoPawn goPawn = _goGame.Board.openPawns[i];
                if(goPawn.lider != null && !goPawn.CanLive())
                {
                    goPawn.lider.RemoveAllFromListOfConnectedNeighbours();
                }
            }
        }
    }
}