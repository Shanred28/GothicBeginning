using System;
using CodeBase.Configs.Player;
using CodeBase.GamePlay.Common;
using CodeBase.GamePlay.InventorySystem;
using CodeBase.GamePlay.Player.Animation;
using CodeBase.GamePlay.Player.ControllerCharacter;
using CodeBase.GamePlay.Player.GUI.HUD;
using CodeBase.UI.MainUI.Windows;
using Lean.Pool;
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
        
        [Header("Inventory")]
        [SerializeField] private InventoryDollManager inventoryDollManager;
        
        [Header("Links point spawn item")]
        [SerializeField] private GameObject spawnItemPointForTwoHandedWeapon;

        [SerializeField] private GameObject pointRightArmHand;

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
            _interactionWorldWorld = new InteractionWorld(_thirdPersonCamera.transform,interactibleLayerMask,uiHudPlayer,_playerCharacterSettingConfig.timerIntervalInteractionRaycast);
            _characterMovementHuman = new CharacterMovementHuman(characterController,playerTransform,_playerCharacterSettingConfig,_thirdPersonCamera);
            _characterInputController = new CharacterInputController(_characterMovementHuman, _thirdPersonCamera,_inputKeyBoard,_interactionWorldWorld,_inventory,inventoryWindows, this);
            _characterAnimationState = new CharacterAnimationState(_characterMovementHuman,characterController,targetAnimator,playerTransform,characterAnimatorParametersName,fallFade,jumpIdleFade,jumpMoveFade,minDistanceToGroundByFall);
            
            Initialize();
        }

        private void Initialize()
        {
            _characterMovementHuman.Enter();
            _characterInputController.Enter();
            _characterAnimationState.Enter();
            
            _interactionWorldWorld.Enter();
            
            inventoryDollManager.OnItemEquipped += OnItemEquipped;
        }

        private Item _weapon;
        private void OnItemEquipped(ItemSo obj)
        {
           _weapon = LeanPool.Spawn(obj.prefab, spawnItemPointForTwoHandedWeapon.transform.position, spawnItemPointForTwoHandedWeapon.transform.rotation, spawnItemPointForTwoHandedWeapon.transform).GetComponent<Item>();
        }

        public void ArmDisarmEquippedWeapon()
        {
            _characterMovementHuman.SetFightState();
            _weapon.transform.SetParent(pointRightArmHand.transform);
            
            _weapon.transform.localPosition = Vector3.zero;

            _weapon.transform.localPosition =
                pointRightArmHand.transform.InverseTransformPoint(_weapon.pointForInteract.position);
            
            _weapon.transform.localRotation = Quaternion.Inverse(pointRightArmHand.transform.rotation) * pointRightArmHand.transform.rotation;
        }
    }
}

