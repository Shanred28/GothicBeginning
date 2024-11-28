using System;
using CodeBase.Configs.Player;
using CodeBase.GamePlay.Common;
using CodeBase.GamePlay.InventorySystem;
using CodeBase.GamePlay.Player.ControllerCharacter;
using CodeBase.GamePlay.Player.GUI.HUD;
using CodeBase.UI.MainUI.Windows;
using UnityEngine;
using VContainer;

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
        [SerializeField] private LayerMask interactibleLayerMask;
        
        
        [SerializeField] private Animator targetAnimator;
        [SerializeField] private CharacterAnimatorParametersName characterAnimatorParametersName;
        
        [Header("HUD")]
        [SerializeField] private UIHudPlayer uiHudPlayer;
        [SerializeField] private InventoryWindows inventoryWindows;
        
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
        private Inventory _inventory;
        private InteractionWorld _interactionWorldWorld;
        private InputSystem_Actions _inputKeyBoard;

        public void InitializeHeroPlayer(PlayerCharacterSetting playerCharacterSettingConfig,ThirdPersonCamera thirdPersonCamera)
        {
            _playerCharacterSettingConfig = playerCharacterSettingConfig;
            _thirdPersonCamera = thirdPersonCamera;

            _inputKeyBoard = new InputSystem_Actions();
            
            _inventory = new Inventory(inventoryWindows);
            _interactionWorldWorld = new InteractionWorld(_thirdPersonCamera.transform,interactibleLayerMask,uiHudPlayer);
            _characterMovementHuman = new CharacterMovementHuman(characterController,playerTransform,_playerCharacterSettingConfig,_thirdPersonCamera);
            _characterInputController = new CharacterInputController(_characterMovementHuman, _thirdPersonCamera,_inputKeyBoard,_interactionWorldWorld,_inventory,inventoryWindows);
            _characterAnimationState = new CharacterAnimationState(_characterMovementHuman,characterController,targetAnimator,playerTransform,characterAnimatorParametersName,fallFade,jumpIdleFade,jumpMoveFade,minDistanceToGroundByFall);
            
            Initialize();
        }

        private void Initialize()
        {
            _characterMovementHuman.Enter();
            _characterInputController.Enter();
            _characterAnimationState.Enter();
            
            _interactionWorldWorld.Enter();
        }
    }
}

