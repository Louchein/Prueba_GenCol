using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource; // Fuente para música de fondo
    public AudioSource sfxSource;   // Fuente para efectos de sonido (SFX)

    [Header("Audio Clips")]
    public List<AudioClip> musicTracks; // Lista de pistas de música
    public List<AudioClip> soundEffects; // Lista de clips de efectos de sonido

    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    [Header("Settings")]
    public bool loopMusic = true; // ¿Debería la música repetirse automáticamente?
    public float musicVolume = 0.5f; // Volumen de la música
    public float sfxVolume = 1.0f;   // Volumen de los efectos de sonido

    private void Awake() {
        // Implementar Singleton
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        } else {
            Destroy(gameObject); // Asegurar que solo hay un AudioManager
        }

        // Cargar los efectos de sonido en un diccionario para acceso rápido
        foreach (var sfx in soundEffects) {
            if (!sfxDictionary.ContainsKey(sfx.name)) {
                sfxDictionary.Add(sfx.name, sfx);
            }
        }
    }

    private void Start() {
        // Configurar el volumen inicial
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;

        // Opcional: Comenzar la reproducción de música automáticamente
        if (musicTracks.Count > 0) {
            PlayMusic(musicTracks[0]);
        }
    }

    // Reproducir música de fondo
    public void PlayMusic(AudioClip clip) {
        if (musicSource.isPlaying) {
            musicSource.Stop();
        }

        musicSource.clip = clip;
        musicSource.loop = loopMusic;
        musicSource.Play();
    }

    // Reproducir un efecto de sonido por su nombre
    public void PlaySFX(string sfxName) {
        if (sfxDictionary.ContainsKey(sfxName)) {
            sfxSource.PlayOneShot(sfxDictionary[sfxName]);
        } else {
            Debug.LogWarning($"SFX '{sfxName}' not found in AudioManager.");
        }
    }

    // Reproducir un efecto de sonido directamente
    public void PlaySFX(AudioClip clip) {
        sfxSource.PlayOneShot(clip);
    }

    // Cambiar volumen de la música
    public void SetMusicVolume(float volume) {
        musicVolume = Mathf.Clamp01(volume); // Asegura que el volumen esté entre 0 y 1
        musicSource.volume = musicVolume;
    }

    // Cambiar volumen de los efectos de sonido
    public void SetSFXVolume(float volume) {
        sfxVolume = Mathf.Clamp01(volume); // Asegura que el volumen esté entre 0 y 1
        sfxSource.volume = sfxVolume;
    }

    // Detener la música
    public void StopMusic() {
        musicSource.Stop();
    }

    // Pausar la música
    public void PauseMusic() {
        musicSource.Pause();
    }

    // Reanudar la música
    public void ResumeMusic() {
        musicSource.UnPause();
    }
}
