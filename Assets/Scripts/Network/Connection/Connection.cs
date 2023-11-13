using DebugGUI;
using Go;
using Network.UnityClient;
using Network.UnityTools;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Network.Connection
{
    public class Connection : UNetworkClient
    { 
        [SerializeField] private GameObject _prefabBoard;
        [SerializeField] private GameObject _prefabLocalPlayer;
        [SerializeField] private GameObject _prefabGlobalPlayer;
        [SerializeField] private ConnectionGUI _connectionGUI;
        private void Awake()
        {
            if (dontDestroyOnLoad) DontDestroyOnLoad(this);
            if (startOnAwake) StartClient();
            if (connectOnAwake) ConnectClient();
        }
        private void FixedUpdate() => UNetworkUpdate.Update();
        private void OnApplicationQuit() => CloseClient();
        public override void OnCloseClient()
        {
            Debug.Log("OnCloseClient!");
        }
        public override void OnStartClient()
        {
            Debug.Log("OnStartClient!");
            RulesHandler.AddRule((ushort)PacketType.HandShake, HandShake);
            RulesHandler.AddRule((ushort)PacketType.DisconnectingPlayer, DisconnectingPlayer);
            RulesHandler.AddRule((ushort)PacketType.ConnectingPlayer, ConnectingPlayer);
            RulesHandler.AddRule((ushort)PacketType.JoinGame, JoinGame);
            RulesHandler.AddRule((ushort)PacketType.CreateGame, CreateGame);
            RulesHandler.AddRule((ushort)PacketType.UpdatePlayer, UpdatePlayer);
            RulesHandler.AddRule((ushort)PacketType.PawnOpen, PawnOpen);
            RulesHandler.AddRule((ushort)PacketType.PawnClose, PawnClose);
        }
        public override void OnConnectClient()
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
        private void JoinGame()
        {
            UNetworkIOPacket packet = new UNetworkIOPacket((ushort)PacketType.JoinGame);
            
            DataHandler.SendDataTcp(packet);
        }
        private void JoinGame(UNetworkReadablePacket readablePacket)
        {
            float pawnsSize = readablePacket.ReadFloat();
            Vector2Int boardSize = new Vector2Int(readablePacket.ReadInt(), readablePacket.ReadInt());
            float cellsSize = readablePacket.ReadFloat();
            float cellsCoefSize = readablePacket.ReadFloat();
            
            GoGame goGame = Instantiate(_prefabBoard, GameObject.FindWithTag("Table").transform).GetComponent<GoGame>();
            
            goGame.Settings.pawnsSize = pawnsSize;
            goGame.Settings.boardSize = boardSize;
            goGame.Settings.cellsSize = cellsSize;
            goGame.Settings.cellsCoefSize = cellsCoefSize;
            
            goGame.InitializingGame();
            
            if (readablePacket.BufferBytes.Count > readablePacket.ReadPointer)
            {
                int numberOfOpenPawns = readablePacket.ReadInt();
                for (int i = 0; i < numberOfOpenPawns; i++)
                {
                    goGame.Board.pawns[readablePacket.ReadShort()].OpenMe((NodeType)readablePacket.ReadByte());
                }
            }
        }
        private void CreateGame(UNetworkReadablePacket readablePacket)
        {
            float pawnsSize = readablePacket.ReadFloat();
            Vector2Int boardSize = new Vector2Int(readablePacket.ReadInt(), readablePacket.ReadInt());
            float cellsSize = readablePacket.ReadFloat();
            float cellsCoefSize = readablePacket.ReadFloat();

            if (LocalPlayer.Instance.mainGame != null) Destroy(LocalPlayer.Instance.mainGame.gameObject);
            
            GoGame goGame = Instantiate(_prefabBoard, GameObject.FindWithTag("Table").transform).GetComponent<GoGame>();
            
            goGame.Settings.pawnsSize = pawnsSize;
            goGame.Settings.boardSize = boardSize;
            goGame.Settings.cellsSize = cellsSize;
            goGame.Settings.cellsCoefSize = cellsCoefSize;
            
            goGame.InitializingGame();
        }
        private void UpdatePlayer(UNetworkReadablePacket readablePacket)
        {
            ushort clientId = readablePacket.ReadUShort();
            if (Index == clientId)
            {
                Instantiate(_prefabLocalPlayer);
            }
            else if (Index != clientId)
            {
                Instantiate(_prefabGlobalPlayer);
            }
        }
        private void PawnOpen(UNetworkReadablePacket readablePacket)
        {
            short index = readablePacket.ReadShort();
            byte type = readablePacket.ReadByte();
            LocalPlayer.Instance.mainGame.Board.pawns[index].OpenMe((NodeType)type);
        }
        private void PawnClose(UNetworkReadablePacket readablePacket)
        {
            short index = readablePacket.ReadShort();
            LocalPlayer.Instance.mainGame.Board.pawns[index].CloseMe();
        }
        public enum PacketType : byte
        {
            HandShake,
            DisconnectingPlayer,
            ConnectingPlayer,
            JoinGame,
            CreateGame,
            UpdatePlayer,
            PawnOpen,
            PawnClose,
            PawnPass,
            ConsoleCommand,
        }
    }
}