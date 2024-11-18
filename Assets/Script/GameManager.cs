using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridView _gridView;
    [SerializeField] private int _width = 16;
    [SerializeField] private int _height = 9;

    private GridController _gridController;

    void Start()
    {
        GridModel model = new GridModel(_width, _height);
        GridView view = _gridView;
        _gridController = new GridController(model, view);
    }
}