using CustomEditor.Attributes;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Go
{
    public class GoBoard : MonoBehaviour
    {
        public GoPawn[] pawns;
        [ReadOnlyInspector] public GameObject pawnCursor;
        [ReadOnlyInspector] public Vector2 offset;
        [ReadOnlyInspector] public Vector2 pawnOffset;
    }
}