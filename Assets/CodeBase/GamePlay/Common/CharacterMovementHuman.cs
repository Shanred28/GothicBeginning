using CodeBase.Common.Interface;
using CodeBase.Common.Ticker;
using CodeBase.Common.Ticker.Interfaces;
using CodeBase.Configs.Player;
using CodeBase.GamePlay.Player;
using UniRx;
using UnityEngine;


namespace CodeBase.GamePlay.Common
{
    public class CharacterMovementHuman : IUpdateable,IFixedUpdateable, ILogic
    {
        public float DistanceToGround { get; private set; }

        public float CurrentSpeed => GetCurrentSpeedByState();
        public Vector3 TargetDirectionControl;

        #region PublicReactiveProperties

        public readonly BoolReactiveProperty IsSprint;
        public readonly BoolReactiveProperty IsCrouch;
        public readonly BoolReactiveProperty IsFight;
        public readonly BoolReactiveProperty IsGrounded;

        #endregion
        
        
        public bool IsJump;
      
        public Vector3 DirectionControl;
        private Vector3 _movementDirections;
        
        private readonly CharacterController _characterController;
        private readonly Transform _controllingModel;
        private float _accelerationRate;
        private float _walkSpeed;
        private float _runSpeed;
        private float _jumpSpeed;
        
        private bool _isSliding;
        private Vector3 _slopeSlideVelocity;
        private float _ySpeed;
        
        private float _distanceForRayToGround;
        private float _distanceForRaySlopeSlide;
        
        private readonly ThirdPersonCamera _thirdPersonCamera;
        private readonly PlayerCharacterSetting _playerCharacterSettingConfig;
        
        public CharacterMovementHuman(CharacterController characterController, Transform targetModelTransform,PlayerCharacterSetting playerCharacterSetting,ThirdPersonCamera thirdPersonCamera)
        {
            _characterController = characterController;
            _controllingModel = targetModelTransform;
            _playerCharacterSettingConfig = playerCharacterSetting;
            _thirdPersonCamera = thirdPersonCamera;
            
            IsSprint = new BoolReactiveProperty();
            IsCrouch = new BoolReactiveProperty();
            IsFight = new BoolReactiveProperty();
            IsGrounded = new BoolReactiveProperty();
        }
        
        public void Enter()
        {
            SetPropertyConfig();

            Ticker.RegisterUpdateable(this);
            Ticker.RegisterFixedUpdateable(this);
        }
        
        private void SetPropertyConfig()
        {
            _walkSpeed = _playerCharacterSettingConfig.WalkSpeed;
            _runSpeed = _playerCharacterSettingConfig.RunSpeed;
            _jumpSpeed = _playerCharacterSettingConfig.JumpSpeed;
            _accelerationRate = _playerCharacterSettingConfig.AccelerationRate;
            _ySpeed = _playerCharacterSettingConfig.SpeedSlider;
            _distanceForRayToGround = _playerCharacterSettingConfig.DistanceForRayToGround;
            _distanceForRaySlopeSlide = _playerCharacterSettingConfig.DistanceForRaySlopeSlide;
        }


        public void OnUpdate()
        {
            SetSlopeSlide();
            UpdateDistanceToGround();
            TargetControlMove();
            CheckMove();
        }

        public void OnFixedUpdate()
        {
            Move();
        }
        
        public void Exit()
        {
            Ticker.UnregisterUpdateable(this);
            Ticker.UnregisterFixedUpdateable(this);
        }

        private void Move()
        {
            if (IsGrounded.Value && _isSliding == false)
            {
                _movementDirections = DirectionControl * GetCurrentSpeedByState();
                if (IsJump)
                {
                    _movementDirections.y = _jumpSpeed;
                    IsJump = false;
                }

                _movementDirections = _controllingModel.TransformDirection(_movementDirections);
                _movementDirections += Physics.gravity * Time.fixedDeltaTime;
                
                
                _characterController.Move(_movementDirections * (_accelerationRate * Time.fixedDeltaTime));
            }
            else
            {
                _movementDirections += Physics.gravity * Time.fixedDeltaTime;
                _characterController.Move(_movementDirections * Time.fixedDeltaTime);
            }
            
            if (_isSliding)
            {
                Vector3 velocity = _slopeSlideVelocity;
                velocity.y = _ySpeed;

                _characterController.Move(velocity * Time.deltaTime);
            }
        }

        private void CheckMove() => _thirdPersonCamera.TryMovePlayer(_characterController.velocity.magnitude  > 0.1f);

        private void TargetControlMove()
        {
            DirectionControl = Vector3.MoveTowards(DirectionControl, TargetDirectionControl, Time.deltaTime * _accelerationRate);

            if (_slopeSlideVelocity == Vector3.zero)
            {
                _isSliding = false;
            }
            else
                _isSliding = true;
            
        }
        public void Sprint()
        {
            if (IsGrounded.Value == false) return;
            if (IsCrouch.Value) return;

            IsSprint.Value = !IsSprint.Value;
        }
        
        public void Jump()
        {
            if (IsGrounded.Value == false) return;
            if (IsFight.Value || IsCrouch.Value) return;

            IsJump = true;
        }

        public void SetFightState()
        {
            if (IsFight.Value) return;
            
            IsFight.Value = true;
        }

        private float GetCurrentSpeedByState()
        {
            return IsSprint.Value ? _runSpeed : _walkSpeed;
        }
        
        private void SetSlopeSlide()
        {
            if (Physics.Raycast(_controllingModel.position, Vector3.down, out RaycastHit hitInfo, _distanceForRaySlopeSlide))
            {
                float angle = Vector3.Angle(hitInfo.normal,Vector3.up);

                if (angle >= _characterController.slopeLimit)
                {
                    _slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, _ySpeed, 0), hitInfo.normal);
                    return;
                }   
            }

            if (_isSliding)
            { 
                //TODO
                _slopeSlideVelocity -= _slopeSlideVelocity * (Time.deltaTime * 3);

                if (_slopeSlideVelocity.magnitude > 1)
                {
                    return;
                }
            }

            _slopeSlideVelocity = Vector3.zero;
        }
        private void UpdateDistanceToGround()
        {
            if (Physics.Raycast(_controllingModel.position, Vector3.down, out RaycastHit hit, _distanceForRayToGround))
            {
                DistanceToGround = Vector3.Distance(_controllingModel.position, hit.point);
            }
            
            IsGrounded.Value = _characterController.isGrounded || DistanceToGround < 0.09f;
        }
    }
}


