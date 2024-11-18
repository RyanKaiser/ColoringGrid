using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridView _gridView;
    [SerializeField] private PaletteView _paletteView;
    [SerializeField] private int _width = 16;
    [SerializeField] private int _height = 9;
    [SerializeField] private ColorSet _colors;


    private GridController _gridController;

    void Start()
    {
        GridModel model = new GridModel(_width, _height, _colors);
        GridView view = _gridView;
        PaletteView paletteView = _paletteView;
        _gridController = new GridController(model, view, paletteView);
    }
}