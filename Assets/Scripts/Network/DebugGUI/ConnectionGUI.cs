using Singleton;
using UnityEngine;
using UnityEngine.Serialization;

namespace Network.DebugGUI
{
    public class ConnectionGUI : Singleton<ConnectionGUI>
    {
        public Connection.Connection connection;
        public bool isActiveInitializedGUIStyle = true;
        public bool isActiveConnectionPanel = true;
        
        private string _ip = "127.0.0.1";
        private string _port = "34567";
        
        private GUIStyle _fields;
        private GUIStyle _buttons;

        private void InitGUIStyle(bool isActive)
        {
            if (!isActive) return;
            
            _fields = new GUIStyle(GUI.skin.textField);
            _fields.normal.textColor = Color.white;
            _fields.fontSize = 24;
            _fields.alignment = TextAnchor.MiddleCenter;

            _buttons = new GUIStyle(GUI.skin.button);
            _buttons.fontSize = 24;
            _fields.alignment = TextAnchor.MiddleCenter;

            isActiveInitializedGUIStyle = false;
        }

        private async void ConnectionPanel(bool isActive)
        {
            if (!isActive) return;
   
            GUI.Box(new Rect((int)(Screen.width / 2 - 220/2), 20, 220, 145), Texture2D.blackTexture);
            _ip = GUI.TextField(new Rect((int)(Screen.width / 2 - 200/2), 30, 200, 36), _ip, 15, _fields);
            _port = GUI.TextField(new Rect((int)(Screen.width / 2 - 200/2), 75, 200, 36), _port, 5, _fields);

            if (GUI.Button(new Rect((int)(Screen.width / 2 - 200/2), 120, 200, 36), "CONNECT", _buttons ))
            {
                isActiveConnectionPanel = false;
                connection.serverInternetProtocol = _ip;
                connection.serverPort = ushort.Parse(_port);
                connection.ConnectClient();
            }
        }
        private void OnGUI()
        {
            InitGUIStyle(isActiveInitializedGUIStyle);
            ConnectionPanel(isActiveConnectionPanel);
        }
    }
}