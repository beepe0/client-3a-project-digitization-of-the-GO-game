﻿using Go;
using Network.UnityClient;
using Network.UnityTools;
using Player;
using UnityEngine;

namespace Network.Connection
{
    public class Connection : UNetworkClient
    {
        [SerializeField] private GameObject prefabBoard;
        [SerializeField] private GameObject prefabLocalPlayer;
        [SerializeField] private GameObject prefabGlobalPlayer;
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
            RulesHandler.AddRule((ushort)PacketType.StartGame, StartGame);
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
            StartGame();
        }

        private void HandShake()
        {
            UNetworkIOPacket packet = new UNetworkIOPacket((ushort)PacketType.HandShake);
            
            DataHandler.SendDataTcp(packet);
        }
        private void StartGame()
        {
            UNetworkIOPacket packet = new UNetworkIOPacket((ushort)PacketType.StartGame);
            
            DataHandler.SendDataTcp(packet);
        }

        private void StartGame(UNetworkReadablePacket readablePacket)
        {
            float pawnsSize = readablePacket.ReadFloat();
            Vector2Int boardSize = new Vector2Int(readablePacket.ReadInt(), readablePacket.ReadInt());
            float cellsSize = readablePacket.ReadFloat();
            float cellsCoefSize = readablePacket.ReadFloat();

            Debug.Log($"Pawns Size: {pawnsSize}, Board Size: {boardSize}, Cells Size: {cellsSize}, Cells Coef Size: {cellsCoefSize}");
            Instantiate(prefabLocalPlayer);
            GoGame goGame = Instantiate(prefabBoard, GameObject.FindWithTag("Table").transform).GetComponent<GoGame>();
            goGame.Rules.GameInitialization(goGame);
            LocalPlayer.Instance.mainGame = goGame;
            
            goGame.Settings.pawnsSize = pawnsSize;
            goGame.Settings.boardSize = boardSize;
            goGame.Settings.cellsSize = cellsSize;
            goGame.Settings.cellsCoefSize = cellsCoefSize;
        }

        private void UpdatePlayer(UNetworkReadablePacket readablePacket)
        {
            ushort clientId = readablePacket.ReadUShort();
            if (Index != clientId)
            {
                Instantiate(prefabGlobalPlayer);
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
            StartGame,
            UpdatePlayer,
            PawnOpen,
            PawnClose,
        }
    }
}