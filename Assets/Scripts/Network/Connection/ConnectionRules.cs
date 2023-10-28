using Network.UClient;
using Network.UNTools;
using UnityEditor.Rendering;
using UnityEngine;

namespace Network.Connection
{
    public abstract class ConnectionRules
    {
        public enum PacketType : ushort
        {
            OnWelcome,
            SynchronizePosition
        }
        public class GeneralRules : UNetworkIORules.IGeneralRules
        {
            public void OnDisconnect()
            {
                Debug.Log("A client disconnected!");
            }
        }
        public class InputRules : UNetworkIORules.IInputRules
        {
            public void OnWelcome(UNetworkReadablePacket inputPacket)
            {
                Debug.Log($"ID: {inputPacket.Index}, LN: {inputPacket.Length}, PT: {(PacketType)inputPacket.PacketNumber}, DT: {inputPacket.ReadString()}");
                UNetworkCore.OutputRules.OnWelcome();
            }
        }
        public class OutputRules : UNetworkIORules.IOutputRules
        {
            public void OnWelcome()
            {
                UNetworkIOPacket packet = new UNetworkIOPacket((ushort)PacketType.OnWelcome);
                
                packet.Write("TEST123");
                
                UNetworkCore.DataHandler.Tcp.SendData(packet);
            }
            
            public void SynchronizePosition(Vector3 position)
            {
                UNetworkIOPacket packet = new UNetworkIOPacket((ushort)PacketType.SynchronizePosition);
                
                packet.Write(position.x);
                packet.Write(position.y);
                packet.Write(position.z);
                
                UNetworkCore.DataHandler.Udp.SendData(packet);
            }
        }
    }
}