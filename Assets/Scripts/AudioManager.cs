using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource; // Fuente para m�sica de fondo
    public AudioSource sfxSource;   // Fuente para efectos de sonido (SFX)

    [Header("Audio Clips")]
    public List<AudioClip> musicTracks; // Lista de pistas de m�sica
    public List<AudioClip> soundEffects; // Lista de clips de efectos de sonido

    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    [Header("Settings")]
    public bool loopMusic = true; // �Deber�a la m�sica repetirse autom�ticamente?
    public float musicVolume = 0.5f; // Volumen de la m�sica
    public float sfxVolume = 1.0f;   // Volumen de los efectos de sonido

    private void Awake() {
        // Implementar Singleton
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        } else {
            Destroy(gameObject); // Asegurar que solo hay un AudioManager
        }

        // Cargar los efectos de sonido en un diccionario para acceso r�pido
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

        // Opcional: Comenzar la reproducci�n de m�sica autom�ticamente
        if (musicTracks.Count > 0) {
            PlayMusic(musicTracks[0]);
        }
    }

    // Reproducir m�sica de fondo
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

    // Cambiar volumen de la m�sica
    public void SetMusicVolume(float volume) {
        musicVolume = Mathf.Clamp01(volume); // Asegura que el volumen est� entre 0 y 1
        musicSource.volume = musicVolume;
    }

    // Cambiar volumen de los efectos de sonido
    public void SetSFXVolume(float volume) {
        sfxVolume = Mathf.Clamp01(volume); // Asegura que el volumen est� entre 0 y 1
        sfxSource.volume = sfxVolume;
    }

    // Detener la m�sica
    public void StopMusic() {
        musicSource.Stop();
    }

    // Pausar la m�sica
    public void PauseMusic() {
        musicSource.Pause();
    }

    // Reanudar la m�sica
    public void ResumeMusic() {
        musicSource.UnPause();
    }
}
