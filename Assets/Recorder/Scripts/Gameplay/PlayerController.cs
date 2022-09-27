using UnityEngine;

namespace Recorder.Scripts.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _gravityTarget;
        [SerializeField] private float _gravityForce;
        [SerializeField] private Rigidbody _rb;

        private void Start()
        {
            Application.targetFrameRate = 60;

        }

        private void FixedUpdate()
        {
            ProcessGravity();
        }

        private void ProcessGravity()
        {
            Vector3 diff = transform.position - _gravityTarget.position;
            _rb.AddForce(-diff.normalized * _gravityForce * _rb.mass);
        }
    }
}