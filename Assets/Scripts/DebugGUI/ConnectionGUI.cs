using Singleton;
using UnityEngine;

namespace DebugGUI
{
    public class ConnectionGUI : Singleton<ConnectionGUI>
    {
        public Network.Connection.Connection connection;
        
        public bool isActiveInitializedGUIStyle = true;
        public bool isActiveConnectionPanel = true;
        public bool isActiveClientInfoPanel = true;
        public bool isActiveSettingsPanel = true;
        
        private string _ip = "127.0.0.1";
        private string _port = "34567";
        
        private float _timer = 0;
        private ushort _fps = 0;

        private string _fpsTarget = "60";
        
        private GUIStyle _fields;
        private GUIStyle _labels;
        private GUIStyle _buttons;
        private void InitGUIStyle(bool isActive)
        {
            if (!isActive) return;
            
            _fields = new GUIStyle(GUI.skin.textField);
            _fields.normal.textColor = Color.white;
            _fields.fontSize = 24;
            _fields.alignment = TextAnchor.MiddleCenter;
            
            _labels = new GUIStyle(GUI.skin.label);
            _labels.normal.textColor = Color.white;
            _labels.fontSize = 24;
            _labels.alignment = TextAnchor.MiddleCenter;

            _buttons = new GUIStyle(GUI.skin.button);
            _buttons.fontSize = 24;
            _buttons.alignment = TextAnchor.MiddleCenter;

            isActiveInitializedGUIStyle = false;
        }
        private void ConnectionPanel(bool isActive)
        {
            if (!isActive) return;
            
            GUI.Box(new Rect((int)(Screen.width / 2 - 220/2), 20, 220, 145), Texture2D.blackTexture);
            _ip = GUI.TextField(new Rect((int)(Screen.width / 2 - 200/2), 30, 200, 36), _ip, 15, _fields);
            _port = GUI.TextField(new Rect((int)(Screen.width / 2 - 200/2), 75, 200, 36), _port, 5, _fields);

            if (GUI.Button(new Rect((int)(Screen.width / 2 - 200/2), 120, 200, 36), "CONNECT", _buttons ))
            {
                isActiveConnectionPanel = false;
                isActiveSettingsPanel = false;
                
                ApplicationSettings.Instance.frameRate = int.Parse(_fpsTarget);
                ApplicationSettings.Instance.UpdateField();
                
                connection.serverInternetProtocol = _ip;
                connection.serverPort = ushort.Parse(_port);
                connection.ConnectClient();
            }
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
            GUI.Label(new Rect(0, 0, 220, 48), $"FPS: {_fps.ToString()}", _labels);
        }
        private void SettingsPanel(bool isActive)
        {
            if (!isActive) return;
            
            GUI.Box(new Rect((int)(Screen.width / 2 - 220/2), 180, 220, 145), Texture2D.blackTexture);
            _fpsTarget = GUI.TextField(new Rect((int)(Screen.width / 2 - 200/2), 210, 200, 36), _fpsTarget, 15, _fields);
        }
        private void OnGUI()
        {
            InitGUIStyle(isActiveInitializedGUIStyle);
            ConnectionPanel(isActiveConnectionPanel);
            ClientInfoPanel(isActiveClientInfoPanel);
            SettingsPanel(isActiveSettingsPanel);
        }
    }
}