using UnityEngine;

namespace Scenes.TurretTargeting.Scripts
{
    public class ChaoticTargetController : MonoBehaviour
    {
        public Collider restartTrigger;
        public float randomOffset = 20f;
        public float initialImpulse = 100f;
        
        private Vector3 originalPosition;
        private Rigidbody rigidBody;
        private bool firstRun = true;

        private void OnEnable()
        {
            originalPosition = transform.position;
            rigidBody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (firstRun)
            {
                ApplyInitForce();
                firstRun = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == restartTrigger)
            {
                rigidBody.MovePosition(originalPosition + Vector3.right * randomOffset);
                ApplyInitForce();
            }
        }

        private void ApplyInitForce()
        {
            var velocity = rigidBody.velocity;
            var force = new Vector3(0, 0, 
                -Mathf.Clamp(initialImpulse + velocity.z*rigidBody.mass, 0, float.PositiveInfinity));
            rigidBody.AddForce(force, ForceMode.Impulse);
        }
    }
}