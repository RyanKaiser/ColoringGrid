using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionView : MonoBehaviour
{
    [SerializeField] private Button _undoButton;
    [SerializeField] private Button _redoButton;

    public event Action OnUndo;
    public event Action OnRedo;

    public Button UndoButton => _undoButton;
    public Button RedoButton => _redoButton;


    void Start()
    {
        _undoButton.interactable = false;
        _redoButton.interactable = false;
        _undoButton.onClick.AddListener(() =>
        {
            _redoButton.interactable = true;
            OnUndo?.Invoke();
        });
        _redoButton.onClick.AddListener(() =>
        {
            _undoButton.interactable = true;
            OnRedo?.Invoke();
        });
    }
}
