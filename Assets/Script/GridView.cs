using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class GridView : MonoBehaviour
{
    [SerializeField] private float _gridSpacing = 0.1f;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private InputActionReference _moveActionReference;
    [SerializeField] private InputActionReference _clickActionReference;
    [SerializeField] private InputActionReference _zoomActionReference;
    [SerializeField] private InputActionReference _panActionReference;

    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private float panSpeed = 1f;


    private TileCell[,] _tileCells;
    private bool _isDragging;
    private bool _isPanning;
    private Vector3 _lastMousePosition;

    private int _temp;

    private TileCell _lastHoveredTile;
    // public event Action<Color> OnColorSelected;

    public void InitializeGrid(int width, int height, Action<int, int> onTileClickedCallback)
    {
        _tileCells = new TileCell[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float x = i + _gridSpacing * i;
                float y = j + _gridSpacing * j;
                GameObject tileObj = Instantiate(_tilePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                TileCell tileCell = tileObj.GetComponent<TileCell>();
                tileCell.Init(i, j, onTileClickedCallback);
                _tileCells[i, j] = tileCell;
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
        // _panActionReference.action.performed += OnPanPerformed;
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
        // _panActionReference.action.performed -= OnPanPerformed;
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
                float zoomFactor = Camera.main.orthographicSize * panSpeed * Time.deltaTime;
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
        Debug.Log("ryan OnPointerDragCanceled");
        _isDragging = false;
    }

    private void OnPointerDragStarted(InputAction.CallbackContext context)
    {
        Debug.Log("ryan OnPointerDragStarted");
        _isDragging = true;
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
        var _camera = Camera.main;
        _camera.orthographicSize -= scrollValue.y * zoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, minZoom, maxZoom);
    }

    private void OnPanStarted(InputAction.CallbackContext context)
    {
        if (context.control.path == "/Mouse/middleButton")
        {
            Debug.Log("pan started");
            _isPanning = true;
            _lastMousePosition = Mouse.current.position.ReadValue();
        }
    }

    // private void OnPanPerformed(InputAction.CallbackContext context)
    // {
    //
    // }

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