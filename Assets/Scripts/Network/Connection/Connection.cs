using Network.UnityClient;
using Singleton;
using UnityEngine;

namespace Network.Connection
{
    public class Connection : Singleton<Connection>
    {
        [SerializeField] public UNetworkClientManager networkClientManager;
        private void Awake()
        {
            networkClientManager.Client.RulesHandler.UpdateGeneralRules(new ConnectionRules.GeneralRules());
            networkClientManager.Client.RulesHandler.UpdateInputRules(new ConnectionRules.InputRules());
            networkClientManager.Client.RulesHandler.UpdateOutputRules(new ConnectionRules.OutputRules());
            
            networkClientManager.Client.RulesHandler.AddNewRule((ushort)ConnectionRules.PacketType.OnWelcome, ((ConnectionRules.InputRules)networkClientManager.Client.InputRules).OnWelcome); 
        }
    }
}