using JetBrains.Annotations;
using UnityEngine;

namespace CodeBase.GamePlay.Player
{
    public class PlayerHolderLinks : MonoBehaviour
    {
        public Transform SpawnPointForOneHandWeapon => spawnPointForOneHandWeapon;
        
        [SerializeField] private Transform spawnPointForOneHandWeapon;
    }
}
