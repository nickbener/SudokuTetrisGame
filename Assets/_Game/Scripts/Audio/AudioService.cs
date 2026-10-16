using UnityEngine;
using Zenject;

public class AudioService : MonoBehaviour, IAudioService
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    private AudioConfigSO _config;
    private float _normalMusicVolume;

    [Inject]
    public void Construct(AudioConfigSO config)
    {
        _config = config;
    }

    private void Awake()
    {
        if (_musicSource == null)
            _musicSource = gameObject.AddComponent<AudioSource>();
            
        if (_sfxSource == null)
            _sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (_config == null || _config.BackgroundMusic == null) return;

        _normalMusicVolume = _config.MusicVolume;

        _musicSource.clip = _config.BackgroundMusic;
        _musicSource.volume = _normalMusicVolume;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void SetPauseMusicVolume(bool isPaused)
    {
        if (_musicSource == null) return;

        if (isPaused)
        {
            _musicSource.volume = _normalMusicVolume * 0.25f;
        }
        else
        {
            _musicSource.volume = _normalMusicVolume;
        }
    }

    public void PlayPlaceSound()
    {
        PlaySfx(_config.PlaceShapeSound);
    }

    public void PlayRotateSound()
    {
        PlaySfx(_config.RotateSound);
    }

    public void PlayClearLineSound()
    {
        PlaySfx(_config.ClearLineSound);
    }

    public void PlayTakeShape()
    {
        PlaySfx(_config.TakeShape);
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip != null && _sfxSource != null)
        {
            _sfxSource.PlayOneShot(clip, _config.SfxVolume);
        }
    }
}