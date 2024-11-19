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
        _gridView.OnDragEnd += () =>
        {
            _model.EndAction();
            UpdateActionButtons();
        };

        void HandleTileSelection(int x, int y)
        {
            _model.UpdateTileColor(x, y);
            _gridView.UpdateTileColor(x, y, _model.CurrentColor);
        }

        void OnUndoButtonClicked()
        {
            _model.Undo();
            _gridView.UpdateGridColors(_model.Tiles);
            UpdateActionButtons();
        }

        void OnRedoButtonClicked()
        {
            model.Redo();
            _gridView.UpdateGridColors(_model.Tiles);
            UpdateActionButtons();
        }

        void UpdateActionButtons()
        {
            _actionView.UndoButton.interactable = _model.CanUndo();
            _actionView.RedoButton.interactable = _model.CanRedo();
        }
    }
}