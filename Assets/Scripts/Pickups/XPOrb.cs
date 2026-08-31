using UnityEngine;

public class XPOrb : MonoBehaviour
{
    private int xpAmount;

    public static void Create(Vector3 position, int amount)
    {
        GameObject orbObject = new("XPOrb");
        orbObject.transform.position = position;
        orbObject.transform.localScale = new Vector3(0.35f, 0.35f, 1f);

        SpriteRenderer spriteRenderer = orbObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = RuntimeSpriteFactory.GetCircleSprite();
        spriteRenderer.color = new Color(0.45f, 1f, 0.55f);

        CircleCollider2D collider2D = orbObject.AddComponent<CircleCollider2D>();
        collider2D.isTrigger = true;

        XPOrb orb = orbObject.AddComponent<XPOrb>();
        orb.xpAmount = amount;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerExperience playerExperience = other.GetComponent<PlayerExperience>();
        if (playerExperience == null || GameManager.Instance.IsTerminalState)
        {
            return;
        }

        playerExperience.AddExperience(xpAmount);
        Destroy(gameObject);
    }
}
