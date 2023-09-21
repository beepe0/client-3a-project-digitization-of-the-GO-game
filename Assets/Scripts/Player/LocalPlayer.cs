using UnityEngine;

namespace Player
{
    public class LocalPlayer : Singleton<LocalPlayer>
    {
        public Camera localPlayerCamera;

        protected override void OnAwake()
        {
     
        }
    }
}