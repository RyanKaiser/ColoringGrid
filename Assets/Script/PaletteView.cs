using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PaletteView : MonoBehaviour
{
    [SerializeField] private Image _currentColorImage;
    [SerializeField] private ColorSet _colorSet;
    [SerializeField] private GameObject _colorButtonPrefab;
    [SerializeField] private Transform _paletteContainer;
    [SerializeField] private DynamicGridLayout _dynamicGridLayout;

    private Color _currentColor;
    public event Action<Color> OnColorSelected;

    void OnRectTransformDimensionsChange()
    {
        _dynamicGridLayout.AdjustCellSize();
    }

    void OnValidate()
    {
        _dynamicGridLayout.AdjustCellSize();
    }

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
        }
    }
}