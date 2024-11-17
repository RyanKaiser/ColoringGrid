using System;
using UnityEngine;

public class GridView : MonoBehaviour
{
    [SerializeField] private float _gridSpacing = 0.1f;
    public GameObject tilePrefab;
    private TileView[,] tileViews;

    public event Action<Color> OnColorSelected;
    public event Action<int, int> OnTileSelected;

    public void InitializeGrid(int width, int height, Action<int, int> onTileClickedCallback)
    {
        tileViews = new TileView[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float x = i + _gridSpacing * i;
                float y = j + _gridSpacing * j;
                GameObject tileObj = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                TileView tileView = tileObj.GetComponent<TileView>();
                tileView.Init(i, j, onTileClickedCallback);
                tileViews[i, j] = tileView;
            }
        }

        UpdateGridColors();

        float centerX = (width + (width - 1) * _gridSpacing) / 2f - 0.5f;
        float centerY = (height + (height - 1) * _gridSpacing) / 2f - 0.5f;
        if (Camera.main != null)
            Camera.main.transform.position = new Vector3(centerX, centerY, -10f);
    }

    // private void HandleTileSelection(int x, int y)
    // {
    //     OnTileSelected?.Invoke(x, y); // 필요시 외부로 이벤트 전달
    // }

    public void UpdateTileColor(int x, int y, Color color)
    {
        if (tileViews[x, y] != null)
            tileViews[x, y].SetColor(color);
    }

    public void UpdateGridColors()
    {
        foreach (var tileView in tileViews)
        {
            if (tileView != null)
                tileView.UpdateVisual();
        }
    }
}