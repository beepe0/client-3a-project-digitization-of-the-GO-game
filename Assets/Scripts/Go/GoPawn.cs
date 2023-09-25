using System;
using UnityEngine;

namespace Go
{
    [Serializable]
    public class GoPawn
    { 
        public int index;
        public bool isClosed;
        public NodeType pawnType;

        public GameObject pawnObject;
        public MeshRenderer pawnMeshRenderer;
        public Vector3 pawnPosition;
        
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
    }

    public enum NodeType : byte
    {
        None,
        PawnA,
        PawnB
    }
}