using UnityEngine;

public class GridController// : MonoBehaviour
{
    private GridModel gridModel;
    private GridView gridView;

    public Color selectedColor;  // 현재 선택된 색상

    
    public GridController(GridModel model, GridView view)
    {
        this.gridModel = model;
        this.gridView = view;
        
        gridView.InitializeGrid(gridModel.width, gridModel.height);
    }

    // void Update()
    // {
    //     HandleMouseInput();
    // }
    //
    // private void HandleMouseInput()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //         int x = Mathf.FloorToInt(mousePosition.x);
    //         int y = Mathf.FloorToInt(mousePosition.y);
    //
    //         if (gridModel.GetTile(x, y) != null)
    //         {
    //             gridModel.SetTileColor(x, y, selectedColor);  // 모델 업데이트
    //             gridView.UpdateTileColor(x, y, selectedColor);  // 뷰 업데이트
    //         }
    //     }
    // }

    // Undo/Redo 버튼 처리
    public void OnUndoButtonClicked()
    {
        gridModel.Undo();
        gridView.UpdateGridColors();  // 뷰 업데이트
    }

    public void OnRedoButtonClicked()
    {
        gridModel.Redo();
        gridView.UpdateGridColors();  // 뷰 업데이트
    }
}