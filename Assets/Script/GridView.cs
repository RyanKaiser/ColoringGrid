using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GridView : MonoBehaviour
{
    [SerializeField] private float _gridSpacing = 0.1f;
    [SerializeField] private GameObject _tilePrefab;

    private TileView[,] _tileViews;
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
}