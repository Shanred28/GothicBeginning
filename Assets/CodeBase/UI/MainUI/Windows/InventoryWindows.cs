using System.Collections.Generic;
using CodeBase.GamePlay.InventorySystem;
using CodeBase.UI.Common;
using Lean.Pool;
using UnityEngine;

namespace CodeBase.UI.MainUI.Windows
{
    public class InventoryWindows : WindowsBase
    {
        public Transform itemsParent;

        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private InventorySlot slotsPrefab;
        [SerializeField] private InventoryDollManager dollManager;

        private Inventory _inventory;
        private List<InventorySlot> _slots = new List<InventorySlot>();

        public void OpenInventory()
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }

        public void AddItem(ItemSo itemSo)
        {
            InventorySlot slot = LeanPool.Spawn(slotsPrefab,itemsParent);
            slot.AddItem(itemSo,dollManager);
            _slots.Add(slot);
        }
    }
}
