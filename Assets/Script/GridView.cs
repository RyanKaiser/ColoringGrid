using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class GridView : MonoBehaviour//, IDragHandler, IEndDragHandler
{
    [SerializeField] private float _gridSpacing = 0.1f;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private InputActionReference _inputActionReference;

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

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        _isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;

    }

    private void OnEnable()
    {
        _inputActionReference.action.performed += OnPointerMove;
        _inputActionReference.action.Enable();
    }

    private void OnDisable()
    {
        _inputActionReference.action.performed -= OnPointerMove;
        _inputActionReference.action.Disable();
    }

    private void OnPointerMove(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();
        TileView hoveredTile = GetTile(mousePosition);

        if (hoveredTile != null)
        {
            if (_lastHoveredTile != null && _lastHoveredTile != hoveredTile)
            {
                _lastHoveredTile.Highlight(false);
            }

            hoveredTile.Highlight(true);
            _lastHoveredTile = hoveredTile;
        }
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