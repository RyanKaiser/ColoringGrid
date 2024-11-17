using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class TileView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _offsetColor;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _highlight;

    private int _x;
    private int _y;
    public event Action<int, int> OnTileClicked;

    public void Init(int x, int y, Action<int, int> onTileClicked)
    {
        _x = x;
        _y = y;
        OnTileClicked = onTileClicked;

        _spriteRenderer.color = (x + y) % 2 == 0 ? _offsetColor : _baseColor;
        _highlight.SetActive(false);
    }


    public void SetColor(Color color)
    {
        _baseColor = color;
    }

    public void UpdateVisual()
    {
        _spriteRenderer.color = _baseColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _highlight.SetActive(false);
    }

     public void OnPointerClick(PointerEventData eventData)
     {
         UpdateVisual();
         OnTileClicked?.Invoke(_x, _y);

     }
}