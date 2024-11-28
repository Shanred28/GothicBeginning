using TMPro;
using UnityEngine;

namespace CodeBase.GamePlay.Player.GUI.HUD
{
    public class UIHudPlayer : MonoBehaviour
    {
        [SerializeField] private GameObject interactiblePanel;
        [SerializeField] private TextMeshProUGUI textName;

        public void ShowInteractiblePanel(string nameObject)
        {
            interactiblePanel.SetActive(true);
            textName.text = nameObject;
        }

        public void CleanInteractiblePanel()
        {
            interactiblePanel.SetActive(false);
        }
    }
}
