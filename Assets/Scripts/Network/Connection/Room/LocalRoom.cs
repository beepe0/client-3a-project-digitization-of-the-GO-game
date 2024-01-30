using CustomEditor.Attributes;
using Go;
using Network.UnityClient;
using UnityEngine.Serialization;

namespace Network.Connection.Room
{
    public class LocalRoom : UNetworkRoom
    {
        [ReadOnlyInspector] public string title;
        [ReadOnlyInspector] public ushort numberOfPlayers;

        public GoGame mainGame;
    }
}