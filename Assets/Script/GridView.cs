using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class GridView : MonoBehaviour
{
    [SerializeField] private float _gridSpacing = 0.1f;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private InputActionReference _moveActionReference;
    [SerializeField] private InputActionReference _clickActionReference;

    private TileView[,] _tileViews;
    private bool _isDragging;

    private TileView _lastHoveredTile = null;
    public event Action<Color> OnColorSelected;

    public void InitializeGrid(int width, int height, Action<int, int> onTileClickedCallback)
    {
        _tileViews = new TileView[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float x = i + _gridSpacing * i;
                float y = j + _gridSpacing * j;
                GameObject tileObj = Instantiate(_tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                TileView tileView = tileObj.GetComponent<TileView>();
                tileView.Init(i, j, onTileClickedCallback);
                _tileViews[i, j] = tileView;
            }
        }

        UpdateGridColors();

        float centerX = (width + (width - 1) * _gridSpacing) / 2f - 0.5f;
        float centerY = (height + (height - 1) * _gridSpacing) / 2f - 0.5f;
        if (Camera.main != null)
            Camera.main.transform.position = new Vector3(centerX, centerY, -10f);
    }

    public void UpdateTileColor(int x, int y, Color color)
    {
        if (_tileViews[x, y] != null)
        {
            _tileViews[x, y].SetColor(color);
            _tileViews[x, y].UpdateVisual();
        }
    }

    public void UpdateGridColors()
    {
        foreach (var tileView in _tileViews)
        {
            if (tileView != null)
                tileView.UpdateVisual();
        }
    }

    private void OnEnable()
    {
        _moveActionReference.action.performed += OnPointerMove;

        _clickActionReference.action.started += OnPointerDragStarted;
        _clickActionReference.action.performed += OnPointerClick;
        _clickActionReference.action.canceled += OnPointerDragCanceled;

        _moveActionReference.action.Enable();
        _clickActionReference.action.Enable();
    }

    private void OnDisable()
    {
        _moveActionReference.action.performed -= OnPointerMove;

        _clickActionReference.action.started -= OnPointerDragStarted;
        _clickActionReference.action.performed -= OnPointerClick;
        _clickActionReference.action.canceled -= OnPointerDragCanceled;

        _moveActionReference.action.Disable();
        _clickActionReference.action.Disable();
    }

    private void OnPointerMove(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();
        TileView hoveredTile = GetTile(mousePosition);

        if (hoveredTile != null)
        {
            if (_lastHoveredTile != null )
            {
                _lastHoveredTile.Highlight(false);
            }

            if (_isDragging && _lastHoveredTile != hoveredTile)
            {
                hoveredTile.OnPointerClick();
            }

            hoveredTile.Highlight(true);
            _lastHoveredTile = hoveredTile;
        }
    }

    private void OnPointerClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        TileView clickedTile = GetTile(mousePosition);

        if (clickedTile != null)
        {
            clickedTile.OnPointerClick();
        }
    }

    private void OnPointerDragCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("ryan OnPointerDragCanceled");
        _isDragging = false;
    }

    private void OnPointerDragStarted(InputAction.CallbackContext context)
    {
        Debug.Log("ryan OnPointerDragStarted");
        _isDragging = true;
    }




    private TileView GetTile(Vector2 screenPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null)
        {
            return hit.collider.GetComponent<TileView>();
        }

        return null;
    }
}