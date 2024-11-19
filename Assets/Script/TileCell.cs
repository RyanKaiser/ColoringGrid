using System;
using UnityEngine;

public class TileCell : MonoBehaviour
{
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _offsetColor;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _highlight;

    private int _x;
    private int _y;
    public event Action<int, int> OnTileClicked;

    public void Highlight(bool flag) => _highlight.SetActive(flag);
    public void OnPointerClick() => OnTileClicked?.Invoke(_x, _y);
    public void Init(int x, int y, float gridRatio, Action<int, int> onTileClicked)
    {
        _x = x;
        _y = y;
        OnTileClicked = onTileClicked;
        float gridSpacing = 1f - gridRatio;

        _spriteRenderer.color = (x + y) % 2 == 0 ? _offsetColor : _baseColor;
        transform.localScale = new Vector3(gridSpacing, gridSpacing, gridSpacing);
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
 }