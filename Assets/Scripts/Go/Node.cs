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
        public bool isClosed;

        public GameObject pawn;
        public GameObject pawnPosition;

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