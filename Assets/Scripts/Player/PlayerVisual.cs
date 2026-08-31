using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerController))]
public class PlayerVisual : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;
    private bool facingRight = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        ApplyFacingSprite(forceRight: true);
        spriteRenderer.sortingOrder = 10;

        if (PuffPewArt.HasSprite(spriteRenderer.sprite))
        {
            spriteRenderer.color = Color.white;
            PuffPewArt.SetUniformWorldSize(transform, spriteRenderer.sprite, 1.65f);
        }
    }

    private void LateUpdate()
    {
        Vector2 moveInput = playerController.CurrentMoveInput;
        if (moveInput.x > 0.01f)
        {
            facingRight = true;
        }
        else if (moveInput.x < -0.01f)
        {
            facingRight = false;
        }

        ApplyFacingSprite(facingRight);
    }

    private void ApplyFacingSprite(bool forceRight)
    {
        Sprite nextSprite = forceRight ? PuffPewArt.GetPlayerRightSprite() : PuffPewArt.GetPlayerLeftSprite();
        if (nextSprite != null)
        {
            spriteRenderer.sprite = nextSprite;
        }
    }
}
