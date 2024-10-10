using UnityEngine;

namespace Arphros.Cameras
{
    [ExecuteInEditMode]
    public class CameraMovement : CameraHost
    {
        public Camera mainCamera;

        [Header("Variables")]
        [AllowSavingState]
        public Vector3 pivotOffset = Vector3.zero;
        [AllowSavingState]
        public Vector3 targetRotation = new Vector3(45f, 60f, 0);
        [AllowSavingState]
        public float targetDistance = 20f;
        [AllowSavingState]
        [Range(0.001f, 10f)]
        public float smoothFactor = 1f;

        [AllowSavingState]
        public Vector3 localPositionOffset;
        [AllowSavingState]
        public Vector3 localEulerOffset;

        [Header("Shake")]
        [AllowSavingState]
        public float shakeDuration = 0f;
        [AllowSavingState]
        public float shakeAmount = 0.7f;
        [AllowSavingState]
        private float decreaseFactor = 1.0f;

        [Header("Compatibility")]
        public bool oldSystemActive;
        public CameraValues oldValues = new CameraValues();

        [Header("Misc")]
        public bool simulateInEditor = true;
        void Start()
        {
            if (mainCamera == null)
                return;

            if (target != null)
                transform.position = target.position + pivotOffset;
            else
                transform.position = Vector3.zero + pivotOffset;

            mainCamera.transform.localPosition = new Vector3(0, 0, -targetDistance);
            transform.eulerAngles = targetRotation;
            rotationTemp = targetRotation;
        }

        void Update()
        {
            if (mainCamera == null)
                return;

            if (Application.isPlaying)
            {
                /*if (LevelEditor.isPaused) return;
                if (References.Editor.playmode == LevelPlaymode.Play)
                    Process(false);
                else if (References.Editor.playmode == LevelPlaymode.Stopped)
                    Process(true);*/
                Process(false);
            }
            else
            {
                if (simulateInEditor)
                    Process(true);
            }
        }

        public override Camera GetMainCamera() => mainCamera;

        public void Process(bool quick)
        {
            Vector3 targetDist = new Vector3(0, 0, -targetDistance) + localPositionOffset;
            transform.eulerAngles = targetRotation;

            if (oldSystemActive)
                ProcessOldSystem(quick);

            if (quick)
            {
                if (target != null)
                    transform.position = target.position + pivotOffset;
                else
                    transform.position = Vector3.zero + pivotOffset;
            }
            else
            {
                if (target != null)
                    transform.position = Vector3.Slerp(transform.position, target.position + pivotOffset, smoothFactor * Time.deltaTime);
                else
                    transform.position = Vector3.Slerp(transform.position, Vector3.zero + pivotOffset, smoothFactor * Time.deltaTime);
            }

            if (shakeDuration > 0)
            {
                mainCamera.transform.localPosition = targetDist + Random.onUnitSphere * shakeAmount;
                shakeDuration -= decreaseFactor * Time.deltaTime;
            }
            else
            {
                mainCamera.transform.localPosition = targetDist;
                shakeDuration = 0f;
            }

            mainCamera.transform.localEulerAngles = localEulerOffset;
        }

        private Vector3 rotationTemp;
        private Vector3 rotationReference = Vector3.one;

        public void ProcessOldSystem(bool quick)
        {
            if (quick)
            {
                rotationTemp = oldValues.rotation;
                targetRotation = rotationTemp;

                targetDistance = oldValues.distance;
            }
            else
            {
                rotationTemp.x = Mathf.SmoothDampAngle(rotationTemp.x, oldValues.rotation.x, ref rotationReference.x, oldValues.rotationSmoothing);
                rotationTemp.y = Mathf.SmoothDampAngle(rotationTemp.y, oldValues.rotation.y, ref rotationReference.y, oldValues.rotationSmoothing);
                rotationTemp.z = Mathf.SmoothDampAngle(rotationTemp.z, oldValues.rotation.z, ref rotationReference.z, oldValues.rotationSmoothing);
                targetRotation = rotationTemp;

                targetDistance = Mathf.Lerp(targetDistance, oldValues.distance, Time.deltaTime * oldValues.smoothing);
            }
        }

        public override Transform GetTarget() => target;
    }

    [System.Serializable]
    public class CameraValues
    {
        public Vector3 rotation = new Vector3(30, 45, 0);
        public float distance = 40;

        public float smoothing = 1f;
        public float rotationSmoothing = 1f;
    }
}