using UnityEngine;
using UnityEngine.Serialization;

namespace Go
{
    public class GoSettings : MonoBehaviour
    {
        [Header("Pawns")]
        public GameObject prefabPawnCursor;
        public GameObject prefabPawnAB;
        public Material materialPawnA, materialPawnB;
        public float pawnsSize;

        [Header("Board")] 
        public Material boardMaterial;
        public GameObject prefabBoard;
        public Vector2Int boardSize;
        [Range(2, 20)]
        public float cellsSize;
        [Range(1, 10)]
        public float cellsCoefSize;
    }
}