using Go;
using Singleton;
using UnityEngine;

namespace Player
{
    public class LocalPlayer : Singleton<LocalPlayer>
    {
        public Camera localPlayerCamera;

        public GoGame mainGame;

        private void Update() => MouseRaycast();
        
        private void MouseRaycast()
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray r = localPlayerCamera.ScreenPointToRay(mousePosition);
        
            RaycastHit hitInfo;
            
            if (Physics.Raycast(r, out hitInfo, 100))
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Debug.DrawLine(r.origin, hitInfo.point, Color.red, 10);
                    mainGame.Rules.PawnInitialization(NodeType.PawnA, new Vector2(hitInfo.point.x, hitInfo.point.z));
                }
                else if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Debug.DrawLine(r.origin, hitInfo.point, Color.red, 10);
                    mainGame.Rules.PawnInitialization(NodeType.PawnB, new Vector2(hitInfo.point.x, hitInfo.point.z));
                }
                else
                {
                    Debug.DrawLine(r.origin, hitInfo.point, Color.blue, 0);
                    mainGame.ShowCursor(new Vector2(hitInfo.point.x, hitInfo.point.z));
                }
                
            }
        }
    }
}