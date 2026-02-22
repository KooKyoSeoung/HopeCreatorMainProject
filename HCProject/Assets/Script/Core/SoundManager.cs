using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("BGM")]
    [SerializeField] private Sound[] arrayBGM;
    [SerializeField] private AudioSource bgmPlayer;

    [Header("SFX")]
    [SerializeField] private Sound[] arraySFX;
    [SerializeField] private AudioSource[] sfxPlayerArray;

    [Header("Volume")]
    [SerializeField] private float bgmVolume = 0.8f;
    [SerializeField] private float sfxVolume = 0.6f;

    private List<AudioSource> sfxPlayers;
    private Dictionary<string, AudioClip> dicBGM;
    private Dictionary<string, AudioClip> dicSFX;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        dicBGM = new Dictionary<string, AudioClip>();
        dicSFX = new Dictionary<string, AudioClip>();

        foreach (Sound sound in arrayBGM)
            dicBGM[sound.name] = sound.clip;

        foreach (Sound sound in arraySFX)
            dicSFX[sound.name] = sound.clip;

        sfxPlayers = sfxPlayerArray.ToList();
        sfxPlayers.ForEach(p => p.volume = sfxVolume);
    }

    public void PlayBGM(string bgmName)
    {
        if (!dicBGM.ContainsKey(bgmName))
        {
            Debug.LogWarning($"[SoundManager] BGM not found: {bgmName}");
            return;
        }

        if (bgmPlayer.isPlaying && bgmPlayer.clip == dicBGM[bgmName])
            return;

        bgmPlayer.clip = dicBGM[bgmName];
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
        bgmPlayer.clip = null;
    }

    public void PlaySFX(string sfxName)
    {
        if (!dicSFX.ContainsKey(sfxName))
        {
            Debug.LogWarning($"[SoundManager] SFX not found: {sfxName}");
            return;
        }

        foreach (var sfxPlayer in sfxPlayers)
        {
            if (!sfxPlayer.isPlaying)
            {
                sfxPlayer.clip = dicSFX[sfxName];
                sfxPlayer.volume = sfxVolume;
                sfxPlayer.Play();
                return;
            }
        }
    }

    public void StopSFX()
    {
        sfxPlayers.ForEach(p => p.Stop());
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmPlayer.volume = bgmVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxPlayers.ForEach(p => p.volume = sfxVolume);
    }
}
