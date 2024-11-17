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

    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     int x = Mathf.FloorToInt(mousePos.x);
        //     int y = Mathf.FloorToInt(mousePos.y);
        //
        //     // gridController.HandleTileClick(x, y);  // 컨트롤러에 로직 위임
        // }
    }
}