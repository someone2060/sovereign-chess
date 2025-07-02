using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance {get; private set;}
    
    private InputSystem_Actions _actions;

    public event EventHandler OnSelectPerformed;
    public event EventHandler OnSelectCanceled;
    
    private void Awake()
    {
        Instance = this;
        
        _actions = new InputSystem_Actions();
        _actions.Gameplay.Enable();

        _actions.Gameplay.Select.performed += Select_performed;
        _actions.Gameplay.Select.canceled += Select_canceled;
    }

    private void Select_performed(InputAction.CallbackContext obj)
    {
        OnSelectPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Select_canceled(InputAction.CallbackContext obj)
    {
        OnSelectCanceled?.Invoke(this, EventArgs.Empty);
    }

    // Returns the mouse/touchscreen position in screen space
    public Vector2 GetPositionScreen()
    {
        return _actions.Gameplay.Position.ReadValue<Vector2>();
    }
    
    // Returns the mouse/touchscreen position in world space with given camera
    public Vector2 GetPositionWorld(Camera camera)
    {
        return camera.ScreenToWorldPoint(_actions.Gameplay.Position.ReadValue<Vector2>());
    }
}
