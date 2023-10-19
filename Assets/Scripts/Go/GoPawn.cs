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
        public List<GoPawn> listOfConnectedNeighbours;
        [NonSerialized]
        public GoPawn lider;
        [NonSerialized] 
        public static Vector2[] OffsetNeighbours =
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
		
		public void CloseMe()
		{
			this.isClosed = false;
            this.pawnType = NodeType.None;
            this.pawnMeshRenderer.material = MainGame.goSettings.materialPawnNone;
            this.listOfConnectedNeighbours = null;
            this.lider = null;
            this.pawnObject.SetActive(false);
            
		}

		public GoPawn OpenMe(NodeType nodeType)
		{
            if (this.isClosed) return null;
            
			this.isClosed = true;
            this.pawnType = nodeType;
            this.pawnMeshRenderer.material = nodeType == NodeType.PawnA ? MainGame.goSettings.materialPawnA : MainGame.goSettings.materialPawnB;
            this.pawnObject.transform.localScale = new Vector3(MainGame.goSettings.pawnsSize, 0.5f, MainGame.goSettings.pawnsSize);
            this.pawnObject.SetActive(true);
            
            return this;
        }

        public ushort GetNumberOfEmptyNeighbours()
        {
            ushort count = 0;
            foreach (GoPawn node in Neighbours)
            {
                if (node == null || node.isClosed) continue;
                count++;
            }

            return count;
        }
        public ushort GetNumberOfMyNeighbours()
        {
            ushort count = 0;
            foreach (GoPawn node in Neighbours)
            {
                if (node == null || (node.pawnType != this.pawnType)) continue;
                count++;
            }

            return count;
        }

        public ushort GetNumberOfEnemyNeighbours()
        {
            ushort count = 0;
            foreach (GoPawn node in Neighbours)
            {
                if (node == null || (node.pawnType == this.pawnType || node.pawnType == NodeType.None)) continue;
                count++;
            }

            return count;
        }
        
        public ushort GetNumberOfMyNeighboursAndEmpty()
        {
            ushort count = 0;
            foreach (GoPawn node in Neighbours)
            {
                if (node == null || (node.isClosed && node.pawnType != this.pawnType)) continue;
                count++;
            }

            return count;
        }

        public ushort GetNumberOfNeighbours()
        {
            ushort count = 0;
            foreach (GoPawn node in Neighbours)
            {
                if (node == null) continue;
                count++;
            }

            return count;
        }
        
        public GoPawn GetFirstMyNeighbour()
        {
            foreach (GoPawn node in Neighbours)
            {
                if (node != null && (node.pawnType == this.pawnType)) return node;
            }

            return null;
        }

        public GoPawn GetBetterMyNeighbourOption()
        {
            int indexBestOption;
            GoPawn bestOption;
            List<GoPawn> tempOfMyNeighbours = new List<GoPawn>();
            
            tempOfMyNeighbours.AddRange(this.Neighbours.Where(e => (e != null && e.pawnType == this.pawnType)));
            indexBestOption = tempOfMyNeighbours.FindIndex(e =>
                e.lider.listOfConnectedNeighbours.Count ==
                tempOfMyNeighbours.Max(v => v.lider.listOfConnectedNeighbours.Count));
            bestOption = tempOfMyNeighbours[indexBestOption];

            for (int i = 0; i < tempOfMyNeighbours.Count; i++)
            {
                if (tempOfMyNeighbours.Count > 1 && i != indexBestOption && bestOption.lider.listOfConnectedNeighbours != tempOfMyNeighbours[i].lider.listOfConnectedNeighbours)
                {
                    bestOption.lider.listOfConnectedNeighbours.AddRange(tempOfMyNeighbours[i].lider.listOfConnectedNeighbours);
                    tempOfMyNeighbours[i].lider.listOfConnectedNeighbours = bestOption.lider.listOfConnectedNeighbours;
                    tempOfMyNeighbours[i].lider = bestOption.lider;
                }
            }

            return bestOption;
        }

        public bool CanLive()
        {
            foreach(GoPawn gp in lider.listOfConnectedNeighbours)
            {
                if(gp.GetNumberOfEmptyNeighbours() > 0) return true;
            }
            return false;
        }

        public void RemoveAllFromListOfConnectedNeighbours() => listOfConnectedNeighbours.ForEach(e => e.CloseMe());
    }

    public enum NodeType : byte
    {
        None,
        PawnA,
        PawnB
    }
}