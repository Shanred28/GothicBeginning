using CodeBase.Common.Interface;
using CodeBase.Common.Ticker;
using CodeBase.Common.Ticker.Interfaces;
using CodeBase.GamePlay.Common;
using CodeBase.GamePlay.Player;
using UnityEngine;
using UniRx;

public class CharacterAnimationState : ILogic, ILateUpdateable
{
    private const float INPUT_CONTROL_LERP = 10f;

    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    
    private readonly PlayerInfoHolder _playerInfoHolder;
    
    private readonly CharacterController _targetCharacterController;
    private readonly CharacterMovementHuman _characterMovement;
    private readonly Transform _targetTransform;
    
    private readonly CharacterAnimatorParametersName _animatorParametersName;
    
    private readonly AnimationCrossFadeParameters _jumpIdleFade;
    private readonly AnimationCrossFadeParameters _jumpMoveFade;
    private readonly AnimationCrossFadeParameters _fallFade;
    
    private readonly Animator _targetAnimator;
    private readonly float _minDistanceToGroundByFall;
    private Vector3 _inputControl;
    
    public CharacterAnimationState(CharacterMovementHuman characterMovementController, CharacterController targetCharacterController, Animator targetAnimator, Transform targetTransform,CharacterAnimatorParametersName animatorParametersName,
        AnimationCrossFadeParameters jumpIdleFade,AnimationCrossFadeParameters jumpMoveFade,AnimationCrossFadeParameters fallFade,float minDistanceToGroundByFall)
    {
        _characterMovement = characterMovementController;
        _targetCharacterController = targetCharacterController;
        _targetAnimator = targetAnimator;
        _targetTransform = targetTransform;
        _animatorParametersName = animatorParametersName;

        _jumpIdleFade = jumpIdleFade;
        _jumpMoveFade = jumpMoveFade;
        _fallFade = fallFade;
        _minDistanceToGroundByFall = minDistanceToGroundByFall;
    }

    public void Enter()
    {
        SubscribeReactiveProperty();
        Ticker.RegisterLateUpdateable(this);
    }

    private void SubscribeReactiveProperty()
    {
        _characterMovement.IsSprint.Subscribe( v => _targetAnimator.SetBool(_animatorParametersName.Sprint, v)).AddTo(_disposables);
        _characterMovement.IsCrouch.Subscribe(v => _targetAnimator.SetBool(_animatorParametersName.Crouch, v)).AddTo(_disposables);
        _characterMovement.IsFight.Subscribe(v => _targetAnimator.SetBool(_animatorParametersName.Fight, v)).AddTo(_disposables);
        _characterMovement.IsGrounded.Subscribe(v => _targetAnimator.SetBool(_animatorParametersName.Ground, v)).AddTo(_disposables);
    }

    public void OnLateUpdate()
    {
         Vector3 movementSpeed = _targetTransform.InverseTransformDirection(_targetCharacterController.velocity);
        _inputControl = Vector3.MoveTowards(_inputControl, _characterMovement.DirectionControl, Time.deltaTime * INPUT_CONTROL_LERP);
        _targetAnimator.SetFloat(_animatorParametersName.NormolizeMovementX, _inputControl.x);
        _targetAnimator.SetFloat(_animatorParametersName.NormolizeMovementZ, _inputControl.z);

        Vector3 groundSpeed = _targetCharacterController.velocity;
        groundSpeed.y = 0;
        _targetAnimator.SetFloat(_animatorParametersName.GroundSpeed, groundSpeed.magnitude);

        if (_characterMovement.IsJump)
        {
            if (groundSpeed.magnitude <= 0.03f)
            {
                CrossFade(_jumpIdleFade);
            }

            if (groundSpeed.magnitude > 0.03f)
            {
                CrossFade(_jumpMoveFade);
            }
        }

        if (_characterMovement.IsGrounded.Value == false && _characterMovement.IsGrounded.Value == false)
        {
            _targetAnimator.SetFloat(_animatorParametersName.Jump, movementSpeed.y);

            if (movementSpeed.y < 0 && _characterMovement.DistanceToGround > _minDistanceToGroundByFall)
            {
                CrossFade(_fallFade);
            }
            _targetAnimator.SetFloat(_animatorParametersName.Jump, movementSpeed.y);
        }
        else
            _targetAnimator.SetFloat(_animatorParametersName.Jump, movementSpeed.y);

        _targetAnimator.SetFloat(_animatorParametersName.DistanceToGround, _characterMovement.DistanceToGround);
    }
    
    private void CrossFade(AnimationCrossFadeParameters parameters)
    {
        _targetAnimator.CrossFade(parameters.Name, parameters.Duration);
    }

    public void Exit()
    {
        _disposables.Clear();
        Ticker.UnregisterLateUpdateable(this);
    }
}
