using UnityEngine;

namespace Scenes.TurretTargeting.Scripts
{
    [ExecuteInEditMode]
    public class TurretAimingController : MonoBehaviour
    {
        public Transform target;
        public Transform gun;
        public Vector2 turretAngleLimits = new Vector2(-90f, 90f);
        public Vector2 gunAngleLimits = new Vector2(0, 45f);
        [Range(0, 1)] public float rotationSpeed = 0.5f;
        private float rotationSlerpFactor;

        private void OnEnable()
        {
            transform.localRotation = Quaternion.identity;
            gun.localRotation = Quaternion.identity;
        
            const float slerpMin = 1f;
            const float slerpMax = 100f;
            rotationSlerpFactor = 1f / (slerpMin + (slerpMax - slerpMin) * (1f - rotationSpeed));
        }

        private void OnDisable()
        {
            transform.localRotation = Quaternion.identity;
            if (gun)
            {
                gun.transform.localRotation = Quaternion.identity;
            }
        }

        private void Update()
        {
            if (Application.isEditor && !Application.isPlaying)
            {
                FixedUpdate();
            }
        }

        private void FixedUpdate()
        {
            if (!target)
            {
                return;
            }

            var baseTarget = (target.position - transform.position).normalized;
            Rotate(transform, baseTarget, Vector3.forward, Vector3.up, turretAngleLimits);
        
            baseTarget = (target.position - gun.position).normalized;
            Rotate(gun, baseTarget, -Vector3.up, Vector3.left, gunAngleLimits);
        
            Debug.DrawRay(gun.position, -gun.up * 40f, Color.green);
        }
    
        private void Rotate(
            Transform subTransform,
            Vector3 targetDirection, 
            Vector3 localForwardAxis,
            Vector3 localRotationAxis,
            Vector2 angleLimits)
        {
            targetDirection = subTransform.parent.InverseTransformDirection(targetDirection);
            var projectedTarget = Vector3.ProjectOnPlane(targetDirection, localRotationAxis).normalized;
            var angle = Vector3.SignedAngle(localForwardAxis, projectedTarget, localRotationAxis);
            angle = Mathf.Clamp(angle, angleLimits.x, angleLimits.y);

            subTransform.localRotation = Quaternion.Slerp(subTransform.localRotation,
                Quaternion.AngleAxis(angle, localRotationAxis), rotationSlerpFactor);
        }
    }
}
