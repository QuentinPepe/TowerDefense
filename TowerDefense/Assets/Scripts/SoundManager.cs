using Towers;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private Game game;

    [SerializeField] private AudioClip placeTowerSound;
    [SerializeField] private AudioClip upgradeTowerSound;

    [SerializeField] private AudioClip hitTowerSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        towerManager.OnTowerPlaced += HandleTowerPlaced;
        towerManager.OnTowerUpgraded += HandleTowerUpgraded;

        game.OnTowerHit += HandleTowerHit;
    }

    private void HandleTowerPlaced(Tower tower)
    {
        _audioSource.clip = placeTowerSound;
        _audioSource.Play();
    }

    private void HandleTowerUpgraded(Tower tower)
    {
        _audioSource.clip = upgradeTowerSound;
        _audioSource.Play();
    }

    private void HandleTowerHit()
    {
        _audioSource.clip = hitTowerSound;
        _audioSource.Play();
    }


}