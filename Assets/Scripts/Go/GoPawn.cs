using System;
using CustomEditor.Attributes;
using UnityEngine;

namespace Go
{
    [Serializable]
    public class GoPawn
    { 
        [ReadOnlyInspector] public int index;
        [ReadOnlyInspector] public bool isClosed;
        [ReadOnlyInspector] public NodeType pawnType;

        [ReadOnlyInspector] public GameObject pawnObject;
        [ReadOnlyInspector] public MeshRenderer pawnMeshRenderer;
        [ReadOnlyInspector] public Vector3 pawnPosition;
        
        [NonSerialized]
        public GoGame MainGame;
        [NonSerialized]
        public GoPawn[] Neighbours;
        [NonSerialized] 
        public static Vector2[] OffsetNeigh =
        {
            Vector2.right,
            Vector2.left,
            Vector2.up,
            Vector2.down,
        };
        
        public GoPawn(GoGame mainGame, int index, GameObject pawnObject)
        {
            this.Neighbours = new GoPawn[4];
                
            this.MainGame = mainGame;
            this.index = index;
            this.pawnObject = pawnObject;
            this.pawnPosition = pawnObject.transform.position;
            this.pawnMeshRenderer = pawnObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        }

        public ushort GetNumberOfEmptyNeighbors()
        {
            ushort count = 0;
            foreach (GoPawn node in Neighbours)
            {
                if (node.isClosed) continue;
                count++;
            }

            return count;
        }
    }

    public enum NodeType : byte
    {
        None,
        PawnA,
        PawnB
    }
}