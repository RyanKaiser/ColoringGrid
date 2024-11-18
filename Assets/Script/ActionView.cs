using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionView : MonoBehaviour
{
    [SerializeField] private Button _undoButton;
    [SerializeField] private Button _redoButton;

    // public Button UnndoButton => _undoButton;
    // public Button RedoButton => _redoButton;

    public event Action OnUndo;
    public event Action OnRedo;

    void Start()
    {
        _undoButton.onClick.AddListener(() => OnUndo?.Invoke());
        _redoButton.onClick.AddListener(() => OnRedo?.Invoke());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
