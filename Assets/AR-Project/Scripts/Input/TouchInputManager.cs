using UnityEngine;
using UnityEngine.InputSystem;

public class TouchInputManager : MonoBehaviour
{
    #region Variables

    private TouchControls touchControls;

    #endregion

    #region Methods

    private void Awake()
    {
        touchControls = new TouchControls();
    }

    private void OnEnable()
    {
        touchControls.Enable();
    }

    private void OnDisable()
    {
        touchControls.Disable();
    }

    private void Start()
    {
        touchControls.Touch.TouchInput.started += ctx => StartTouch(ctx);
        touchControls.Touch.TouchInput.canceled += ctx => EndTouch(ctx);
    }

    public void StartTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch Started " + context.ReadValue<Vector2>());
    }

    public void EndTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch Ended");
    }

    #endregion
}
