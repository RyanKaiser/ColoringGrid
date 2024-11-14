using UnityEngine;
using System.Collections.Generic;

public class GridModel
{
    public int width;
    public int height;
    private TileModel[,] tiles;

    private Stack<Action> undoStack = new Stack<Action>();
    private Stack<Action> redoStack = new Stack<Action>();

    public GridModel(int width, int height)
    {
        this.width = width;
        this.height = height;
        tiles = new TileModel[width, height];
        InitializeGrid();
    }

    // 그리드 초기화
    private void InitializeGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = new TileModel(x, y, Color.white);  // 기본 색상 흰색
            }
        }
    }

    public TileModel GetTile(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return tiles[x, y];
        return null;
    }

    public void SetTileColor(int x, int y, Color color)
    {
        var tile = GetTile(x, y);
        if (tile != null)
        {
            Color previousColor = tile.color;
            tile.color = color;

            // Undo 스택에 액션 기록
            Action action = new Action(tile, previousColor, color);
            undoStack.Push(action);
            redoStack.Clear();  // 새로운 액션이 발생하면 redo 스택 초기화
        }
    }

    public void Undo()
    {
        if (undoStack.Count > 0)
        {
            Action lastAction = undoStack.Pop();
            lastAction.Undo();
            redoStack.Push(lastAction);
        }
    }

    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            Action lastAction = redoStack.Pop();
            lastAction.Redo();
            undoStack.Push(lastAction);
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

public class Action
{
    private TileModel tile;
    private Color oldColor;
    private Color newColor;

    public Action(TileModel tile, Color oldColor, Color newColor)
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
