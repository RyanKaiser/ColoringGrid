using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionView : MonoBehaviour
{
    [SerializeField] private Button _undoButton;
    [SerializeField] private Button _redoButton;

    public event Action OnUndo;
    public event Action OnRedo;

    void Start()
    {
        _undoButton.onClick.AddListener(() => OnUndo?.Invoke());
        _redoButton.onClick.AddListener(() => OnRedo?.Invoke());
    }
}
