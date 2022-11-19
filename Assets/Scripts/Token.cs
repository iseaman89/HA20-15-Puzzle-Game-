using UnityEngine;
using UnityEngine.EventSystems;

public class Token : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Variables

    private Camera _camera;
    private Vector3 _pointerPositionBeforeDrag;
    private Vector3 _positionBeforeDrag;
    private int _tokenTyp;
    private int[] _dragSpace;
    private AudioSource _audioSource;

    #endregion
    #region Functions

    private void Start()
    {
        _camera = Camera.main;

        _audioSource = gameObject.GetComponent<AudioSource>();

        AlignOnGrid();

        _tokenTyp = Random.Range(0, Controller.Instance.TokenTypes);

        Material myMaterial = gameObject.GetComponent<Renderer>().material;

        myMaterial.SetColor("_Color", Controller.Instance.TokenColors[_tokenTyp]);

        Controller.Instance.TokensByTypes[_tokenTyp].Add(this);

        transform.SetParent(Controller.Instance.Field.transform);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _audioSource.Play();
        _pointerPositionBeforeDrag = _camera.ScreenToWorldPoint(eventData.position);
        _positionBeforeDrag = transform.position;
        GetDragSpace();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 previousPosition = transform.position;

        Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(eventData.position);

        Vector3 totalDrag = mouseWorldPosition - _pointerPositionBeforeDrag;

        if (Mathf.Abs(totalDrag.x) > Mathf.Abs(totalDrag.y))
        {
            float posX = Mathf.Clamp(mouseWorldPosition.x, _positionBeforeDrag.x - _dragSpace[1], _positionBeforeDrag.x + _dragSpace[0]);

            transform.position = new Vector3(posX, _positionBeforeDrag.y, transform.position.z);
        }
        else
        {
            float posY = Mathf.Clamp(mouseWorldPosition.y, _positionBeforeDrag.y - _dragSpace[3], _positionBeforeDrag.y + _dragSpace[2]);

            transform.position = new Vector3(_positionBeforeDrag.x, posY, transform.position.z);
        }

        float currentFrameTokenDrag = Vector3.Distance(previousPosition, transform.position);

        float clampedPitchDrag = Mathf.Clamp(currentFrameTokenDrag * 10, 0.9f, 1.05f);

        _audioSource.pitch = Mathf.Lerp(_audioSource.pitch, clampedPitchDrag, 0.5f);

        float clampedVolumeDrag = Mathf.Clamp(currentFrameTokenDrag * 10, 0.2f, 1.2f);

        float interpolatedDrag = Mathf.Lerp(_audioSource.volume, clampedVolumeDrag - 0.2f, 0.7f);

        _audioSource.volume = interpolatedDrag * Controller.Instance.Audio.SfxVolume;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _audioSource.Stop();

        AlignOnGrid();

        Controller.Instance.TurnDone();
    }


    /// <summary>
    /// Align on grid
    /// </summary>
    private void AlignOnGrid()
    {
        Vector3 alignedPosition = transform.position;
        alignedPosition.x = Mathf.Round(transform.position.x);
        alignedPosition.y = Mathf.Round(transform.position.y);
        transform.position = alignedPosition;
    }

    /// <summary>
    /// Search space by drag
    /// </summary>
    private void GetDragSpace()
    {
        int OddEven = 1;
        if (Controller.Instance.FieldSize % 2 != 0)
        {
            OddEven = 0;
        }

        _dragSpace = new int[] { 0, 0, 0, 0 };
        int halfField = (Controller.Instance.FieldSize - 1) / 2;

        _dragSpace[0] = CheckSpace(Vector2.right);
        if (_dragSpace[0] == -1)
        {
            _dragSpace[0] = halfField - (int)transform.position.x + OddEven;
        }

        _dragSpace[1] = CheckSpace(Vector2.left);
        if (_dragSpace[1] == -1)
        {
            _dragSpace[1] = halfField + (int)transform.position.x;
        }

        _dragSpace[2] = CheckSpace(Vector2.up);
        if (_dragSpace[2] == -1)
        {
            _dragSpace[2] = halfField - (int)transform.position.y + OddEven;
        }

        _dragSpace[3] = CheckSpace(Vector2.down);
        if (_dragSpace[3] == -1)
        {
            _dragSpace[3] = halfField + (int)transform.position.y;
        }
    }


    /// <summary>
    /// Check space around token
    /// </summary>
    /// <param name="direction">Ray direction</param>
    /// <returns>
    /// If token didn't found, return -1
    /// </returns>
    private int CheckSpace(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject != gameObject)
            {
                return Mathf.FloorToInt(hits[i].distance);
            }
        }
        return -1;
    }

    #endregion


}
