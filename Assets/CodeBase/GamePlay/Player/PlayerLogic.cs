using System;
using CodeBase.Configs.Player;
using CodeBase.GamePlay.Common;
using CodeBase.GamePlay.Player.ControllerCharacter;
using UnityEngine;

namespace CodeBase.GamePlay.Player
{
    [Serializable]
    public class CharacterAnimatorParametersName
    {
        public string NormolizeMovementX;
        public string NormolizeMovementZ;
        public string Sprint;
        public string Crouch;
        public string Fight;
        public string Ground;
        public string Jump;
        public string GroundSpeed;
        public string DistanceToGround;
    }

    [Serializable]
    public class AnimationCrossFadeParameters
    {
        public string Name;
        public float Duration;
    }
    
    public class PlayerLogic : MonoBehaviour
    {
        public Transform CameraFollowPaint => cameraFollowPaint;
        
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform cameraFollowPaint;
        
        
        [SerializeField] private Animator targetAnimator;
        [SerializeField] private CharacterAnimatorParametersName characterAnimatorParametersName;
        
        [Header("Animation Cross Fade Parameters")]
        [SerializeField] private AnimationCrossFadeParameters fallFade;
        [SerializeField] private float minDistanceToGroundByFall;
        [SerializeField] private AnimationCrossFadeParameters jumpIdleFade;
        [SerializeField] private AnimationCrossFadeParameters jumpMoveFade;

        private ThirdPersonCamera _thirdPersonCamera;
        private PlayerCharacterSetting _playerCharacterSettingConfig;
        
        private CharacterInputController _characterInputController;
        private CharacterMovementHuman _characterMovementHuman;
        private CharacterAnimationState _characterAnimationState;
        
        public void InitializeHeroPlayer(PlayerCharacterSetting playerCharacterSettingConfig,ThirdPersonCamera thirdPersonCamera)
        {
            _playerCharacterSettingConfig = playerCharacterSettingConfig;
            _thirdPersonCamera = thirdPersonCamera;
            
            _characterMovementHuman = new CharacterMovementHuman(characterController,playerTransform,_playerCharacterSettingConfig,_thirdPersonCamera);
            _characterInputController = new CharacterInputController(_characterMovementHuman, _thirdPersonCamera);
            _characterAnimationState = new CharacterAnimationState(_characterMovementHuman,characterController,targetAnimator,playerTransform,characterAnimatorParametersName,fallFade,jumpIdleFade,jumpMoveFade,minDistanceToGroundByFall);
            
            Initialize();
        }

        private void Initialize()
        {
            _characterMovementHuman.Enter();
            _characterInputController.Enter();
            _characterAnimationState.Enter();
        }
    }
}

