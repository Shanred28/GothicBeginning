using System;
using CodeBase.Common.Interface;
using CodeBase.Common.Ticker;
using CodeBase.Common.Ticker.Interfaces;
using CodeBase.GamePlay.InventorySystem;
using CodeBase.GamePlay.Player.GUI.HUD;
using UnityEngine;

namespace CodeBase.GamePlay.Player
{
    public class InteractionWorld : ILogic, IUpdateable
    {
        public Item TryItemInteract() => _itemInteractible;
        private Action<string> _onInteract => (s) => _hud.ShowInteractiblePanel(s);
        
        private readonly LayerMask _layerMask;
        
        private readonly Transform _pointToRaycast;
        private readonly float _distanceRaycast = 50;
        private UIHudPlayer _hud;
        private Collider _hitInteractible;
        private Item _itemInteractible;

        public InteractionWorld(Transform pointToRaycast, LayerMask layerMask,UIHudPlayer hud)
        {
            _pointToRaycast = pointToRaycast;
            _layerMask = layerMask;
            _hud = hud;
        }

        public void Enter()
        {
            Ticker.RegisterUpdateable(this);
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdate()
        {
            if (Physics.Raycast(_pointToRaycast.transform.position, _pointToRaycast.transform.forward, out RaycastHit hit, _distanceRaycast,_layerMask))
            {
                if(hit.collider == _hitInteractible) return;
                
                _hitInteractible = hit.collider;
                _itemInteractible = _hitInteractible.GetComponent<Item>();
                _onInteract?.Invoke(_itemInteractible.itemSo.itemName);
            }
            else
            {
                if(!_hitInteractible) return;
                   
                _hud.CleanInteractiblePanel();
                _itemInteractible = null;
                _hitInteractible = null;
            }
        }
    }
}
