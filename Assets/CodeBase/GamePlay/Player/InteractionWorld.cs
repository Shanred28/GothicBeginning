using System;
using CodeBase.Common.Interface;
using CodeBase.GamePlay.InventorySystem;
using CodeBase.GamePlay.Player.GUI.HUD;
using UniRx;
using UnityEngine;

namespace CodeBase.GamePlay.Player
{
    public class InteractionWorld : ILogic
    {
        public Item TryItemInteract() => _itemInteractable;
        private Action<string> _onInteract => (s) => _hud.ShowInteractablePanel(s);
        
        private readonly LayerMask _layerMask;
        
        private readonly Transform _pointToRaycast;
        private readonly float _distanceRaycast = 50;
        private readonly UIHudPlayer _hud;
        private Collider _hitInteractable;
        private Item _itemInteractable;
        private bool _isInteracting;
        
        private readonly float _timerIntervalRaycast;
        private IDisposable _intervalTimer;

        public InteractionWorld(Transform pointToRaycast, LayerMask layerMask,UIHudPlayer hud, float intervalRaycast)
        {
            _pointToRaycast = pointToRaycast;
            _layerMask = layerMask;
            _hud = hud;
            _timerIntervalRaycast = intervalRaycast;
        }

        public void Enter()
        {
            _intervalTimer =Observable.Timer(TimeSpan.FromSeconds (_timerIntervalRaycast)).Repeat().Subscribe(_ => CheckInteractable());
        }

        public void Exit()
        {
            _intervalTimer.Dispose();
        }

        public void OnUpdate()
        {
            CheckInteractable();
        }

        private void CheckInteractable()
        {
            if (Physics.Raycast(_pointToRaycast.transform.position, _pointToRaycast.transform.forward, out RaycastHit hit, _distanceRaycast,_layerMask))
            {
                OnInteractable(hit);
            }
            else
            {
                NonInteractable();
            }
        }

        private void OnInteractable(RaycastHit hit)
        {
            if(hit.collider == _hitInteractable) return;
                
            _hitInteractable = hit.collider;
            _itemInteractable = _hitInteractable.GetComponent<Item>();
            _onInteract?.Invoke(_itemInteractable.itemSo.itemName);
            _isInteracting = true;
        }

        private void NonInteractable()
        {
            if (_isInteracting)
            {
                _hud.CleanInteractablePanel();
                _itemInteractable = null;
                _hitInteractable = null;
            }
            else
            {
                _isInteracting = false;
            }
        }
    }
}
