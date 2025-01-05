using TMPro;
using UnityEngine;

namespace CodeBase.GamePlay.Player.GUI.HUD
{
    public class UIHudPlayer : MonoBehaviour
    {
        [SerializeField] private GameObject interactablePanel;
        [SerializeField] private TextMeshProUGUI textName;

        public void ShowInteractablePanel(string nameObject)
        {
            interactablePanel.SetActive(true);
            textName.text = nameObject;
        }

        public void CleanInteractablePanel()
        {
            interactablePanel.SetActive(false);
        }
    }
}
