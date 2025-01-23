using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton instance

    [Header("Audio Clips")]
    public AudioClip lidOpenSound; // Kapak açýlma sesi
    public AudioClip lidCloseSound; // Kapak kapanma sesi
    public AudioClip matchSound; // Eþleþme sesi
    public AudioClip shuffleSound;

    private AudioSource audioSource; // Ses çalmak için kullanýlan AudioSource

    private void Awake()
    {
        // Singleton yapýsýný oluþtur
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Oyun sahneleri arasýnda yok edilmez
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Ses çalmak için genel bir yöntem
    public void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
