using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.GamePlay.InventorySystem
{
    public class InventoryDollSlot : MonoBehaviour, IPointerEnterHandler
    {
        public TypeDollSlot DollTypeSlot => doll;
        
        [SerializeField] private TypeDollSlot doll;
        [SerializeField] private Image imageIcon;
        
        private ItemSo _itemSO;
        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("OnPointerEnter");
        }

        public void EquipItemDollSlot(ItemSo itemSO)
        {
            imageIcon.sprite = itemSO.icon;
        }
    }
}
