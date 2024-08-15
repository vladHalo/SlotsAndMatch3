using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] public Button[] _buttons;

    [SerializeField] private AudioSource[] _soundEffects;

    public AudioSource _musicSource;

    private float _startVolumeMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _startVolumeMusic = _musicSource.volume;
        _buttons[0].onClick.AddListener(() => SetVolumeMusic(Str.Music, _musicSource));
        _buttons[1].onClick.AddListener(() => SetVolume(Str.Sound, _soundEffects));

        if (ES3.KeyExists(Str.Music))
        {
            _musicSource.volume = ES3.Load<float>(Str.Music);
        }

        if (ES3.KeyExists(Str.Sound))
        {
            var value = ES3.Load<float>(Str.Sound);
            _soundEffects.ForEach(x => x.volume = value);
        }
    }

    public void PlaySoundEffect(SoundType soundType)
    {
        _soundEffects[(int)soundType].Play();
    }

    private void SetVolume(string musicSound, params AudioSource[] manager)
    {
        float indexVolume = 1;
        if (ES3.KeyExists(musicSound))
            indexVolume = ES3.Load<float>(musicSound);
        indexVolume = indexVolume == 0 ? 1 : 0;
        manager.ForEach(x => x.volume = indexVolume);
        ES3.Save(musicSound, indexVolume);
    }

    private void SetVolumeMusic(string musicSound, params AudioSource[] manager)
    {
        float indexVolume = _startVolumeMusic;
        if (ES3.KeyExists(musicSound))
            indexVolume = ES3.Load<float>(musicSound);
        indexVolume = indexVolume == 0 ? _startVolumeMusic : 0;
        manager.ForEach(x => x.volume = indexVolume);
        ES3.Save(musicSound, indexVolume);
    }
}

public enum SoundType
{
    Win,
    Lose,
    Point,
    Bonus,
    Crash,
    Slot
}