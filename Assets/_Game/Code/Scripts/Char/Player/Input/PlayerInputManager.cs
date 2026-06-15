using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class PlayerInputManager : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    public Vector2 MoveDir { get; private set; }
    private ButtonState _jump;
    private ButtonState _sprint;
    private ButtonState _dash;
    private ButtonState _attackBasic1;
    private ButtonState _attackBasic2;
    private ButtonState _attackSpecial1;
    private ButtonState _attackSpecial2;
    private ButtonState _interact;
    private ButtonState _useItem;

    public Vector2 NavigateDir { get; private set; }
    private ButtonState _confirm;
    private ButtonState _cancel;

    public ButtonState Jump => _jump;
    public ButtonState Sprint => _sprint;
    public ButtonState Dash => _dash;
    public ButtonState AttackBasic1 => _attackBasic1;
    public ButtonState AttackBasic2 => _attackBasic2;
    public ButtonState AttackSpecial1 => _attackSpecial1;
    public ButtonState AttackSpecial2 => _attackSpecial2;
    public ButtonState Interact => _interact;
    public ButtonState Confirm => _confirm;
    public ButtonState Cancel => _cancel;


    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        EnableInGameControls();
    }

    private void OnEnable() => _playerInputActions?.Enable();
    private void OnDisable() => _playerInputActions?.Disable();
    private void OnDestroy() => _playerInputActions?.Dispose();

    public void EnableInGameControls()
    {
        _playerInputActions.InGame.Enable();
        _playerInputActions.OnMenu.Disable();
    }

    public void EnableOnMenuControls()
    {
        _playerInputActions.InGame.Disable();
        _playerInputActions.OnMenu.Enable();
    }

    private void Update()
    {
        // UI
        NavigateDir = _playerInputActions.OnMenu.Navigate.ReadValue<Vector2>();
        _confirm.Update(_playerInputActions.OnMenu.Confirm);
        _cancel.Update(_playerInputActions.OnMenu.Cancel);

        // In game
        MoveDir = _playerInputActions.InGame.Movement.ReadValue<Vector2>();
        _jump.Update(_playerInputActions.InGame.Jump);
        _sprint.Update(_playerInputActions.InGame.Sprint);
        _dash.Update(_playerInputActions.InGame.Dash);
        _attackBasic1.Update(_playerInputActions.InGame.AttackBasic1);
        _attackBasic2.Update(_playerInputActions.InGame.AttackBasic2);
        _attackSpecial1.Update(_playerInputActions.InGame.AttackSpecial1);
        _attackSpecial2.Update(_playerInputActions.InGame.AttackSpecial2);
        _interact.Update(_playerInputActions.InGame.Interact);
        _useItem.Update(_playerInputActions.InGame.UseItem);
    }

    [System.Serializable]
    public struct ButtonState
    {
        private bool _wasPressedLastFrame;
        private bool _isPressed;

        public bool Pressed => _isPressed && !_wasPressedLastFrame;
        public bool Hold => _isPressed;
        public bool Released => !_isPressed && _wasPressedLastFrame;

        public void Update(InputAction action)
        {
            _wasPressedLastFrame = _isPressed;
            _isPressed = action.IsPressed();
        }

        public void Reset()
        {
            _wasPressedLastFrame = false;
            _isPressed = false;
        }
    }
}