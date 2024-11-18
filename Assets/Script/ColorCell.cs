using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class ColorCell : MonoBehaviour
{
    [SerializeField] private Color _baseColor;
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    // [SerializeField] private GameObject _highlight;

    // public event Action<int, int> OnClicked;
    public Button Button => _button;

    // public event Action<int, int> OnCursorEntered;
    // public event Action OnCursorExited;
    // public void Highlight(bool flag) => _highlight.SetActive(flag);
    // public void OnPointerClick() => OnTileClicked?.Invoke(_x, _y);
    // public void Init(int x, int y, Action<int, int> onTileClicked)
    // {
    //     _x = x;
    //     _y = y;
    //     OnTileClicked = onTileClicked;
    //
    //     _spriteRenderer.color = _baseColor;
    //     _highlight.SetActive(false);
    // }


    public void SetColor(Color color)
    {
        _baseColor = color;
        _image.color = _baseColor;
    }

    // public void UpdateVisual()
    // {
    //     _spriteRenderer.color = _baseColor;
    // }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     Debug.Log($"OnPointerEnter {_x}{_y}");
    //     _highlight.SetActive(true);
    //     // OnCursorEntered?.Invoke(_x, _y);
    // }
    //
    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     _highlight.SetActive(false);
    //     // OnCursorExited?.Invoke();
    // }

    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     OnTileClicked?.Invoke(_x, _y);
    // }




    // public void OnDrag(PointerEventData eventData)
    //  {
    //      OnTileClicked?.Invoke(_x, _y);
    //
    //  }
}