using System;
using System.Collections.Generic;
using Network.Connection;
using Network.UnityTools;
using Singleton;
using UnityEngine;

namespace DebugGUI
{
    public class ConnectionGUI : Singleton<ConnectionGUI>
    {
        public Connection connection;
        
        public bool isActiveInitializedGUIStyle = true;
        public bool isActiveClientInfoPanel = false;
        public bool isActiveConsole = false;
        
        private float _timer = 0;
        private ushort _fps = 0;

        private string _consoleFieldValue = "";
        public readonly List<string> ConsoleValue = new();
        private int _consoleY;
        
        private GUIStyle _fields;
        private GUIStyle _labels;
        private GUIStyle _buttons;

        private void InputFrom()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T)) isActiveConsole = !isActiveConsole;
            else _consoleY += (int)Input.mouseScrollDelta.y;
        }
        private void InitGUIStyle(bool isActive)
        {
            if (!isActive) return;
            
            _fields = new GUIStyle(GUI.skin.textField);
            _fields.normal.textColor = Color.white;
            _fields.fontSize = 15;
            _fields.alignment = TextAnchor.MiddleCenter;
            
            _labels = new GUIStyle(GUI.skin.label);
            _labels.normal.textColor = Color.white;
            _labels.fontSize = 15;
            _labels.alignment = TextAnchor.MiddleLeft;

            _buttons = new GUIStyle(GUI.skin.button);
            _buttons.fontSize = 15;
            _buttons.alignment = TextAnchor.MiddleCenter;

            isActiveInitializedGUIStyle = false;
        }
        private void ClientInfoPanel(bool isActive)
        {
            if (!isActive) return;
            
            if ((_timer += Time.deltaTime) > 1)
            {
                _fps = (ushort)(1 / Time.deltaTime);
                _timer = 0;
            }
            GUI.Box(new Rect(0, 0, 220, 48), Texture2D.blackTexture);
            GUI.Label(new Rect(10, 0, 220, 48), $"FPS: {_fps.ToString()}", _labels);
        }
        private void ConsolePanel(bool isActive)
        {
            if (!isActive) return;
            
            _consoleY = Math.Clamp(_consoleY, 0, (ConsoleValue.Count > 5 ? ConsoleValue.  Count - 5 : 0));
            for (int i = _consoleY, k = 0; k < (ConsoleValue.Count > 5 ? 5 : ConsoleValue.Count); i++, k++)
            {
                GUI.Label(new Rect(10, Screen.height - 80 - (k * 20), 600, 27), ConsoleValue[i], _labels);
            }
            _consoleFieldValue = GUI.TextField(new Rect(10, Screen.height - 50, 600, 27), _consoleFieldValue, 512, _fields);
            if (GUI.Button(new Rect(620, Screen.height - 50, 100, 27), "SEND", _buttons ))
            {
                ParserConsoleCommand(_consoleFieldValue);
            }       
        }
        private void Awake()
        {
            connection.RulesHandler.AddRule((ushort)Connection.PacketType.ConsoleCommand, ConsoleCommand);
        }
        private void Update() => InputFrom();
        private void OnGUI()
        {
            InitGUIStyle(isActiveInitializedGUIStyle);
            ClientInfoPanel(isActiveClientInfoPanel);
            ConsolePanel(isActiveConsole);
        }
        private void ParserConsoleCommand(string value)
        {
            string[] keys = value.Split(' ');
            bool isGlobal = false;
            bool clearField = false;
            bool showAnswer = true;
            string answer = null;
            
            switch (keys[0])
            {
                case "local" :
                    if (keys[1].Equals("connect"))
                    {
                        clearField = true;
                        connection.serverInternetProtocol = keys[2];
                        connection.serverPort = ushort.Parse(keys[3]);
                        connection.ConnectClient();
                        
                        answer = $"connecting to the server [{connection.serverInternetProtocol}:{connection.serverPort}]";
                    }
                    else if (keys[1].Equals("disconnect"))
                    {
                        clearField = true;
                        Application.Quit();
                        
                        answer = $"disconnecting from the server [{connection.serverInternetProtocol}:{connection.serverPort}]";
                    }
                    else if (keys[1].Equals("target-fps"))
                    {
                        showAnswer = false;
                        clearField = true;
                        ApplicationSettings.Instance.frameRate = int.Parse(keys[2]);
                        ApplicationSettings.Instance.UpdateField();
                    }
                    else if (keys[1].Equals("show-statics"))
                    {
                        showAnswer = false;
                        clearField = true;
                        isActiveClientInfoPanel = !isActiveClientInfoPanel;
                    }
                    break;
                case "global" : ConsoleCommand(value); isGlobal = true; break;
                default:
                    clearField = false;
                    answer = $"command \"{value}\" wasn't found!";
                    break;
            }

            if (!isGlobal && showAnswer) ConsoleCommand(answer, clearField);
        }
        private void ConsoleCommand(string value)
        {
            UNetworkIOPacket packet = new UNetworkIOPacket((ushort)Connection.PacketType.ConsoleCommand);
            
            packet.Write(value);
            
            connection.DataHandler.SendDataTcp(packet);
        }

        private void ConsoleCommand(UNetworkReadablePacket readablePacket)
        {
            _consoleFieldValue = "";
            ConsoleValue.Insert(0,$"ID: {readablePacket.Index} {readablePacket.ReadString()}");
        }
        private void ConsoleCommand(string value, bool clearField)
        {
            if (clearField) _consoleFieldValue = "";
            ConsoleValue.Insert(0,$"ID: {connection.Index} {value}");
        }
    }
}