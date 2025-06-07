using System.Collections.Generic;
using CromisDev.CardMatchingSystem;
using TMPro;
using UnityEngine;
namespace CromisDev.SimplePanelSystem
{
    public class GridLayoutDropdownGenerator : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown layoutDropdown;

        private readonly List<Vector2Int> validLayouts = new();
        private const int maxCards = 40;

        private void Start()
        {
            GenerateValidLayouts();
            PopulateDropdown();
            LoadSelection();
            layoutDropdown.onValueChanged.AddListener(OnDropdownChanged);
        }

        private void GenerateValidLayouts()
        {
            validLayouts.Clear();

            for (int columns = 2; columns <= maxCards; columns++)
            {
                for (int rows = 2; rows <= maxCards; rows++)
                {
                    int totalCards = columns * rows;

                    if (totalCards > maxCards) continue;
                    if (totalCards % 2 != 0) continue;
                    validLayouts.Add(new Vector2Int(columns, rows));
                }
            }
        }

        private void PopulateDropdown()
        {
            layoutDropdown.ClearOptions();
            List<string> options = new List<string>();

            foreach (var layout in validLayouts)
            {
                int total = layout.x * layout.y;
                options.Add($"{layout.x} x {layout.y} ({total} Cards)");
            }

            layoutDropdown.AddOptions(options);
        }

        private void OnDropdownChanged(int index)
        {
            if (index >= 0 && index < validLayouts.Count)
            {
                var selected = validLayouts[index];
                string save = $"{selected.x},{selected.y}";
                PlayerPrefs.SetString(GameConstants.PrefKeys.LAYOUT_PREFKEY, save);
                PlayerPrefs.Save();
            }
        }

        private void LoadSelection()
        {
            if (!PlayerPrefs.HasKey(GameConstants.PrefKeys.LAYOUT_PREFKEY)) return;

            string saved = PlayerPrefs.GetString(GameConstants.PrefKeys.LAYOUT_PREFKEY);
            var parts = saved.Split(',');
            if (parts.Length != 2) return;

            if (int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))
            {
                for (int i = 0; i < validLayouts.Count; i++)
                {
                    if (validLayouts[i].x == x && validLayouts[i].y == y)
                    {
                        layoutDropdown.value = i;
                        return;
                    }
                }
            }
        }

        public Vector2Int GetSelectedLayout => layoutDropdown.value >= 0 && layoutDropdown.value < validLayouts.Count ? validLayouts[layoutDropdown.value] : new Vector2Int(2, 2);

    }
}
