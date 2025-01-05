using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.GamePlay.InventorySystem
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler
    {
        private const float DOUBLE_CLICK_TRESHOLD = 0.25f;

        public Image bgImage;
        public Image icon; 
        public Button removeButton;
        
        private ItemSo _itemSo;

        private float _lastClickTime;
        private InventoryDollManager _inventoryDollManager;

        public void IntiSlotInventory()
        {
            
        }
        
        public void AddItem(ItemSo newItemSo, InventoryDollManager inventoryDollManager)
        {
            _inventoryDollManager = inventoryDollManager;
            _itemSo = newItemSo;
            icon.sprite = _itemSo.icon;
            icon.enabled = true;
            removeButton.interactable = true;
        }
        
        public void ClearSlot()
        {
            _itemSo = null;
            icon.sprite = null;
            icon.enabled = false;
            removeButton.interactable = false;
        }
        
        public void OnRemoveButton()
        {
           
        }
        
        public void UseItem()
        {
            if (_itemSo != null)
            {
                // Вызываем действие предмета
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Time.time - _lastClickTime < DOUBLE_CLICK_TRESHOLD)
            {
                OnDoubleClick();
            } 
            
            _lastClickTime = Time.time;
        }

        private void OnDoubleClick()
        {
            _inventoryDollManager.EquipItem(_itemSo);
            bgImage.color = Color.green;
        }
    }
}
