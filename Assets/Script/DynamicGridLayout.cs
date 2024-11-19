using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DynamicGridLayout : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private RectTransform _parentRectTransform;

    public void AdjustCellSize()
    {
        float parentWidth = _parentRectTransform.rect.width;
        float spacing = _gridLayoutGroup.spacing.x;
        float padding = _gridLayoutGroup.padding.left + _gridLayoutGroup.padding.right;

        Debug.Log($"width: {parentWidth}, spacing: {spacing}, padding: {padding}");

        float cellWidth = (parentWidth - padding - spacing) / 2;

        _gridLayoutGroup.cellSize = new Vector2(cellWidth, cellWidth);
    }
}