using Lean.Pool;
using UnityEngine;

namespace CodeBase.GamePlay.InventorySystem
{
    public class Item : MonoBehaviour
    {
        public ItemSo itemSo;
        public Transform pointForInteract;

        public ItemSo PickUp()
        {
            LeanPool.Despawn(gameObject);
            return itemSo;
        }
    }
}
