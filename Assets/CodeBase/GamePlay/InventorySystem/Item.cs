using Lean.Pool;
using UnityEngine;

namespace CodeBase.GamePlay.InventorySystem
{
    public class Item : MonoBehaviour
    {
        public ItemSo itemSo;

        public ItemSo PickUp()
        {
            LeanPool.Despawn(gameObject);
            return itemSo;
        }
    }
}
