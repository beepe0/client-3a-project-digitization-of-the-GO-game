using System;
using UnityEngine;

namespace Go
{
    [Serializable]
    public class Node
    {
        public int index;
        public bool isClosed;
        public NodeType pawnType;
        
        public Go go;
        public GameObject pawn;
        public GameObject pawnPosition;

        public Node[] neighbours;

        public Node(Go go, int index, GameObject pawnPosition)
        {
            this.go = go;
            this.index = index;
            this.pawnPosition = pawnPosition;
        }
    }

    public enum NodeType : byte
    {
        None,
        PawnA,
        PawnB
    }
}