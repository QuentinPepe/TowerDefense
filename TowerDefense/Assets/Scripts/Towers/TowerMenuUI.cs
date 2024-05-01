using System.Collections.Generic;
using DG.Tweening;
using Grid;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Towers
{
    public class TowerMenuUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI currentDamage;
        [SerializeField] private TextMeshProUGUI currentFireRate;
        [SerializeField] private TextMeshProUGUI currentRange;
        [SerializeField] private TextMeshProUGUI upgradeDamage;
        [SerializeField] private TextMeshProUGUI upgradeFireRate;
        [SerializeField] private TextMeshProUGUI upgradeRange;
        [SerializeField] private TextMeshProUGUI cost;
        [SerializeField] private List<GameObject> toHideIfThereIsNoUpgrade;

        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button destroyButton;
        [SerializeField] private Button closeButton;

        [SerializeField] private GridController gridController;
        [SerializeField] private TowerManager towerManager;

        [SerializeField] private OpenCloseAnimationPanel panel;

        private void Start()
        {
            gridController.OnCellClick += HandleCellClick;
            closeButton.onClick.AddListener(Hide);
            Game.Instance.GetWaveManager().OnWaveStarted += (_) => Hide();
        }

        private void OnDestroy()
        {
            gridController.OnCellClick -= HandleCellClick;
        }

        private void HandleCellClick(CellPosition position)
        {
            if (panel.IsOpen) return;
            Tower tower = towerManager.GetTower(position);
            if (tower && tower.GetTimeSincePlacement() > 0.25f) Show(tower);
        }

        private void Show(Tower tower)
        {
            upgradeButton.onClick.AddListener(() => {
                towerManager.UpgradeTower(tower);
                Refresh(tower);
            });

            destroyButton.onClick.AddListener(() => {
                towerManager.DestroyTower(tower);
                Hide();
            });

            currentDamage.text = "0";
            currentFireRate.text = "0";
            currentRange.text = "0";
            upgradeDamage.text = "0";
            upgradeFireRate.text = "0";
            upgradeRange.text = "0";
            cost.text = "0";

            Refresh(tower);

            panel.Show();
        }

        private void AnimateValue(TextMeshProUGUI textComponent, float startValue, float endValue, float duration = 1.0f)
        {
            DOTween.To(() => startValue, x => {
                startValue = x;
                textComponent.text = Mathf.RoundToInt(startValue).ToString();
            }, endValue, duration);
        }

        private void Refresh(Tower tower)
        {
            title.text = tower.Data.towerName + " Lv." + tower.Data.level;

            AnimateValue(currentDamage, float.Parse(currentDamage.text), tower.Data.damage);
            AnimateValue(currentFireRate, float.Parse(currentFireRate.text), tower.Data.fireRate);
            AnimateValue(currentRange, float.Parse(currentRange.text), tower.Data.range);

            foreach (GameObject obj in toHideIfThereIsNoUpgrade)
            {
                obj.SetActive(tower.CanUpgrade());
            }

            if (!tower.CanUpgrade()) return;
            upgradeButton.interactable = tower.CanUpgrade() && towerManager.CanAfford(tower.Data.upgrade.cost);

            AnimateValue(upgradeDamage, float.Parse(upgradeDamage.text), tower.Data.upgrade.damage);
            AnimateValue(upgradeFireRate, float.Parse(upgradeFireRate.text), tower.Data.upgrade.fireRate);
            AnimateValue(upgradeRange, float.Parse(upgradeRange.text), tower.Data.upgrade.range);
            AnimateValue(cost, float.Parse(cost.text), tower.Data.upgrade.cost);
        }

        private void Hide()
        {
            upgradeButton.onClick.RemoveAllListeners();
            destroyButton.onClick.RemoveAllListeners();
            panel.Hide();
        }

        private void Update()
        {
            if (!panel.IsOpen) return;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Hide();
            }
        }
    }
}