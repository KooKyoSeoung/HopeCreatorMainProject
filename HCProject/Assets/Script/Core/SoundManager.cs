using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Option")]
    [SerializeField] private bool dontDestroyOnLoad = true;

    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip[] bgmClips;
    private float bgmVolume = 1f;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] sfxClips;
    private float sfxVolume = 1f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(int index)
    {
        if (bgmClips == null || index < 0 || index >= bgmClips.Length)
            return;
        AudioClip clip = bgmClips[index];
        if (clip == null)
            return;
        if (bgmSource.isPlaying && bgmSource.clip == clip)
            return;
        bgmSource.clip = clip;
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null)
            return;
        if (bgmSource.isPlaying && bgmSource.clip == clip)
            return;
        bgmSource.clip = clip;
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
        bgmSource.clip = null;
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = bgmVolume;
    }

    public void PlaySFX(int index)
    {
        if (sfxClips == null || index < 0 || index >= sfxClips.Length)
            return;
        AudioClip clip = sfxClips[index];
        if (clip == null)
            return;
        sfxSource.volume = sfxVolume;
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;
        sfxSource.volume = sfxVolume;
        sfxSource.PlayOneShot(clip);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
}
