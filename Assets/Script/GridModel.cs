using UnityEngine;
using System.Collections.Generic;

public class GridModel
{
    private readonly int _width;
    private readonly int _height;
    private Color _currentColor;
    private readonly TileModel[,] _tiles;
    private List<Color> _paletteColors;

    private readonly Stack<UserActionGroup> _undoStack = new Stack<UserActionGroup>();
    private readonly Stack<UserActionGroup> _redoStack = new Stack<UserActionGroup>();
    private UserActionGroup _currentActionGroup;

    public int Width => _width;
    public int Height => _height;
    public Color CurrentColor
    {
        get => _currentColor;
        set => _currentColor = value;
    }
    public TileModel[,] Tiles => _tiles;

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

    private TileModel GetTile(int x, int y)
    {
        if (x >= 0 && x < _width && y >= 0 && y < _height)
            return _tiles[x, y];
        return null;
    }

    public void BeginAction()
    {
        _currentActionGroup = new UserActionGroup();
    }

    public void UpdateTileColor(int x, int y)
    {
        SetTileColor(x, y, _currentColor);
    }

    public void EndAction()
    {
        if (_currentActionGroup != null && !_currentActionGroup.IsEmpty())
        {
            _undoStack.Push(_currentActionGroup);
            _redoStack.Clear();
        }

        _currentActionGroup = null;
    }

    private void SetTileColor(int x, int y, Color color)
    {
        var tile = GetTile(x, y);
        if (tile == null) return;

        var previousColor = tile.color;
        if (previousColor == color) return;

        tile.color = color;

        if (_currentActionGroup == null) return;

        UserAction userAction = new UserAction(tile, previousColor, color);
        _currentActionGroup.Add(userAction);
    }

    public bool CanUndo() => _undoStack.Count > 0;
    public bool CanRedo() => _redoStack.Count > 0;

    public void Undo()
    {
        if (_undoStack.Count <= 0) return;

        UserActionGroup lastUserActionGroup = _undoStack.Pop();
        lastUserActionGroup.Undo();
        _redoStack.Push(lastUserActionGroup);
    }

    public void Redo()
    {
        if (_redoStack.Count <= 0) return;

        UserActionGroup lastUserActionGroup = _redoStack.Pop();
        lastUserActionGroup.Redo();
        _undoStack.Push(lastUserActionGroup);
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

public class UserActionGroup
{
   private List<UserAction> actions = new List<UserAction>();

   public void Add(UserAction action)
   {
       actions.Add(action);
   }

   public bool IsEmpty()
   {
       return actions.Count == 0;
   }

   public void Undo()
   {
       for (int i = actions.Count - 1; i >= 0; i--)
       {
           actions[i].Undo();
       }
   }

   public void Redo()
   {
       foreach (var action in actions)
       {
           action.Redo();
       }
   }
}