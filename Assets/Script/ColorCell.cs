using UnityEngine;
using UnityEngine.UI;

public class ColorCell : MonoBehaviour
{
    [SerializeField] private Color _baseColor;
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    public Button Button => _button;

    public void SetColor(Color color)
    {
        _baseColor = color;
        _image.color = _baseColor;
    }
}