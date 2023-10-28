using System;
using Network.UClient;
using UnityEngine;
using Singleton;

namespace Network.Connection
{
    public class Connection : Singleton<Connection>
    {
        private void Awake()
        {
            UNetworkCore.GeneralRules = new ConnectionRules.GeneralRules();
            UNetworkCore.InputRules = new ConnectionRules.InputRules();
            UNetworkCore.OutputRules = new ConnectionRules.OutputRules();
            
            UNetworkCore.RulesHandler.AddNewRule((ushort)ConnectionRules.PacketType.OnWelcome, UNetworkCore.InputRules.OnWelcome); 
            UNetworkCore.OutputRules.OnWelcome();
        }

        private void Update()
        {
            ((ConnectionRules.OutputRules)UNetworkCore.OutputRules).SynchronizePosition(gameObject.transform.position);
        }
    }
}