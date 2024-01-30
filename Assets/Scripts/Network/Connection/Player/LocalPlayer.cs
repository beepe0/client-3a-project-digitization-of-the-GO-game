using Go;
using Network.UnityClient;
using Singleton;
using UnityEngine;

namespace Network.Connection.Player
{
    public class LocalPlayer : UNetworkClient
    {
        [SerializeField] private Camera _localPlayerCamera;
        
        private bool _didRayHitSomething;

        private Ray _rayFromCursor;
        private RaycastHit _result;
        //t
        private GoGame _mainGame;
        //t
        
        private void Update()
        {
            if (CurrentConnection.Index == Index) InputFrom();
        }
        private void InputFrom()
        {
            _didRayHitSomething = MouseRaycast(out _rayFromCursor, out _result);
            if ((_mainGame == null && _didRayHitSomething) && !_result.collider.gameObject.TryGetComponent(out _mainGame))
                return;
            if (_mainGame == null) return;
            
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Space))
            {
                _mainGame.PawnPass();
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && _didRayHitSomething)
            {
                Debug.DrawLine(_rayFromCursor.origin, _result.point, (_mainGame.Board.numberOfSteps % 2 == 0) ? Color.red : Color.green, 10);
                _mainGame.InitializingPawn(new Vector2(_result.point.x, _result.point.z));
            }
            else if(_didRayHitSomething)
            {
                Debug.DrawLine(_rayFromCursor.origin, _result.point, Color.blue, 0);
                _mainGame.ShowCursor(new Vector2(_result.point.x, _result.point.z));
            }
        }
        private bool MouseRaycast(out Ray r, out RaycastHit hitInfo) => Physics.Raycast(r = _localPlayerCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, 100);
    }
}