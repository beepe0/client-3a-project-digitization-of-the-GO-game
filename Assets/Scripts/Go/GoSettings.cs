using UnityEngine;
using UnityEngine.Serialization;

namespace Go
{
    public class GoSettings : MonoBehaviour
    {
        [Header("Pawns")]
        public GameObject prefabPawnA;
        public GameObject prefabPawnB;
        public GameObject prefabPositionAB;
        public float pawnsSize;

        [Header("Board")] 
        public GameObject prefabBoard;
        public Vector2Int boardSize;
        public float cellsSize;
    }
}