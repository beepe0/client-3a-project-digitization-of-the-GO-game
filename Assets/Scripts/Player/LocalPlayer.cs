using Go;
using Singleton;
using UnityEngine;

namespace Player
{
    public class LocalPlayer : Singleton<LocalPlayer>
    {
        public Camera localPlayerCamera;
        public GoGame mainGame;

        private Ray _rayFromCursor;
        private RaycastHit _result;
        private bool _didRayHitSomething;

        private void Update()
        {
            _didRayHitSomething = MouseRaycast(out _rayFromCursor, out _result);
            InputFrom();
        }

        private void InputFrom()
        {
            if ((mainGame == null && _didRayHitSomething) && !_result.collider.gameObject.TryGetComponent(out mainGame))
                return;
            if (mainGame == null) return;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mainGame.PawnPass();
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && _didRayHitSomething)
            {
                Debug.DrawLine(_rayFromCursor.origin, _result.point, (mainGame.Board.numberOfSteps % 2 == 0) ? Color.red : Color.green, 10);
                mainGame.PawnInitialization(new Vector2(_result.point.x, _result.point.z));
            }
            else if(_didRayHitSomething)
            {
                Debug.DrawLine(_rayFromCursor.origin, _result.point, Color.blue, 0);
                mainGame.ShowCursor(new Vector2(_result.point.x, _result.point.z));
            }
        }
        private bool MouseRaycast(out Ray r, out RaycastHit hitInfo) => Physics.Raycast(r = localPlayerCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, 100);
    }
}