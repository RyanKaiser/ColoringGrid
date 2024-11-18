using UnityEngine;


public class GridController
{
    private GridModel _model;
    private GridView _gridView;
    private PaletteView _paletteView;

    public GridController(GridModel model, GridView gridView, PaletteView paletteView)
    {
        _model = model;
        _gridView = gridView;
        _paletteView = paletteView;

        _gridView.InitializeGrid(_model.Width, _model.Height, HandleTileSelection);
        _paletteView.Initialize(_model.PaletteColors);
        _paletteView.OnColorSelected += c =>
        {
            _model.CurrentColor = c;
        };

        void HandleTileSelection(int x, int y)
        {
            Debug.Log($"HandleTileSelection: {x}, {y}  = {_model.CurrentColor}");
            _model.SetTileColor(x, y);

            // _model.CurrentColor = new Color(Random.value, Random.value, Random.value);
            _gridView.UpdateTileColor(x, y, _model.CurrentColor);
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