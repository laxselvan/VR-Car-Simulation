using UnityEngine;
using UnityEngine.Assertions;

namespace Oculus.Interaction.Samples
{
    public class LookAtTarget : MonoBehaviour
    {
        [SerializeField]
        private Transform _toRotate; // The transform of the object to rotate (e.g., the camera)

        [SerializeField]
        private Transform _target; // The target transform to look at

        protected virtual void Start()
        {
            this.AssertField(_toRotate, nameof(_toRotate));
            this.AssertField(_target, nameof(_target));
        }

        /// <summary>
        /// Rotates the _toRotate transform to look at the _target.
        /// </summary>
        public void RotateToTarget()
        {
            if (_toRotate == null || _target == null)
            {
                Debug.LogWarning("Either _toRotate or _target is not assigned.");
                return;
            }

            Vector3 dirToTarget = (_target.position - _toRotate.position).normalized;
            _toRotate.LookAt(_toRotate.position - dirToTarget, Vector3.up);
        }
    }
}
