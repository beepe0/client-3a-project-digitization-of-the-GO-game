using System;
using UnityEngine;

namespace Go
{
    [Serializable]
    public class Node
    {
        public Go go;
        
        public NodeType pawnType;
        public int index;

        public Vector2 convertXY;
        public Vector3 realXY;

        public GameObject pawn;

        public Node(Go go, NodeType pawnType, int index, Vector2 convertXY, Vector3 realXY)
        {
            this.go = go;
            this.pawnType = pawnType;
            
            this.index = index;
            this.convertXY = convertXY;
            this.realXY = realXY;

            this.go.Initialization(this);
        }
    }

    public enum NodeType : byte
    {
        PawnA,
        PawnB
    }
}