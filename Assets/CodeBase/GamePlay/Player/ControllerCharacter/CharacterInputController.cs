using CodeBase.Common.Interface;
using CodeBase.Common.Ticker;
using CodeBase.Common.Ticker.Interfaces;
using CodeBase.GamePlay.Common;
using CodeBase.GamePlay.InventorySystem;
using CodeBase.GamePlay.Player.GUI.HUD;
using CodeBase.UI.MainUI.Windows;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.GamePlay.Player.ControllerCharacter
{
    public class CharacterInputController : ILogic, IUpdateable
    {
        private readonly InputSystem_Actions _inputKeyBoard;
        private readonly CharacterMovementHuman _characterMovement;
        private readonly ThirdPersonCamera _thirdPersonCamera;
        private readonly InteractionWorld _interactionWorld;
        private readonly Inventory _inventory;
        private readonly InventoryWindows _inventoryWindows;
        private readonly PlayerLogic _playerLogic;


        public CharacterInputController(CharacterMovementHuman characterMovement, ThirdPersonCamera thirdPersonCamera,
            InputSystem_Actions inputKeyBoard, InteractionWorld interactionWorld, Inventory inventory,
            InventoryWindows inventoryWindows, PlayerLogic playerLogic)
        {
            _characterMovement = characterMovement;
            _thirdPersonCamera = thirdPersonCamera;
            _inputKeyBoard = inputKeyBoard;
            _interactionWorld = interactionWorld;
            _inventory = inventory;
            _inventoryWindows = inventoryWindows;
            _playerLogic = playerLogic;
        }

        public void Enter()
        {
            _inputKeyBoard.Player.Enable();
            _inputKeyBoard.UI.Enable();
            _inputKeyBoard.Player.Jump.started += OnJump;
            _inputKeyBoard.Player.Sprint.started += OnSprint;
            _inputKeyBoard.Player.Interact.started += OnInteract;
            _inputKeyBoard.Player.ArmDisarm_weapon.started += OnArmDisarmWeapon;
            _inputKeyBoard.UI.OpenInventary.started += OnOpenInventory;

            Ticker.RegisterUpdateable(this);
        }

        private void OnArmDisarmWeapon(InputAction.CallbackContext obj)
        {
            _playerLogic.ArmDisarmEquippedWeapon();
        }

        public void OnUpdate()
        {
            _characterMovement.TargetDirectionControl = new Vector3(_inputKeyBoard.Player.Move.ReadValue<Vector2>().x,
                0, _inputKeyBoard.Player.Move.ReadValue<Vector2>().y);
            _thirdPersonCamera.RotationControl = new Vector2(_inputKeyBoard.Player.Look.ReadValue<Vector2>().x,
                _inputKeyBoard.Player.Look.ReadValue<Vector2>().y);
        }

        private void OnSprint(InputAction.CallbackContext obj)
        {
            _characterMovement.Sprint();
        }

        private void OnJump(InputAction.CallbackContext obj)
        {
            _characterMovement.Jump();
        }

        private void OnInteract(InputAction.CallbackContext obj)
        {
            if (!_interactionWorld.TryItemInteract()) return;

            _inventory.AddItem(_interactionWorld.TryItemInteract().itemSo);
            _interactionWorld.TryItemInteract().PickUp();
        }

        private void OnOpenInventory(InputAction.CallbackContext obj)
        {
            _inventoryWindows.OpenInventory();

            if (_inputKeyBoard.Player.enabled)
            {
                _inputKeyBoard.Player.Disable();
            }
            else
            {
                _inputKeyBoard.Player.Enable();
            }
        }

        public void Exit()
        {
            Ticker.UnregisterUpdateable(this);
            _inputKeyBoard.Player.Sprint.started -= OnSprint;
            _inputKeyBoard.Player.Jump.started -= OnJump;
        }
    }
}