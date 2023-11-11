using System;
using System.Collections.Generic;
using System.Linq;
using CustomEditor.Attributes;
using UnityEngine;

namespace Go
{
    [Serializable]
    public class GoPawn
    { 
        [ReadOnlyInspector] public ushort index;
        [ReadOnlyInspector] public bool isClosed;
        [ReadOnlyInspector] public NodeType pawnType;

        [ReadOnlyInspector] public GameObject pawnObject;
        [ReadOnlyInspector] public MeshRenderer pawnMeshRenderer;
        [ReadOnlyInspector] public Vector3 pawnPosition;
        
        [NonSerialized]
        public GoGame MainGame;
        
        public GoPawn(GoGame mainGame, int index, GameObject pawnObject)
        {
            this.MainGame = mainGame;
            this.pawnObject = pawnObject;
            this.pawnPosition = pawnObject.transform.position;
            this.pawnMeshRenderer = pawnObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        }
		
		public void CloseMe()
        {
			this.isClosed = false;
            this.pawnType = NodeType.None;
            this.pawnMeshRenderer.material = MainGame.Settings.materialPawnNone;
            this.pawnObject.SetActive(false);
            
            this.MainGame.Board.openPawns.Remove(this);
		}

		public GoPawn OpenMe(NodeType nodeType)
		{
            if (this.isClosed) return null;
    
            this.index = (ushort)MainGame.Board.openPawns.Count;
            this.isClosed = true;
            this.pawnType = nodeType;
            this.pawnMeshRenderer.material = nodeType == NodeType.PawnA ? MainGame.Settings.materialPawnA : MainGame.Settings.materialPawnB;
            this.pawnObject.transform.localScale = new Vector3(MainGame.Settings.pawnsSize, 0.5f, MainGame.Settings.pawnsSize);
            this.pawnObject.SetActive(true);
            
            this.MainGame.Board.openPawns.Add(this);
            
            return this;
        }
    }

    public enum NodeType : byte
    {
        None,
        PawnA,
        PawnB,
    }
}