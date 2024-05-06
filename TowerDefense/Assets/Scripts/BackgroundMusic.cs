using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic _instance;

    [SerializeField] private AudioClip backgroundMusicClip;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = backgroundMusicClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}