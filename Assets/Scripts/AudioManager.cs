using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton instance

    [Header("Audio Clips")]
    public AudioClip lidOpenSound; // Kapak a��lma sesi
    public AudioClip lidCloseSound; // Kapak kapanma sesi
    public AudioClip matchSound; // E�le�me sesi
    public AudioClip shuffleSound;

    private AudioSource audioSource; // Ses �almak i�in kullan�lan AudioSource

    private void Awake()
    {
        // Singleton yap�s�n� olu�tur
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Oyun sahneleri aras�nda yok edilmez
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Ses �almak i�in genel bir y�ntem
    public void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
