using UnityEngine;

public class GridController
{
    private GridModel _model;
    private GridView _view;


    public GridController(GridModel model, GridView view)
    {
        _model = model;
        _view = view;

        _view.InitializeGrid(_model.Width, _model.Height, HandleTileSelection);
        _view.OnColorSelected += color =>
        {
            _model.CurrentColor = color;
        };

        // _view.OnTileSelected += (x, y) =>
        // {
        //
        //     _model.SetTileColor(x, y);
        // };

        void HandleTileSelection(int x, int y)
        {
            _model.SetTileColor(x, y);
        }
    }




    // public void OnUndoButtonClicked()
    // {
    //     gridModel.Undo();
    //     gridView.UpdateGridColors();
    // }
    //
    // public void OnRedoButtonClicked()
    // {
    //     gridModel.Redo();
    //     gridView.UpdateGridColors();
    // }
}