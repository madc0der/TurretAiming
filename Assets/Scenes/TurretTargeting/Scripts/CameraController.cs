using Unity.Mathematics;
using UnityEngine;

namespace Scenes.TurretTargeting.Scripts
{
    public class CameraController : MonoBehaviour
    {
        public float cameraOffset = 10f;
        public float cameraHeight = 10f;
        public Transform target;
        public Transform turret;

        private Camera gameCamera;

        private void OnEnable()
        {
            gameCamera = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            var turretPosition = turret.transform.position;
            var dir = (target.transform.position - turretPosition);
            dir.y = 0;
            dir.Normalize();
            var cameraTransform = gameCamera.transform;
            cameraTransform.position = turretPosition + dir * cameraOffset + Vector3.up * cameraHeight;
            cameraTransform.rotation = quaternion.LookRotation(dir, Vector3.up); 
        }
    }
}