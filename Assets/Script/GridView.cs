using UnityEngine;

public class GridView : MonoBehaviour
{
    public GameObject tilePrefab;  // 타일 프리팹
    private TileView[,] tileViews;

    // 그리드 초기화 및 타일 생성
    public void InitializeGrid(int width, int height)
    {
        tileViews = new TileView[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tileObj = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                TileView tileView = tileObj.GetComponent<TileView>();
                tileView.Init(x, y);
                tileViews[x, y] = tileView;
            }
        }
        
        UpdateGridColors();  // 초기 색상 업데이트
        if (Camera.main != null)
            Camera.main.transform.position = new Vector3(width / 2.0f - 0.5f, height / 2.0f - 0.5f, -10f);
    }

    // 타일 색상을 업데이트하는 함수
    public void UpdateTileColor(int x, int y, Color color)
    {
        if (tileViews[x, y] != null)
            tileViews[x, y].SetColor(color);
    }

    // 전체 그리드의 색상 업데이트
    public void UpdateGridColors()
    {
        foreach (var tileView in tileViews)
        {
            if (tileView != null)
                tileView.UpdateVisual();
        }
    }
}