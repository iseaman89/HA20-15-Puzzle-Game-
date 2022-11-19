using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataStore
{
    #region Variables

    private const string MUSIC_VOLUME_KEX = "MusicVolume";
    private const string SOUND_VOLUME_KEX = "Sound volume";
    private const string SAVED_LEVEL_KEY = "Level";
    private const string SAVED_SCORE_KEY = "Score";
    private const float DEFAULT_VOLUME = 0.75f;


    #endregion

    #region Function

    /// <summary>
    /// Save sounds options
    /// </summary>
    public static void SaveOptions()
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEX, Controller.Instance.Audio.MusicVolume);

        PlayerPrefs.SetFloat(SOUND_VOLUME_KEX, Controller.Instance.Audio.SfxVolume);

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Load sounds options
    /// </summary>
    public static void LoadOptions()
    {
        Controller.Instance.Audio.MusicVolume =
            PlayerPrefs.GetFloat(MUSIC_VOLUME_KEX, DEFAULT_VOLUME);

        Controller.Instance.Audio.SfxVolume =
            PlayerPrefs.GetFloat(SOUND_VOLUME_KEX, DEFAULT_VOLUME);
    }

    /// <summary>
    /// Load game process
    /// </summary>
    public static void LoadGame()
    {
        Controller.Instance.CurrentLevel =
            PlayerPrefs.GetInt(SAVED_LEVEL_KEY, 1);

        Controller.Instance.Score.CurrentScore =
            PlayerPrefs.GetInt(SAVED_SCORE_KEY, 0);
    }

    /// <summary>
    /// Save game process
    /// </summary>
    public static void SaveGame()
    {
        PlayerPrefs.SetInt(SAVED_LEVEL_KEY, Controller.Instance.CurrentLevel);

        PlayerPrefs.SetInt(SAVED_SCORE_KEY, Controller.Instance.Score.CurrentScore);

        PlayerPrefs.Save();
    }

    #endregion
}
