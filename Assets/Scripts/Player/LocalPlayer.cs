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
                    Debug.DrawLine(r.origin, hitInfo.point, (mainGame.goBoard.numberOfSteps % 2 == 0) ? Color.red : Color.green, 10);
                    mainGame.Rules.PawnInitialization((mainGame.goBoard.numberOfSteps % 2 == 0) ? NodeType.PawnA : NodeType.PawnB, new Vector2(hitInfo.point.x, hitInfo.point.z));
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("PASS");
                    mainGame.Rules.PawnPass();
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