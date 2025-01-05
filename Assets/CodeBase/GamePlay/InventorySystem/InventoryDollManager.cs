using System;
using UnityEngine;

namespace CodeBase.GamePlay.InventorySystem
{
    public class InventoryDollManager : MonoBehaviour
    {
        public event Action<ItemSo> OnItemEquipped;
        
        [SerializeField] private InventoryDollSlot[] slotsDoll;
        
        public void EquipItem(ItemSo item)
        {
            foreach (InventoryDollSlot slot in slotsDoll)
            {
                if (slot.DollTypeSlot == item.dollSlotType)
                {
                    slot.EquipItemDollSlot(item);
                    OnItemEquipped?.Invoke(item);
                }
            }
        }
    }
}