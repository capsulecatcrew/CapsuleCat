using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

namespace Dialog.Scripts
{
    /// <summary>
    /// Shakes the camera when Shake is called.
    /// Place this script on a normal camera.
    /// </summary>
    public class ScreenShaker : MonoBehaviour
    {
        [SerializeField] [Tooltip("In Meters")]
        private float _shakeAmount = 0.5f;

        [SerializeField] private float stopShakeThreshold = 0.005f;

        private float _currShakeAmount = 0;
        private bool _isShaking = false;
        private Vector3 _cameraStartPos;

        /// <summary>
        /// Save orig cam position
        /// </summary>
        private void Start()
        {
            _cameraStartPos = transform.localPosition;
        }

        private void Update()
        {
            if (!_isShaking)
                return;

            // decrease shake amt
            _currShakeAmount =
                Mathf.Lerp(_currShakeAmount, 0, 0.02f); // idk what these magic nums are, copied off online

            // set random position
            transform.localPosition = _cameraStartPos + Random.onUnitSphere * _currShakeAmount;

            if (_currShakeAmount < stopShakeThreshold) // shaking is negligible
                _isShaking = false;
        }

        /// <summary>
        /// Call this using other functions to start the camera shake
        /// </summary>
        public void Shake()
        {
            _currShakeAmount = _shakeAmount;
            _isShaking = true;
        }
    }
}

#if UNITY_EDITOR
namespace Dialog.Scripts
{
    [CustomEditor(typeof(ScreenShaker))]
    public class ScreenShaker : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DialogBehaviour thisScript = (ScreenShaker) target;

            EditorGUILayout.LabelField("Debug Controls");
            if (GUILayout.Button("Make Camera Shake"))
            {
                thisScript.Shake();
            }
        }
    }
}
#endif
