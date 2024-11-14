using System;
using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _offsetColor;
    [SerializeField] private SpriteRenderer _spriteRenderer; 
    [SerializeField] private GameObject _highlight;

    public void Init(int x, int y)
    {
        _spriteRenderer.color = (x + y) % 2 == 0 ? _offsetColor : _baseColor;
        _highlight.SetActive(false);
    }
    
    public void SetColor(Color color)
    {
        
    }

    public void UpdateVisual()
    {
        
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
}