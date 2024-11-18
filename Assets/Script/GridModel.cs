using UnityEngine;
using System.Collections.Generic;

public class GridModel
{
    private int _width;
    private int _height;
    private Color _currentColor;
    private readonly TileModel[,] _tiles;
    private List<Color> _paletteColors;

    private Stack<UserAction> _undoStack = new Stack<UserAction>();
    private Stack<UserAction> _redoStack = new Stack<UserAction>();

    public int Width => _width;
    public int Height => _height;
    public Color CurrentColor
    {
        get => _currentColor;
        set => _currentColor = value;
    }

    public IReadOnlyList<Color> PaletteColors => _paletteColors;

    public GridModel(int width, int height, ColorSet colorSet)
    {
        _width = width;
        _height = height;
        _tiles = new TileModel[width, height];

        InitializeGrid();
        InitializeColors(colorSet);

    }

    private void InitializeGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _tiles[x, y] = new TileModel(x, y, Color.white);
            }
        }
    }

    private void InitializeColors(ColorSet colorSet)
    {
        _paletteColors = new List<Color>();
        _paletteColors.AddRange(colorSet.colors);
        _currentColor = _paletteColors[0];
    }

    public TileModel GetTile(int x, int y)
    {
        if (x >= 0 && x < _width && y >= 0 && y < _height)
            return _tiles[x, y];
        return null;
    }

    public void UpdateTileColor(int x, int y) => SetTileColor(x, y, _currentColor);

    private void SetTileColor(int x, int y, Color color)
    {
        // Debug.Log($"SetTileColor({x}, {y}) called");
        var tile = GetTile(x, y);
        if (tile != null)
        {
            Color previousColor = tile.color;
            tile.color = color;

            UserAction userAction = new UserAction(tile, previousColor, color);
            _undoStack.Push(userAction);
            _redoStack.Clear();
            Debug.Log($"Undo Stack Count: {_undoStack.Count}, Redo Stack Count: {_redoStack.Count}");
        }
    }

    public int Undo()
    {
        if (_undoStack.Count > 0)
        {
            UserAction lastUserAction = _undoStack.Pop();
            lastUserAction.Undo();
            _redoStack.Push(lastUserAction);
        }
        return _undoStack.Count;
    }

    public int Redo()
    {
        if (_redoStack.Count > 0)
        {
            UserAction lastUserAction = _redoStack.Pop();
            lastUserAction.Redo();
            _undoStack.Push(lastUserAction);
        }
        return _redoStack.Count;
    }
}

public class TileModel
{
    public int x;
    public int y;
    public Color color;

    public TileModel(int xPos, int yPos, Color initialColor)
    {
        x = xPos;
        y = yPos;
        color = initialColor;
    }
}

public class UserAction
{
    private TileModel tile;
    private Color oldColor;
    private Color newColor;

    public UserAction(TileModel tile, Color oldColor, Color newColor)
    {
        this.tile = tile;
        this.oldColor = oldColor;
        this.newColor = newColor;
    }

    public void Undo()
    {
        tile.color = oldColor;
    }

    public void Redo()
    {
        tile.color = newColor;
    }
}
