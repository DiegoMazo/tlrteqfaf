using UnityEngine;

namespace CromisDev.SimplePanelSystem
{
    public enum PanelType
    {   /*In Game Panels*/
        IN_GAME_MAIN,
        IN_GAME_GAMEOVER,
        IN_GAME_PAUSE,

        /*Menu Panels*/
        MENU_MAIN,
        NEW_GAME
    }

    public class PanelManager : MonoBehaviour
    {
        [System.Serializable]
        private struct PanelEntry
        {
            public PanelType panelType;
            public Panel panel;
        }

        [SerializeField] private PanelEntry[] panels;

        private void Awake()
        {
            foreach (var entry in panels)
            {
                entry.panel.Initialize();
            }
        }
        public void ShowPanel(PanelType type)
        {
            foreach (var entry in panels)
            {
                if (entry.panelType == type)
                {
                    entry.panel.Show();
                }
                else
                {
                    entry.panel.Hide();
                }
            }
        }
    }
}
