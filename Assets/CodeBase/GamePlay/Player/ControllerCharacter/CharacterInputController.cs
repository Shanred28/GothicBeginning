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
        private InventoryWindows _inventoryWindows;


        public CharacterInputController(CharacterMovementHuman characterMovement, ThirdPersonCamera thirdPersonCamera,InputSystem_Actions inputKeyBoard,InteractionWorld interactionWorld,Inventory inventory,InventoryWindows inventoryWindows)
        {
            _characterMovement = characterMovement;
            _thirdPersonCamera = thirdPersonCamera;
            _inputKeyBoard = inputKeyBoard;
            _interactionWorld = interactionWorld;
            _inventory =inventory;
            _inventoryWindows = inventoryWindows;
        }

        public void Enter()
        {
            _inputKeyBoard.Player.Enable();
            _inputKeyBoard.UI.Enable();
            _inputKeyBoard.Player.Jump.started += OnJump;
            _inputKeyBoard.Player.Sprint.started += OnSprint;
            _inputKeyBoard.Player.Interact.started += OnInteract;
            _inputKeyBoard.UI.OpenInventary.started += OnOpenInventory;
            
            Ticker.RegisterUpdateable(this);
        }

        public void OnUpdate()
        {
            _characterMovement.TargetDirectionControl = new Vector3(_inputKeyBoard.Player.Move.ReadValue<Vector2>().x, 0,  _inputKeyBoard.Player.Move.ReadValue<Vector2>().y);
            _thirdPersonCamera.RotationControl = new Vector2(_inputKeyBoard.Player.Look.ReadValue<Vector2>().x, _inputKeyBoard.Player.Look.ReadValue<Vector2>().y);
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
            Debug.Log(_interactionWorld);
            
            if (_interactionWorld.TryItemInteract())
            {
                _inventory.AddItem(_interactionWorld.TryItemInteract().itemSo);
                _interactionWorld.TryItemInteract().PickUp();
            }
            else
            {
                Debug.Log("No Interact");
            }
        }

        private void OnOpenInventory(InputAction.CallbackContext obj)
        {
            _inventoryWindows.OpenInventory();
            _inputKeyBoard.Player.Disable();
        }

        public void Exit()
        {
            Ticker.UnregisterUpdateable(this);
            _inputKeyBoard.Player.Sprint.started -= OnSprint;
            _inputKeyBoard.Player.Jump.started -= OnJump;
        }
    }
}