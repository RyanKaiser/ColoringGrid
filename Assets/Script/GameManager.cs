using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridView _gridView;
    [SerializeField] private PaletteView _paletteView;
    [SerializeField] private ActionView _actionView;
    [SerializeField] private int _width = 16;
    [SerializeField] private int _height = 9;
    [SerializeField] private ColorSet _colors;


    private GridController _gridController;

    void Start()
    {
        GridModel model = new GridModel(_width, _height, _colors);
        _gridController = new GridController(model, _gridView, _paletteView, _actionView);
    }
}