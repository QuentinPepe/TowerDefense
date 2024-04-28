using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Towers
{
    public class TowerSelectionUI : MonoBehaviour
    {
        [SerializeField] private List<Button> towerButtons;
        [SerializeField] private List<TowerSO> towerData;
        [SerializeField] private List<TextMeshProUGUI> towerCostTexts;
        private TowerSO _selectedTower;

        public event Action<TowerSO> OnTowerSelected;

        private void Awake()
        {
            InitializeButtons();
            UpdateCostTexts();
        }

        private void InitializeButtons()
        {
            if (towerButtons.Count != towerData.Count)
            {
                Debug.LogError("Mismatch between tower buttons and tower data counts.");
                return;
            }

            for (int i = 0; i < towerButtons.Count; i++)
            {
                int index = i;
                towerButtons[i].onClick.AddListener(() => SelectTower(index));
            }
        }

        private void UpdateCostTexts()
        {
            if (towerCostTexts.Count != towerData.Count)
            {
                Debug.LogError("Mismatch between tower cost texts and tower data counts.");
                return;
            }

            for (int i = 0; i < towerCostTexts.Count; i++)
            {
                towerCostTexts[i].text = towerData[i].cost + " $";
            }
        }

        private void SelectTower(int index)
        {
            if (index < 0 || index >= towerData.Count)
            {
                Debug.LogError("Tower index out of range.");
                return;
            }

            _selectedTower = towerData[index];
            OnTowerSelected?.Invoke(_selectedTower);
        }
    }
}