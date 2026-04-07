using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    // InGame Actions
    private Vector2 _movementDirectionInput = Vector2.zero;
    public Vector2 MoveDirInput => _movementDirectionInput;

    private bool _isJumpPressed = false;
    public bool IsJumpPressed => _isJumpPressed;

    private bool _isSprintHold = false;
    public bool SprintHold => _isSprintHold;

    private bool _isDashPressed = false;
    public bool IsDashPressed => _isDashPressed;

    private bool _isAttackBasicPressed = false;
    public bool IsAttackBasicPressed => _isAttackBasicPressed;

    private bool _isAttackSpecial1Pressed = false;
    public bool IsAttackSpecial1Pressed => _isAttackSpecial1Pressed;

    private bool _isInteractPressed = false;
    public bool InteractPressed => _isInteractPressed;

    // OnMenu Actions
    private Vector2 _navigateInput = Vector2.zero;
    public Vector2 NavigateInput => _navigateInput;

    private bool _isConfirmPressed = false;
    public bool IsConfirmPressed => _isConfirmPressed;

    private bool _isCancelPressed = false;
    public bool IsCancelPressed => _isCancelPressed;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();

        // Initially enable InGame, disable OnMenu
        _playerInputActions.InGame.Enable();
        _playerInputActions.OnMenu.Disable();

        SubscribeToAllActions();
    }

    private void OnDestroy()
    {
        UnsubscribeFromAllActions();
        _playerInputActions?.Disable();
        _playerInputActions?.Dispose();
    }

    private void Update()
    {
        ProcessMovementDirectionInput();
        ProcessNavigateInput();
    }

    private void ToggleOnMenuActions(bool onMenu)
    {
        if (onMenu)
        {
            _playerInputActions.InGame.Disable();
            _playerInputActions.OnMenu.Enable();
        }
        else
        {
            _playerInputActions.OnMenu.Disable();
            _playerInputActions.InGame.Enable();
        }
    }

    private void SubscribeToAllActions()
    {
        // InGame Subscriptions
        _playerInputActions.InGame.Jump.started += ProcessPerformedJumpInput;
        _playerInputActions.InGame.Jump.canceled += ProcessCanceledJumpInput;

        _playerInputActions.InGame.Sprint.performed += ProcessPerformedSprintInput;
        _playerInputActions.InGame.Sprint.canceled += ProcessCanceledSprintInput;

        _playerInputActions.InGame.Dash.started += ProcessPerformedDashInput;
        _playerInputActions.InGame.Dash.canceled += ProcessCanceledDashInput;

        _playerInputActions.InGame.AttackBasic.started += ProcessPerformedAttackLightInput;
        _playerInputActions.InGame.AttackBasic.canceled += ProcessCanceledAttackLightInput;

        _playerInputActions.InGame.AttackSpecial1.started += ProcessPerformedAttackSpecial1Input;
        _playerInputActions.InGame.AttackSpecial1.canceled += ProcessCanceledAttackSpecial1Input;

        _playerInputActions.InGame.Interact.started += ProcessPerformedInteractInput;
        _playerInputActions.InGame.Interact.canceled += ProcessCanceledInteractInput;

        // OnMenu Subscriptions
        _playerInputActions.OnMenu.Confirm.started += ProcessPerformedConfirmInput;
        _playerInputActions.OnMenu.Confirm.canceled += ProcessCanceledConfirmInput;

        _playerInputActions.OnMenu.Cancel.started += ProcessPerformedCancelInput;
        _playerInputActions.OnMenu.Cancel.canceled += ProcessCanceledCancelInput;
    }



    private void UnsubscribeFromAllActions()
    {
        // InGame Unsubscriptions
        _playerInputActions.InGame.Jump.started -= ProcessPerformedJumpInput;
        _playerInputActions.InGame.Jump.canceled -= ProcessCanceledJumpInput;

        _playerInputActions.InGame.Sprint.performed -= ProcessPerformedSprintInput;
        _playerInputActions.InGame.Sprint.canceled -= ProcessCanceledSprintInput;

        _playerInputActions.InGame.Dash.started -= ProcessPerformedDashInput;
        _playerInputActions.InGame.Dash.canceled -= ProcessCanceledDashInput;

        _playerInputActions.InGame.AttackBasic.started -= ProcessPerformedAttackLightInput;
        _playerInputActions.InGame.AttackBasic.canceled -= ProcessCanceledAttackLightInput;

        _playerInputActions.InGame.AttackSpecial1.started -= ProcessPerformedAttackSpecial1Input;
        _playerInputActions.InGame.AttackSpecial1.canceled -= ProcessCanceledAttackSpecial1Input;

        _playerInputActions.InGame.Interact.started -= ProcessPerformedInteractInput;
        _playerInputActions.InGame.Interact.canceled -= ProcessCanceledInteractInput;

        // OnMenu Unsubscriptions
        _playerInputActions.OnMenu.Confirm.started -= ProcessPerformedConfirmInput;
        _playerInputActions.OnMenu.Confirm.canceled -= ProcessCanceledConfirmInput;

        _playerInputActions.OnMenu.Cancel.started -= ProcessPerformedCancelInput;
        _playerInputActions.OnMenu.Cancel.canceled -= ProcessCanceledCancelInput;
    }

    // InGame Input Handlers
    private void ProcessPerformedJumpInput(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedJumpInputCoroutine()); }
    private void ProcessCanceledJumpInput(InputAction.CallbackContext context) { _isJumpPressed = false; }

    private void ProcessPerformedSprintInput(InputAction.CallbackContext context) { _isSprintHold = true; }
    private void ProcessCanceledSprintInput(InputAction.CallbackContext context) { _isSprintHold = false; }

    private void ProcessPerformedDashInput(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedDashInputCoroutine()); }
    private void ProcessCanceledDashInput(InputAction.CallbackContext context) { _isDashPressed = false; }

    private void ProcessPerformedAttackLightInput(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedAttackBasicInputCoroutine()); }
    private void ProcessCanceledAttackLightInput(InputAction.CallbackContext context) { _isAttackBasicPressed = false; }

    private void ProcessPerformedAttackSpecial1Input(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedAttackSpecial1InputCoroutine()); }
    private void ProcessCanceledAttackSpecial1Input(InputAction.CallbackContext context) { _isAttackSpecial1Pressed = false; }

    private void ProcessPerformedInteractInput(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedInteractInputCoroutine()); }
    private void ProcessCanceledInteractInput(InputAction.CallbackContext context) { _isInteractPressed = false; }

    // OnMenu Input Handlers
    private void ProcessPerformedConfirmInput(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedConfirmInputCoroutine()); }
    private void ProcessCanceledConfirmInput(InputAction.CallbackContext context) { _isConfirmPressed = false; }

    private void ProcessPerformedCancelInput(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedCancelInputCoroutine()); }
    private void ProcessCanceledCancelInput(InputAction.CallbackContext context) { _isCancelPressed = false; }

    // Continuous Value Reads
    private void ProcessMovementDirectionInput()
    {
        _movementDirectionInput = _playerInputActions.InGame.Movement.ReadValue<Vector2>();
    }

    private void ProcessNavigateInput()
    {
        _navigateInput = _playerInputActions.OnMenu.Navigate.ReadValue<Vector2>();
    }

    // One-frame button presses
    private IEnumerator ProcessPerformedJumpInputCoroutine()
    {
        _isJumpPressed = true;
        yield return null;
        if (this != null) _isJumpPressed = false;
    }

    private IEnumerator ProcessPerformedDashInputCoroutine()
    {
        _isDashPressed = true;
        yield return null;
        if (this != null) _isDashPressed = false;
    }

    private IEnumerator ProcessPerformedAttackBasicInputCoroutine()
    {
        _isAttackBasicPressed = true;
        yield return null;
        if (this != null) _isAttackBasicPressed = false;
    }

    private IEnumerator ProcessPerformedAttackSpecial1InputCoroutine()
    {
        _isAttackSpecial1Pressed = true;
        yield return null;
        if (this != null) _isAttackSpecial1Pressed = false;
    }

    private IEnumerator ProcessPerformedInteractInputCoroutine()
    {
        _isInteractPressed = true;
        yield return null;
        if (this != null) _isInteractPressed = false;
    }

    private IEnumerator ProcessPerformedConfirmInputCoroutine()
    {
        _isConfirmPressed = true;
        yield return null;
        if (this != null) _isConfirmPressed = false;
    }

    private IEnumerator ProcessPerformedCancelInputCoroutine()
    {
        _isCancelPressed = true;
        yield return null;
        if (this != null) _isCancelPressed = false;
    }
}