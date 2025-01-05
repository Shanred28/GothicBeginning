using UnityEngine;

namespace CodeBase.GamePlay.InventorySystem
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class ItemSo : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public bool isStackable;
        public TypeDollSlot dollSlotType;
        public TypeItem itemType;
        public TypeWeapon weaponType;
        public TypeWornStaff staffType;
        public TypeJewelry jewelryType;
        public GameObject prefab;
    }
}
