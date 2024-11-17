using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class GridModel
{
    [SerializeField] private int _space = 1;
    private int _width;
    private int _height;
    private Color _currentColor;
    private readonly TileModel[,] _tiles;

    private Stack<UserAction> _undoStack = new Stack<UserAction>();
    private Stack<UserAction> _redoStack = new Stack<UserAction>();

    public int Width => _width;
    public int Height => _height;
    public Color CurrentColor
    {
        get => _currentColor;
        set => _currentColor = value;
    }

    public GridModel(int width, int height)
    {
        _width = width;
        _height = height;
        _tiles = new TileModel[width, height];
        InitializeGrid();
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

    public TileModel GetTile(int x, int y)
    {
        if (x >= 0 && x < _width && y >= 0 && y < _height)
            return _tiles[x, y];
        return null;
    }

    public void SetTileColor(int x, int y) => SetTileColor(x, y, _currentColor);

    private void SetTileColor(int x, int y, Color color)
    {
        Debug.Log($"SetTileColor({x}, {y}) called");
        var tile = GetTile(x, y);
        if (tile != null)
        {
            Color previousColor = tile.color;
            tile.color = color;

            // Undo 스택에 액션 기록
            UserAction userAction = new UserAction(tile, previousColor, color);
            _undoStack.Push(userAction);
            _redoStack.Clear();  // 새로운 액션이 발생하면 redo 스택 초기화
        }
    }

    public void Undo()
    {
        if (_undoStack.Count > 0)
        {
            UserAction lastUserAction = _undoStack.Pop();
            lastUserAction.Undo();
            _redoStack.Push(lastUserAction);
        }
    }

    public void Redo()
    {
        if (_redoStack.Count > 0)
        {
            UserAction lastUserAction = _redoStack.Pop();
            lastUserAction.Redo();
            _undoStack.Push(lastUserAction);
        }
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
