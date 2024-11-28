using Cinemachine;
using CodeBase.Common.Ticker;
using CodeBase.Common.Ticker.Interfaces;
using UnityEngine;

namespace CodeBase.GamePlay.Player
{
    public class ThirdPersonCamera : MonoBehaviour, IUpdateable
    {
        [HideInInspector] public Vector2 RotationControl;
        public void TryMovePlayer(bool isMove) => _isMove = isMove;

        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float rotationSpeedHorizontal, rotationSpeedVertical;
        [SerializeField] private float minAngle ,maxAngle;
        
        private Transform _targetCameraFollowPoint;
        private Transform _playerTarget;
        private bool _isMove;
        
        private void OnDestroy()
        {
            Ticker.UnregisterUpdateable(this);
        }

        public void InitializeCamera(Transform targetCameraFollowPoint, Transform playerTarget)
        {
            _targetCameraFollowPoint = targetCameraFollowPoint;
            _playerTarget = playerTarget;
            virtualCamera.Follow = targetCameraFollowPoint;
            virtualCamera.LookAt = targetCameraFollowPoint;
            
            Ticker.RegisterUpdateable(this);
        }

        public void OnUpdate()
        {
            //Horizontal
            _targetCameraFollowPoint.rotation *= Quaternion.AngleAxis(RotationControl.x * rotationSpeedHorizontal, Vector3.up);

            //Vertical
            _targetCameraFollowPoint.rotation *= Quaternion.AngleAxis(-RotationControl.y * rotationSpeedVertical, Vector3.right);

            AngleCameraRotation();
        }
        
        private void AngleCameraRotation()
        {
            Vector3 angles = _targetCameraFollowPoint.localEulerAngles;
            angles.z = 0;

            if (angles.x > 180 && angles.x < maxAngle)
            {
                angles.x = maxAngle;
            }
            else if (angles.x < 180 && angles.x > minAngle)
            {
                angles.x = minAngle;
            }

            _targetCameraFollowPoint.localEulerAngles = angles;

            if (_isMove == false)
            {
                _targetCameraFollowPoint.localEulerAngles = new Vector3(angles.x, angles.y, 0);
            }
            else
            {
                _playerTarget.rotation = Quaternion.Euler(0, _targetCameraFollowPoint.eulerAngles.y, 0);
                _targetCameraFollowPoint.localEulerAngles = new Vector3(angles.x, 0, 0);
            }
        }
    }
}

