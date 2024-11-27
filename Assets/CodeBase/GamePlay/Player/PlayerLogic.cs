using CodeBase.GamePlay.Common;
using CodeBase.GamePlay.Player.ControllerCharacter;
using UnityEngine;

namespace CodeBase.GamePlay.Player
{
    public class PlayerLogic : MonoBehaviour
    {
        [SerializeField] private PlayerInfoHolder playerInfo;
        
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private ThirdPersonCamera thirdPersonCamera;
        
        [SerializeField] private Animator targetAnimator;

        private CharacterInputController _characterInputController;
        private CharacterMovementHuman _characterMovementHuman;
        private CharacterAnimationState _characterAnimationState;

        private void Start()
        {
            _characterMovementHuman = new CharacterMovementHuman(characterController,playerTransform);
            _characterInputController = new CharacterInputController(_characterMovementHuman, thirdPersonCamera);
            _characterAnimationState = new CharacterAnimationState(_characterMovementHuman,characterController,targetAnimator);
            
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

