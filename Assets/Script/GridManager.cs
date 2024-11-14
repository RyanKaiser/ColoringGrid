using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;

    void Start() {
        _cam.transform.position = new Vector3(_width / 2.0f - 0.5f, _height / 2.0f - 0.5f, -10f);
        GenerateGrid();
    }
    
    void GenerateGrid() {
        _tiles = new Dictionary<Vector2, Tile>();
        
        for (int y = 0; y < _height; y++) {
            for (int x = 0; x < _width; x++) {
                var tile = Instantiate(_tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                tile.name = $"Tile {x} {y}";
                tile.Init((x + y) % 2 == 0);
                _tiles[new Vector2(x, y)] = tile;
            }
        }
    }

    public Tile GetTile(Vector2 position)
    {
        return _tiles.GetValueOrDefault(position);
    }
}
