using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerController : MonoBehaviour
{
    public Vector2 CurrentMoveInput { get; private set; }

    private Rigidbody2D rb;
    private PlayerStats playerStats;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
    }

    public void InitializeBounds(float arenaMinX, float arenaMaxX, float arenaMinY, float arenaMaxY)
    {
        minX = arenaMinX;
        maxX = arenaMaxX;
        minY = arenaMinY;
        maxY = arenaMaxY;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.GameplayRunning)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        CurrentMoveInput = ReadMoveInput();
        if (CurrentMoveInput.sqrMagnitude > 1f)
        {
            CurrentMoveInput.Normalize();
        }

        Vector2 nextPosition = rb.position + CurrentMoveInput * playerStats.MoveSpeed * Time.fixedDeltaTime;
        nextPosition.x = Mathf.Clamp(nextPosition.x, minX, maxX);
        nextPosition.y = Mathf.Clamp(nextPosition.y, minY, maxY);
        rb.MovePosition(nextPosition);
    }

    private static Vector2 ReadMoveInput()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            Vector2 input = Vector2.zero;
            if (Keyboard.current.aKey.isPressed) input.x -= 1f;
            if (Keyboard.current.dKey.isPressed) input.x += 1f;
            if (Keyboard.current.sKey.isPressed) input.y -= 1f;
            if (Keyboard.current.wKey.isPressed) input.y += 1f;
            return input;
        }
#endif
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
