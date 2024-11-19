using UnityEngine;

public class GridController
{
    private GridModel _model;
    private GridView _gridView;
    private PaletteView _paletteView;
    private ActionView _actionView;

    public GridController(GridModel model, GridView gridView, PaletteView paletteView, ActionView actionView)
    {
        _model = model;
        _gridView = gridView;
        _paletteView = paletteView;
        _actionView = actionView;

        _gridView.InitializeGrid(_model.Width, _model.Height, HandleTileSelection);
        _paletteView.Initialize(_model.PaletteColors);
        _paletteView.OnColorSelected += c =>
        {
            _model.CurrentColor = c;
        };

        _actionView.OnRedo += OnRedoButtonClicked;
        _actionView.OnUndo += OnUndoButtonClicked;

        _gridView.OnDragStart += () => _model.BeginAction();
        _gridView.OnDragEnd += () => _model.EndAction();

        void HandleTileSelection(int x, int y)
        {
            Debug.Log($"HandleTileSelection: {x}, {y}  = {_model.CurrentColor}");

            _model.UpdateTileColor(x, y);
            _gridView.UpdateTileColor(x, y, _model.CurrentColor);
            _actionView.UndoButton.interactable = true;
        }

        void OnUndoButtonClicked()
        {
            int count = _model.Undo();
            _actionView.UndoButton.interactable = count > 0;
            _gridView.UpdateGridColors(_model.Tiles);
        }

        void OnRedoButtonClicked()
        {
            int count = _model.Redo();
            _actionView.RedoButton.interactable = count > 0;
            _gridView.UpdateGridColors(_model.Tiles);
        }
    }





}