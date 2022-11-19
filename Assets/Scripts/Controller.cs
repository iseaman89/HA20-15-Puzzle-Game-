using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    #region Variables

    private static Controller s_instance;

    private List<List<Token>> _tokensByTypes;
    private Field _field;
    private int _currentLevel;

    [SerializeField] private Audio _audio = new Audio();
    [SerializeField] private Score _score;
    [SerializeField] private LevelParameters _level;
    [SerializeField] private Color[] _tokenColors;

    public static Controller Instance
    {
        get
        {
            if (s_instance == null)
            {
                var controller = Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;

                s_instance = controller.GetComponent<Controller>();
            }

            return s_instance;
        }
    }

    public int TokenTypes { get => _level.TokenTypes; }
    public int FieldSize { get => _level.FieldSize; }
    public Color[] TokenColors { get => _tokenColors; set => _tokenColors = value; }
    public List<List<Token>> TokensByTypes { get => _tokensByTypes; set => _tokensByTypes = value; }
    public Field Field { get => _field; set => _field = value; }
    public LevelParameters Level { get => _level; set => _level = value; }
    public Score Score { get => _score; set => _score = value; }
    public Audio Audio { get => _audio; set => _audio = value; }
    public int CurrentLevel { get => _currentLevel; set => _currentLevel = value; }

    #endregion

    #region Function

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (s_instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);

        MakeColors(_level.TokenTypes);

        Audio.SourceMusic = gameObject.AddComponent<AudioSource>();
        Audio.SourceRandomPitchSFX = gameObject.AddComponent<AudioSource>();
        Audio.SourceSFX = gameObject.AddComponent<AudioSource>();

        DataStore.LoadOptions();
    }

    private void Start()
    {
        DataStore.LoadGame();
        InitializeLevel();
        Audio.PlayMusic(true);
    }

    /// <summary>
    /// Set color steps for token
    /// </summary>
    /// <param name="count">Color menge</param>
    /// <returns>
    /// Colors result
    /// </returns>
    private Color[] MakeColors(int count)
    {
        Color[] result = new Color[count];

        float coloeStep = 1f / (count + 1);

        float hue = 0f;

        float saturation = 0.5f;

        float value = 1f;

        for (int i = 0; i < count; i++)
        {
            float newHue = hue + (coloeStep * i);

            result[i] = Color.HSVToRGB(newHue, saturation, value);
        }

        return result;
    }

    /// <summary>
    /// After turn done
    /// </summary>
    public void TurnDone()
    {
        Audio.PlaySound("Drop");

        if (IsAllTokensConnected())
        {
            Audio.PlaySound("Victory");
            Debug.Log("Win!");
            Score.AddLevelBonus();
            CurrentLevel++;
            Destroy(_field.gameObject);
            HUD.Instance.CountScore(_level.Turns);
        }
        else
        {
            Debug.Log("Continue...");
            if (_level.Turns > 0)
            {
                _level.Turns--;
            }
        }
    }

    /// <summary>
    /// Check are all tokens connected
    /// </summary>
    /// <returns>
    /// Return true if yes or false when no
    /// </returns>
    public bool IsAllTokensConnected()
    {
        //TODO:
        //To optimaze: перевіряти лише той тип, фішка якого була переміщена

        for (int i = 0; i < TokensByTypes.Count; i++)
        {
            if (IsTokensConnected(TokensByTypes[i]) == false)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Check is token connected to another token
    /// </summary>
    /// <param name="tokens">List with all tokens in field</param>
    /// <returns></returns>
    private bool IsTokensConnected(List<Token> tokens)
    {
        if (tokens.Count == 0)
        {
            return true;
        }

        List<Token> connectedTokens = new List<Token>();
        connectedTokens.Add(tokens[0]);

        bool moved = true;

        while (moved)
        {
            moved = false;

            for (int i = 0; i < connectedTokens.Count; i++)
            {
                for (int j = 0; j < tokens.Count; j++)
                {
                    if (IsTokensNear(tokens[j], connectedTokens[i]))
                    {
                        if (connectedTokens.Contains(tokens[j]) == false)
                        {
                            connectedTokens.Add(tokens[j]);
                            moved = true;
                        }
                    }
                }
            }
        }

        if (tokens.Count == connectedTokens.Count)
        {
            return true;
        }

        return false;
    }

    private bool IsTokensNear(Token first, Token second)
    {
        if ((int)first.transform.position.x == (int)second.transform.position.x + 1 ||
            (int)first.transform.position.x == (int)second.transform.position.x - 1)
        {
            if ((int)first.transform.position.y == (int)second.transform.position.y)
            {
                return true;
            }
        }

        if ((int)first.transform.position.y == (int)second.transform.position.y + 1 ||
            (int)first.transform.position.y == (int)second.transform.position.y - 1)
        {
            if ((int)first.transform.position.x == (int)second.transform.position.x)
            {
                return true;
            }
        }

        return false;
    }

    public void InitializeLevel()
    {
        _level = new LevelParameters(CurrentLevel);

        TokenColors = MakeColors(_level.TokenTypes);

        TokensByTypes = new List<List<Token>>();

        for (int i = 0; i < _level.TokenTypes; i++)
        {
            TokensByTypes.Add(new List<Token>());
        }

        _field = Field.Create(Level.FieldSize, _level.FreeSpace);
    }

    public void Reset()
    {
        CurrentLevel = 1;
        Score.CurrentScore = 0;
        Destroy(_field.gameObject);
        DataStore.SaveGame();
        InitializeLevel();
    }

    #endregion
}
