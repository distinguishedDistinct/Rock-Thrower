using Platformer.Mechanics;
using Platformer.UI;
using UnityEngine;
// Removed using UnityEngine.InputSystem; as it's no longer used

namespace Platformer.UI
{
    public class MetaGameController : MonoBehaviour
    {
        public MainUIController mainMenu;
        public Canvas[] gamePlayCanvasii;
        public GameController gameController;

        bool showMainCanvas = false;
        // private InputAction m_MenuAction; <-- REMOVED

        // The commented OnEnable() method is gone, as it's not needed.

        public void ToggleMainMenu(bool show)
        {
            if (this.showMainCanvas != show)
            {
                _ToggleMainMenu(show);
            }
        }

        void _ToggleMainMenu(bool show)
        {
            if (show)
            {
                Time.timeScale = 0;
                // Added null checks to prevent crashes if links are accidentally broken
                if (mainMenu != null) mainMenu.gameObject.SetActive(true); 
                foreach (var i in gamePlayCanvasii) 
                    if (i != null) i.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                if (mainMenu != null) mainMenu.gameObject.SetActive(false);
                foreach (var i in gamePlayCanvasii) 
                    if (i != null) i.gameObject.SetActive(true);
            }
            this.showMainCanvas = show;
        }

        // The crashing Update() function is permanently REMOVED.
    }
}