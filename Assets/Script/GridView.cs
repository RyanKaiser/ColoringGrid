using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GridView : MonoBehaviour
{
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private float _gridSpacing = 0.1f;
    [SerializeField] private float _zoomSpeed = 1f;
    [SerializeField] private float _panSpeed = 1f;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference _moveActionReference;
    [SerializeField] private InputActionReference _clickActionReference;
    [SerializeField] private InputActionReference _zoomActionReference;
    [SerializeField] private InputActionReference _panActionReference;

    private TileCell[,] _tileCells;
    private bool _isDragging;
    private bool _isPanning;
    private Vector3 _lastMousePosition;
    private float _minZoom = 5f;
    private float _maxZoom = 20f;
    private TileCell _lastHoveredTile;

    public event Action OnDragStart;
    public event Action OnDragEnd;

    public void InitializeGrid(int width, int height, Action<int, int> onTileClickedCallback)
    {
        _tileCells = new TileCell[width, height];
        _maxZoom = Math.Min(width, height) / 2f + 5;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float x = i;
                float y = j;
                GameObject tileObj = Instantiate(_tilePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                TileCell tileCell = tileObj.GetComponent<TileCell>();
                tileCell.Init(i, j, _gridSpacing, onTileClickedCallback);
                _tileCells[i, j] = tileCell;
            }
        }

        UpdateGridColors();

        float centerX = width / 2f - 0.5f;
        float centerY = height / 2f - 0.5f;
        if (Camera.main != null)
            Camera.main.transform.position = new Vector3(centerX, centerY, -10f);
    }

    public void UpdateTileColor(int x, int y, Color color)
    {
        if (_tileCells[x, y] != null)
        {
            _tileCells[x, y].SetColor(color);
            _tileCells[x, y].UpdateVisual();
        }
    }

    public void UpdateGridColors(TileModel[,] tileModel)
    {
        for (int i = 0; i < tileModel.GetLength(0); i++)
        {
            for (int j = 0; j < tileModel.GetLength(1); j++)
            {
                _tileCells[i, j].SetColor(tileModel[i, j].color);
                _tileCells[i, j].UpdateVisual();
            }

        }
    }

    public void UpdateGridColors()
    {
        foreach (var tileView in _tileCells)
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

        _zoomActionReference.action.performed += OnZoom;

        _panActionReference.action.started += OnPanStarted;
        _panActionReference.action.canceled += OnPanCanceled;

        _moveActionReference.action.Enable();
        _clickActionReference.action.Enable();
        _zoomActionReference.action.Enable();
        _panActionReference.action.Enable();
    }

    private void OnDisable()
    {
        _moveActionReference.action.performed -= OnPointerMove;

        _clickActionReference.action.started -= OnPointerDragStarted;
        _clickActionReference.action.performed -= OnPointerClick;
        _clickActionReference.action.canceled -= OnPointerDragCanceled;

        _zoomActionReference.action.performed -= OnZoom;

        _panActionReference.action.started -= OnPanStarted;
        _panActionReference.action.canceled -= OnPanCanceled;

        _moveActionReference.action.Disable();
        _clickActionReference.action.Disable();
        _zoomActionReference.action.Disable();
        _panActionReference.action.Disable();
    }

    private void OnPointerMove(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();
        TileCell hoveredTile = GetTile(mousePosition);

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

        if (_isPanning)
        {
            Vector3 currentMousePosition = Mouse.current.position.ReadValue();

            if (Camera.main != null)
            {
                Vector3 delta = currentMousePosition - _lastMousePosition;
                float zoomFactor = Camera.main.orthographicSize * _panSpeed * Time.deltaTime;
                Camera.main.transform.Translate(-delta.x * zoomFactor, -delta.y * zoomFactor, 0);
            }

            _lastMousePosition = currentMousePosition;
        }
    }

    private void OnPointerClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (IsPointerOverUI()) return;


        Vector2 mousePosition = Mouse.current.position.ReadValue();
        TileCell clickedTile = GetTile(mousePosition);

        if (clickedTile != null)
        {
            clickedTile.OnPointerClick();
        }
    }

    private void OnPointerDragCanceled(InputAction.CallbackContext context)
    {
        _isDragging = false;
        OnDragEnd?.Invoke();
    }

    private void OnPointerDragStarted(InputAction.CallbackContext context)
    {
        _isDragging = true;
        OnDragStart?.Invoke();
    }

    private bool IsPointerOverUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                return true;
            }
        }

        return false;
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        Vector2 scrollValue = context.ReadValue<Vector2>();

        if (Camera.main != null)
        {
            Camera.main.orthographicSize -= scrollValue.y * _zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, _minZoom, _maxZoom);
        }
    }

    private void OnPanStarted(InputAction.CallbackContext context)
    {
        if (context.control.path == "/Mouse/middleButton")
        {
            _isPanning = true;
            _lastMousePosition = Mouse.current.position.ReadValue();
        }
    }

    private void OnPanCanceled(InputAction.CallbackContext context)
    {
        if (context.control.path == "/Mouse/middleButton")
        {
            Debug.Log("pan Canceled");
            _isPanning = false;
        }
    }

    private TileCell GetTile(Vector2 screenPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null)
        {
            return hit.collider.GetComponent<TileCell>();
        }

        return null;
    }
}