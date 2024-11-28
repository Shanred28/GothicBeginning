using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Common
{
    public abstract class WindowsBase : MonoBehaviour
    {
        public event Action Cleanuped;

        [SerializeField] private Button closeButton;
        [SerializeField] private TMP_Text titleText;

        private void Awake()
        {
            OnAwake();
            closeButton?.onClick.AddListener(Close);
        }

        private void OnDestroy()
        {
            closeButton?.onClick?.RemoveListener(Close);
            OnCleanup();
            Cleanuped?.Invoke();
        }

        public void Close()
        {
            OnClose();
        }

        public void SetTitle(string title)
        {
            if(title == null) return;
            
            titleText.text = title;
        }

        protected virtual void OnAwake() { }
        protected virtual void OnClose() { }
        protected virtual void OnCleanup() { }
    }
}

