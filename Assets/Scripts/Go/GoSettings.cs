using UnityEngine;
using UnityEngine.Serialization;

namespace Go
{
    public class GoSettings : MonoBehaviour
    {
        [Header("Pawns")]
        public GameObject prefabPawnA;
        public GameObject prefabPawnB;
        public GameObject prefabPawnCursor;
        public GameObject prefabPositionAB;
        public float pawnsSize;

        [Header("Board")] 
        public Material boardMaterial;
        public GameObject prefabBoard;
        public Vector2Int boardSize;
        [Range(2, 20)]
        public float cellsSize;
    }
}