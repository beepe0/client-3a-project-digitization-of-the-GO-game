using Network.UnityClient;
using Network.UnityTools;
using UnityEngine;

namespace Network.Connection
{
    public sealed class ConnectionRules
    {
        public enum PacketType : ushort
        {
            OnWelcome,
            SynchronizePosition
        }
        public class GeneralRules : UNetworkClientIORules.IGeneralRules
        {
            public void OnDisconnect()
            {
                Debug.Log("The client is disconnected!");
            }
        }
        public class InputRules : UNetworkClientIORules.IInputRules
        {
            public void OnWelcome(UNetworkReadablePacket inputPacket)
            {
                Debug.Log($"ID: {inputPacket.Index}, LN: {inputPacket.Length}, PT: {(PacketType)inputPacket.PacketNumber}, DT: {inputPacket.ReadString()}");
                //Connection.Instance.networkClientManager.Client.OutputRules.OnWelcome();
            }
        }
        public class OutputRules : UNetworkClientIORules.IOutputRules
        {
            public void OnWelcome()
            {
                UNetworkIOPacket packet = new UNetworkIOPacket((ushort)PacketType.OnWelcome);
                
                packet.Write("TEST123");
                
                Connection.Instance.networkClientManager.Client.DataHandler.SendDataTcp(packet);
            }
            
            public void SynchronizePosition(Vector3 position)
            {
                UNetworkIOPacket packet = new UNetworkIOPacket((ushort)PacketType.SynchronizePosition);
                
                packet.Write(position.x);
                packet.Write(position.y);
                packet.Write(position.z);
                
                Connection.Instance.networkClientManager.Client.DataHandler.SendDataUpd(packet);
            }
        }
    }
}