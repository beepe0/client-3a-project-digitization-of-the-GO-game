using Network.UnityClient;
using Network.UnityTools;
using UnityEngine;

namespace Network.Connection
{
    public class Connection : UNetworkClient
    {
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
            RulesHandler.AddRule(0, Test);
        }
        
        public override void OnConnectClient()
        {
            Debug.Log("OnConnectClientAsync!");
        }

        public void Test(UNetworkReadablePacket packet)
        {
            Debug.Log($"{packet.ReadString()}");
        }
    }
}