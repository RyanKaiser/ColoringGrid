using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PaletteView : MonoBehaviour
{
    [SerializeField] private Image _currentColorImage;
    [SerializeField] private ColorSet _colorSet;
    [SerializeField] private GameObject _colorButtonPrefab;
    [SerializeField] private Transform _paletteContainer;

    private Color _currentColor;

    public event Action<Color> OnColorSelected;


    public void Initialize(IReadOnlyList<Color> colors)
    {
        if (_colorSet == null || _colorButtonPrefab == null || _paletteContainer == null)
        {
            Debug.LogError("Missing references for PaletteView");
            return;
        }

        foreach (Transform child in _paletteContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Color color in colors)
        {
            GameObject buttonInstance = Instantiate(_colorButtonPrefab, _paletteContainer);
            ColorCell colorCell = buttonInstance.GetComponent<ColorCell>();

            colorCell.SetColor(color);
            colorCell.Button.onClick.AddListener(() =>
            {
                _currentColor = color;
                _currentColorImage.color = _currentColor;
                OnColorSelected?.Invoke(_currentColor);
            });


            // Image buttonImage = button.GetComponent<Image>();
            // if (buttonImage != null)
            // {
            //     buttonImage.color = color;
            // }
            //
            // // 클릭 이벤트 추가
            // button.onClick.AddListener(() => OnColorSelected(color));
        }
    }

    // void OnColorSelected(Color selectedColor)
    // {
    //     Debug.Log("Selected Color: " + selectedColor);
    //
    // }
}