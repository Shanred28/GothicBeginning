using System.Collections.Generic;
using CodeBase.Common.Interface;
using CodeBase.UI.MainUI.Windows;
using UnityEngine;

namespace CodeBase.GamePlay.InventorySystem
{
    public class Inventory : ILogic
    {        
        public delegate void OnItemChanged();
        public OnItemChanged OnItemChangedCallback;

        private List<ItemSo> _items = new List<ItemSo>();
        private bool _isChangedInventory;
        private readonly InventoryWindows _inventoryWindows;


        public Inventory(InventoryWindows inventoryWindows)
        {
            _inventoryWindows = inventoryWindows;
        }

        public void Enter()
        {
            Debug.Log("Enter Inventory");
        }

        public void Exit()
        {
            Debug.Log("Exit Inventory");
        }
        
        public void AddItem(ItemSo itemSo)
        {
            _items.Add(itemSo);
            OnItemChangedCallback?.Invoke();
            _inventoryWindows.AddItem(itemSo);
            _isChangedInventory = true;
        }
        
        public void Remove(ItemSo itemSo)
        {
            _items.Remove(itemSo);
            OnItemChangedCallback?.Invoke(); 
            
            _isChangedInventory = true;
        }
    }
}
