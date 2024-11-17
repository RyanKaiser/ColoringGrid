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

        void HandleTileSelection(int x, int y)
        {
            _model.SetTileColor(x, y);

            _model.CurrentColor = new Color(Random.value, Random.value, Random.value);
            _view.UpdateTileColor(x, y, _model.CurrentColor);
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