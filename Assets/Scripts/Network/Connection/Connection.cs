using DebugGUI;
using Go;
using Network.Connection.Player;
using Network.Connection.Room;
using Network.UnityClient;
using Network.UnityTools;
using UnityEngine;

namespace Network.Connection
{
    public class Connection : UNetworkConnection
    { 
        [SerializeField] private GameObject _prefabBoard;
        [SerializeField] private GameObject _prefabLocalPlayer;
        [SerializeField] private ConnectionGUI _connectionGUI;
        private void Awake()
        {
            if (dontDestroyOnLoad) DontDestroyOnLoad(this);
            if (startOnAwake) StartClient();
            if (connectOnAwake) ConnectClient();
        }
        private void FixedUpdate() => UNetworkUpdate.Update();
        private void OnApplicationQuit() => CloseClient();
        protected override void OnCloseClient()
        {
            Debug.Log("OnCloseClient!");
        }
        protected override void OnStartClient()
        {
            Debug.Log("OnStartClient!");
            RulesHandler.AddRule((ushort)PacketType.HandShake, HandShake);
            RulesHandler.AddRule((ushort)PacketType.DisconnectingPlayer, DisconnectingPlayer);
            RulesHandler.AddRule((ushort)PacketType.ConnectingPlayer, ConnectingPlayer);
            RulesHandler.AddRule((ushort)PacketType.StartGame, StartGame);
            RulesHandler.AddRule((ushort)PacketType.UpdatePlayer, UpdatePlayer);
            RulesHandler.AddRule((ushort)PacketType.PawnOpen, PawnOpen);
            RulesHandler.AddRule((ushort)PacketType.PawnClose, PawnClose);
        }
        protected override void OnConnectClient()
        {
            Debug.Log("OnConnectClient!");
        }
        private void HandShake(UNetworkReadablePacket readablePacket)
        {
            Index = readablePacket.Index;
            HandShake();
        }
        private void HandShake()
        {
            UNetworkIOPacket packet = new UNetworkIOPacket((ushort)PacketType.HandShake);
            
            DataHandler.SendDataTcp(packet);
        }
        private void DisconnectingPlayer(UNetworkReadablePacket readablePacket)
        {
            _connectionGUI.ConsoleValue.Insert(0, $"ID: {readablePacket.Index} was disconnected!");
        }
        private void ConnectingPlayer(UNetworkReadablePacket readablePacket)
        {
            _connectionGUI.ConsoleValue.Insert(0, $"ID: {readablePacket.Index} was connected!");
        }
        private void StartGame(UNetworkReadablePacket readablePacket)
        {
            float pawnsSize = readablePacket.ReadFloat();
            Vector2Int boardSize = new Vector2Int(readablePacket.ReadInt(), readablePacket.ReadInt());
            float cellsSize = readablePacket.ReadFloat();
            float cellsCoefSize = readablePacket.ReadFloat();
            ushort roomId = readablePacket.ReadUShort();
            LocalRoom lr;
            GoGame goGame;

            lr = UNetworkRoom.CreateInstance<LocalRoom>(this, roomId, GameObject.FindWithTag("Network.Room").transform);
            goGame = Instantiate(_prefabBoard, GameObject.FindWithTag("Table").transform).GetComponent<GoGame>();
            goGame.Settings.pawnsSize = pawnsSize;
            goGame.Settings.boardSize = boardSize;
            goGame.Settings.cellsSize = cellsSize;
            goGame.Settings.cellsCoefSize = cellsCoefSize;
            goGame.InitializingGame();
            
            lr.mainGame = goGame;
            lr.Open();
            CurrentSession = lr;
            
            if (readablePacket.BufferBytes.Count > readablePacket.ReadPointer)
            {
                int numberOfOpenPawns = readablePacket.ReadInt();
                for (int i = 0; i < numberOfOpenPawns; i++)
                {
                    goGame.Board.pawns[readablePacket.ReadShort()].OpenMe((NodeType)readablePacket.ReadByte());
                }
            }
        }
        private void UpdatePlayer(UNetworkReadablePacket readablePacket)
        {
            ushort clientId = readablePacket.ReadUShort();
            
            if (Index == clientId)
            {
                LocalPlayer lp = UNetworkClient.CreateInstance<LocalPlayer>(this, clientId, GameObject.FindWithTag("Network.Players").transform, _prefabLocalPlayer);
                //CurrentSession.Enter(lp);
            }
            else if (Index != clientId)
            {
                LocalPlayer lp = UNetworkClient.CreateInstance<LocalPlayer>(this, clientId, GameObject.FindWithTag("Network.Players").transform);
                //CurrentSession.Enter(lp);
            }
        }
        private void PawnOpen(UNetworkReadablePacket readablePacket)
        {
            short index = readablePacket.ReadShort();
            byte type = readablePacket.ReadByte();
            GetCurrentSession<LocalRoom>().mainGame.Board.pawns[index].OpenMe((NodeType)type);
        }
        private void PawnClose(UNetworkReadablePacket readablePacket)
        {
            Debug.Log($"PawnClose: {readablePacket.Index}");
            short index = readablePacket.ReadShort();
            GetCurrentSession<LocalRoom>().mainGame.Board.pawns[index].CloseMe();
        }
        public enum PacketType : byte
        {
            HandShake,
            DisconnectingPlayer,
            ConnectingPlayer,
            StartGame,
            UpdatePlayer,
            PawnOpen,
            PawnClose,
            PawnPass,
            ConsoleCommand,
        }
    }
}