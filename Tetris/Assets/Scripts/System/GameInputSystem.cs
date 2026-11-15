using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputSystemをラップした入力管理ラッパークラス
/// </summary>
public class GameInputSystem : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _moveAction;

    [SerializeField]
    private InputActionReference _rotateAction;

    [SerializeField]
    private InputActionReference _dropAction;

    public Vector3 MoveVector { get; }
    public Vector3 RotateVector { get; }

    public event Action<Vector2> OnMove;
    public event Action OnRotate;
    public event Action OnDrop;

    private void OnEnable()
    {
        _moveAction.action.performed += HandleMove;
        _moveAction.action.canceled += HandleMove;

        _rotateAction.action.performed += HandleRotate;
        _dropAction.action.performed += HandleDrop;

        _moveAction.action.Enable();
        _rotateAction.action.Enable();
        _dropAction.action.Enable();
    }

    private void OnDisable()
    {
        _moveAction.action.performed -= HandleMove;
        _moveAction.action.canceled -= HandleMove;

        _rotateAction.action.performed -= HandleRotate;
        _dropAction.action.performed -= HandleDrop;

        _moveAction.action.Disable();
        _rotateAction.action.Disable();
        _dropAction.action.Disable();
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        var setVector = context.ReadValue<Vector2>();
        MoveVector.Set(setVector.y, 0.0f, setVector.x);
    }

    private void HandleRotate(InputAction.CallbackContext context)
    {
        var setVector = RotateVector.y + context.ReadValue<Vector2>().x;
        RotateVector.Set(0.0f, setVector, 0.0f);
    }

    private void HandleDrop(InputAction.CallbackContext context)
    {
        OnDrop?.Invoke();
    }
}
