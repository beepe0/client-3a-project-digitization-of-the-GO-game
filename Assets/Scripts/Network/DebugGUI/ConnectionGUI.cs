using UnityEngine;

namespace Network.DebugGUI
{
    public class ConnectionGUI : MonoBehaviour
    {
        private bool _isInitializedGUIStyle;
        private string _ip = "";
        private string _port = "";
        
        private GUIStyle _fields;
        private GUIStyle _buttons;

        private void InitGUIStyle()
        {
            _fields = new GUIStyle(GUI.skin.textField);
            _fields.normal.textColor = Color.white;
            _fields.fontSize = 24;
            _fields.alignment = TextAnchor.MiddleCenter;

            _buttons = new GUIStyle(GUI.skin.button);
            _buttons.fontSize = 24;
            _fields.alignment = TextAnchor.MiddleCenter;

            _isInitializedGUIStyle = true;
        }

        private void OnGUI()
        {
            if (!_isInitializedGUIStyle) InitGUIStyle();
            
            GUI.Box(new Rect((int)(Screen.width / 2 - 220/2), 20, 220, 145), Texture2D.blackTexture);
            _ip = GUI.TextField(new Rect((int)(Screen.width / 2 - 200/2), 30, 200, 36), _ip, 15, _fields);
            _port = GUI.TextField(new Rect((int)(Screen.width / 2 - 200/2), 75, 200, 36), _port, 5, _fields);

            if (GUI.Button(new Rect((int)(Screen.width / 2 - 200/2), 120, 200, 36), "CONNECT", _buttons ))
            {
                
            }
        }
    }
}