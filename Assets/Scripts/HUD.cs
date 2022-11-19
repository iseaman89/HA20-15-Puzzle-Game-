using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI[] _scoreValue;
    [SerializeField] private TextMeshProUGUI _turnsValue;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private CanvasGroup _levelComplitedWindow;

    private GraphicRaycaster _raycaster;

    private static HUD s_instance;

    public static HUD Instance
    {
        get => s_instance;
    }

    #endregion

    #region Functions

    private void Awake()
    {
        s_instance = this;
        _raycaster = gameObject.GetComponent<GraphicRaycaster>();
    }

    public void UpdateTurnsValue(int value)
    {
        _turnsValue.text = value.ToString();
    }

    public void UpdateScoreValue(int value)
    {
        for (int i = 0; i < _scoreValue.Length; i++)
        {
            _scoreValue[i].text = value.ToString();

        }
    }

    public void ShowWindow(CanvasGroup window)
    {
        window.alpha = 1f;
        window.blocksRaycasts = true;
        window.interactable = true;
    }

    public void HideWindow(CanvasGroup window)
    {
        window.alpha = 0f;
        window.blocksRaycasts = false;
        window.interactable = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Reset()
    {
        Controller.Instance.Reset();
    }

    public void SetMusicVolume(float volume)
    {
        Controller.Instance.Audio.MusicVolume = volume;
    }

    public void SetSoundVolume(float volume)
    {
        Controller.Instance.Audio.SfxVolume = volume;
    }

    public void UpdateOptions()
    {
        _musicSlider.value = Controller.Instance.Audio.MusicVolume;
        _soundSlider.value = Controller.Instance.Audio.SfxVolume;
    }

    public void Next()
    {
        Controller.Instance.InitializeLevel();
    }

    private IEnumerator Count(int to, float delay)
    {
        _raycaster.enabled = false;

        for (int i = 0; i <= to; i++)
        {
            yield return new WaitForSeconds(delay);
            Controller.Instance.Score.AddTurnBonus();
        }

        DataStore.SaveGame();

        _raycaster.enabled = true;
    }

    public void CountScore(int to)
    {
        ShowWindow(_levelComplitedWindow);
        StartCoroutine(Count(to, 0.3f));
    }

    /// <summary>
    /// Play preview sound for sounds settings
    /// </summary>
    public void PlayPreviewSound()
    {
        Controller.Instance.Audio.PlaySound("Drop");
    }

    #endregion
}
