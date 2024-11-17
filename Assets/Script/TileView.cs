
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class TileView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _offsetColor;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _highlight;

    private int _x;
    private int _y;
    // private InputAction _clickAction;
    public event Action<int, int> OnTileClicked;

    public void Init(int x, int y, Action<int, int> onTileClicked)
    {
        _x = x;
        _y = y;
        OnTileClicked = onTileClicked;

        _spriteRenderer.color = (x + y) % 2 == 0 ? _offsetColor : _baseColor;
        _highlight.SetActive(false);

        // _clickAction = new InputAction(type: InputActionType.Button, binding: "Click");
        // _clickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        // _clickAction.performed += OnClickPerformed;
        // _clickAction.Enable();
    }

    // private void OnClickPerformed(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //     {
    //         OnTileClicked.Invoke(_x, _y);
    //     }
    // }
    //
    // private void OnDestroy()
    // {
    //     _clickAction.Disable();
    //     _clickAction.performed -= OnClickPerformed;
    // }



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
         // Debug.Log("click");
         SetColor(new Color(Random.value, Random.value, Random.value));
         UpdateVisual();
         OnTileClicked?.Invoke(_x, _y);

     }
}