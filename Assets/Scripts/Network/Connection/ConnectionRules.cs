using Network.UnityClient;
using Network.UnityTools;
using UnityEngine;

namespace Network.Connection
{
    public sealed class ConnectionRules
    {
        public enum PacketType : ushort
        {
            HandShake,
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
            public void HandShake(UNetworkReadablePacket inputPacket)
            {
                Debug.Log($"ID: {inputPacket.Index}, LN: {inputPacket.Length}, PT: {(PacketType)inputPacket.PacketNumber}, DT: {inputPacket.ReadString()}");
                Connection.Instance.OutputRules.HandShake();
            }
        }
        public class OutputRules : UNetworkClientIORules.IOutputRules
        {
            public void HandShake()
            {
                UNetworkIOPacket packet = new UNetworkIOPacket((ushort)PacketType.HandShake);
                
                packet.Write("C-OK!");
                
                Connection.Instance.DataHandler.SendDataTcp(packet);
            }
            
            public void SynchronizePosition(Vector3 position)
            {
                UNetworkIOPacket packet = new UNetworkIOPacket((ushort)PacketType.SynchronizePosition);
                
                packet.Write(position.x);
                packet.Write(position.y);
                packet.Write(position.z);
                
                Connection.Instance.DataHandler.SendDataTcp(packet);
            }
        }
    }
}