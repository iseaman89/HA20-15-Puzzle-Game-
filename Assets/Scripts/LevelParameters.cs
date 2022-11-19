using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelParameters
{
    #region Variables

    [SerializeField] private int _fieldSize;
    [SerializeField] private int _freeSpace;
    [SerializeField] private int _tokenTypes;
    [SerializeField] private int _turns;

    public int FieldSize { get => _fieldSize; }
    public int FreeSpace { get => _freeSpace; }
    public int TokenTypes { get => _tokenTypes; }
    public int Turns
    {
        get => _turns;
        set
        {
            _turns = value;
            HUD.Instance.UpdateTurnsValue(_turns);
        }
    }

    #endregion

    #region Functions

    /// <summary>
    /// Set parameters for level
    /// </summary>
    /// <param name="currentLevel">Number of current level</param>
    public LevelParameters(int currentLevel)
    {
        int fieldIncreaseStep = currentLevel / 4;

        float subStep = (currentLevel / 4f) - fieldIncreaseStep;

        _fieldSize = 3 + fieldIncreaseStep;

        _freeSpace = (int)(_fieldSize * (1f - subStep));

        if (_freeSpace < 1)
        {
            _freeSpace = 1;
        }

        _tokenTypes = 2 + (currentLevel / 3);

        if (_tokenTypes > 10)
        {
            _tokenTypes = 10;
        }

        _turns = (((_fieldSize * _fieldSize / 2) - _freeSpace) * _tokenTypes) + _fieldSize;
    }

    #endregion
}
