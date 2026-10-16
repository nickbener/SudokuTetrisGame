using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Configs/AudioConfig")]
public class AudioConfigSO : ScriptableObject
{
    [Header("Фоновая музыка")]
    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField] [Range(0f, 1f)] private float _musicVolume = 0.5f;

    [Header("Звуковые эффекты (SFX)")]
    [SerializeField] private AudioClip _placeShapeSound;
    [SerializeField] private AudioClip _takeShape;
    [SerializeField] private AudioClip _rotateSound;
    [SerializeField] private AudioClip _clearLineSound;
    [SerializeField] [Range(0f, 1f)] private float _sfxVolume = 0.8f;

    public AudioClip BackgroundMusic => _backgroundMusic;
    public float MusicVolume => _musicVolume;

    public AudioClip PlaceShapeSound => _placeShapeSound;
    public AudioClip RotateSound => _rotateSound;
    public AudioClip ClearLineSound => _clearLineSound;
    public AudioClip TakeShape => _takeShape;
    public float SfxVolume => _sfxVolume;
}
