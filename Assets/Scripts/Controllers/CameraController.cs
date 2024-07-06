using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        public Transform fixationPoint;
        [Range(0, 180)] public float cameraRotationRange = 60;
        public float distance;
        public bool lockRotation;


        private void Start()
        {
        
        }

        void Update()
        {
            if (lockRotation) return;
        }

        public void LockMouseMovement()
        {
        
        }
    }
}
