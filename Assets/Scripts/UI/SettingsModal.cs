using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsModal : Modal
{
    private const string SoundVolumeKey = "sound_volume";
    private const string MusicVolumeKey = "music_volume";

    [SerializeField] private Slider _soundVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;

    [SerializeField] private AudioMixer _audioMixer;

    private void Start()
    {
        bool firstLaunch = !PlayerPrefs.HasKey(SoundVolumeKey);

        if (firstLaunch)
        {
            _soundVolumeSlider.value = 100f;
            _musicVolumeSlider.value = 100f;
        }
        else
        {
            _soundVolumeSlider.value = PlayerPrefs.GetFloat(SoundVolumeKey);
            _musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey);
        }
    }

    public void SetSoundVolume()
        => PlayerPrefs.SetFloat(SoundVolumeKey, _soundVolumeSlider.value);

    public void SetMusicVolume()
        => PlayerPrefs.SetFloat(MusicVolumeKey, _musicVolumeSlider.value);

    protected override void OnActivate() { }

    protected override void OnDeactivate()
        => PlayerPrefs.Save();
}
