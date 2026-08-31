using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private const float Lifetime = 12f;
    private const float SpinSpeed = 55f;

    private int healAmount;
    private float remainingLifetime;

    public static void Create(Vector3 position, int amount)
    {
        GameObject pickupObject = new("HealthPickup");
        pickupObject.transform.position = position;

        SpriteRenderer spriteRenderer = pickupObject.AddComponent<SpriteRenderer>();
        Sprite pickupSprite = PuffPewArt.GetHealthPickupSprite();
        spriteRenderer.sprite = pickupSprite != null ? pickupSprite : RuntimeSpriteFactory.GetSquareSprite();
        spriteRenderer.color = pickupSprite != null ? Color.white : new Color(1f, 0.45f, 0.55f, 1f);
        spriteRenderer.sortingOrder = 1;

        if (pickupSprite != null)
        {
            PuffPewArt.SetUniformWorldSize(pickupObject.transform, pickupSprite, 0.85f);
        }
        else
        {
            pickupObject.transform.localScale = new Vector3(0.45f, 0.45f, 1f);
        }

        CircleCollider2D collider2D = pickupObject.AddComponent<CircleCollider2D>();
        collider2D.isTrigger = true;
        collider2D.radius = 0.55f;

        HealthPickup pickup = pickupObject.AddComponent<HealthPickup>();
        pickup.healAmount = amount;
        pickup.remainingLifetime = Lifetime;
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.IsTerminalState)
        {
            return;
        }

        remainingLifetime -= Time.deltaTime;
        transform.Rotate(0f, 0f, SpinSpeed * Time.deltaTime);
        if (remainingLifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();
        if (stats == null || GameManager.Instance == null || GameManager.Instance.IsTerminalState)
        {
            return;
        }

        bool healed = stats.Heal(healAmount);
        if (healed)
        {
            GameManager.Instance.SpawnFloatingText($"+{healAmount}", transform.position + Vector3.up * 0.8f, new Color(0.45f, 1f, 0.55f));
        }

        if (!healed && stats.CurrentHP < stats.MaxHP)
        {
            return;
        }

        Destroy(gameObject);
    }
}
