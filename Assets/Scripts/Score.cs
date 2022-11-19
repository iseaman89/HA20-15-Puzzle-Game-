using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score 
{

    #region Variables

    [SerializeField] private int _currentScore;
    [SerializeField] private int _levelScoreBonus;
    [SerializeField] private int _turnScoreBonus;

    public int CurrentScore
    {
        get => _currentScore;
        set
        {
            _currentScore = value;
            HUD.Instance.UpdateScoreValue(_currentScore);
        }
    }

    #endregion

    #region Functions

    public void AddLevelBonus()
    {
        CurrentScore += _levelScoreBonus;
    }

    public void AddTurnBonus()
    {
        CurrentScore += _turnScoreBonus;
    }

    #endregion
}
