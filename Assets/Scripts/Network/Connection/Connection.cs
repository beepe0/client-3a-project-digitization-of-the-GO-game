using Network.UnityClient;
using Network.UnityClient.Handlers;
using Singleton;
using UnityEngine;

namespace Network.Connection
{
    public class Connection : Singleton<Connection>
    {
        [SerializeField] public UNetworkClientManager networkClientManager;

        public UNetworkClientRulesHandler RulesHandler;
        public UNetworkClientDataHandler DataHandler;
        
        public ConnectionRules.GeneralRules GeneralRules;
        public ConnectionRules.InputRules InputRules;
        public ConnectionRules.OutputRules OutputRules;
        private void Awake()
        {
            RulesHandler = networkClientManager.Client.RulesHandler;
            DataHandler = networkClientManager.Client.DataHandler;
            
            RulesHandler.UpdateGeneralRules(new ConnectionRules.GeneralRules());
            RulesHandler.UpdateInputRules(new ConnectionRules.InputRules());
            RulesHandler.UpdateOutputRules(new ConnectionRules.OutputRules());
            
            GeneralRules = networkClientManager.Client.GeneralRules as ConnectionRules.GeneralRules;
            InputRules = networkClientManager.Client.InputRules as ConnectionRules.InputRules;
            OutputRules = networkClientManager.Client.OutputRules as ConnectionRules.OutputRules;

            RulesHandler.AddNewRule((ushort)ConnectionRules.PacketType.HandShake, InputRules!.HandShake); 
        }
        private void Update()
        {
            OutputRules.SynchronizePosition(gameObject.transform.position);
        }
    }
}