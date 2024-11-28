using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GamePlay.InventorySystem
{
    public class InventorySlot : MonoBehaviour
    {
        public Image icon; 
        public Button removeButton;
        
        private ItemSo _itemSo;
        
        public void AddItem(ItemSo newItemSo)
        {
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
    }
}
