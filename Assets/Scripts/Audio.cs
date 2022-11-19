using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Audio
{
    #region Private_Variables

    //Link to sound source
    private AudioSource _sourceSFX;

    //Link to music source
    private AudioSource _sourceMusic;

    //Link to sounds with random pitch source
    private AudioSource _sourceRandomPitchSFX;

    private float _musicVolume = 1f;
    private float _sfxVolume = 1f;

    [SerializeField] private AudioClip[] _sounds;
    [SerializeField] private AudioClip _defaultClip;
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameMusic;

    public AudioSource SourceSFX { get => _sourceSFX; set => _sourceSFX = value; }
    public AudioSource SourceMusic { get => _sourceMusic; set => _sourceMusic = value; }
    public AudioSource SourceRandomPitchSFX { get => _sourceRandomPitchSFX; set => _sourceRandomPitchSFX = value; }
    public float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            SourceMusic.volume = _musicVolume;
            DataStore.SaveOptions();
        }
    }
    public float SfxVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = value;
            SourceSFX.volume = _sfxVolume;
            SourceRandomPitchSFX.volume = _sfxVolume;

            DataStore.SaveOptions();
        }
    }

    #endregion

    ///<summary>
    ///Search sound in array
    ///</summary>
    ///<param name="clipName">Sound's name</param>
    ///<returns>
    ///Sound. If sound didn't found, return default Clip
    ///</returns>
    private AudioClip GetSound(string clipName)
    {
        for (int i = 0; i < _sounds.Length; i++)
        {
            if (_sounds[i].name == clipName)
            {
                return _sounds[i];
            }
        }

        Debug.LogError("Can't find clip" + clipName);

        return _defaultClip;
    }

    ///<summary>
    ///Play sounds from array
    ///</summary>
    ///<param name="clipName">Sound's name</param>
    public void PlaySound(string clipName)
    {
        SourceSFX.PlayOneShot(GetSound(clipName), SfxVolume);
    }

    ///<summary>
    ///Play sounds from array with random pitch
    ///</summary>
    ///<param name="clipName">Sound's name</param>
    public void PlaySoundRandomPitch(string clipName)
    {
        SourceRandomPitchSFX.pitch = Random.Range(0.7f, 1.3f);
        SourceRandomPitchSFX.PlayOneShot(GetSound(clipName), SfxVolume);
    }

    ///<summary>
    ///Play Music
    ///</summary>
    ///<param name="menu">Is it for main menu?</param>
    public void PlayMusic(bool menu)
    {
        if (menu)
        {
            SourceMusic.clip = _menuMusic;
        }
        else
        {
            SourceMusic.clip = _gameMusic;
        }

        SourceMusic.volume = MusicVolume;
        SourceMusic.loop = true;
        SourceMusic.Play();
    }
}
