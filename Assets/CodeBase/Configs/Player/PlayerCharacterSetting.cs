using UnityEngine;

namespace CodeBase.Configs.Player
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player/PlayerConfig", order = 1)]
    public class PlayerCharacterSetting : ScriptableObject
    {
        public string PathCameraPrefab;
        public string PathPlayerPrefabe;
        
        public float WalkSpeed;
        public float RunSpeed;
        public float JumpSpeed;
        public float AccelerationRate;
        public float SpeedSlider;
        public float DistanceForRayToGround;
        public float DistanceForRaySlopeSlide;
        
        public Vector3 defaultSpawnPosition;
    }
}
