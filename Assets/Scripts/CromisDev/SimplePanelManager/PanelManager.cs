using UnityEngine;

namespace CromisDev.SimplePanelSystem
{
    public enum PanelType
    {
  
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
                entry.panel.Hide();
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
