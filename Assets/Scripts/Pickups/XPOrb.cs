using UnityEngine;

public class XPOrb : MonoBehaviour
{
    private const float AutoCollectRadius = 3.5f;
    private const float AutoCollectSpeed = 11f;

    private static Transform cachedPlayerTransform;

    private int xpAmount;
    private PlayerExperience playerExperience;

    public static void Create(Vector3 position, int amount)
    {
        GameObject orbObject = new("XPOrb");
        orbObject.transform.position = position;
        orbObject.transform.localScale = new Vector3(0.35f, 0.35f, 1f);

        SpriteRenderer spriteRenderer = orbObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = RuntimeSpriteFactory.GetCircleSprite();
        spriteRenderer.color = new Color(0.45f, 1f, 0.55f);
        spriteRenderer.sortingOrder = 0;

        CircleCollider2D collider2D = orbObject.AddComponent<CircleCollider2D>();
        collider2D.isTrigger = true;

        XPOrb orb = orbObject.AddComponent<XPOrb>();
        orb.xpAmount = amount;
    }

    private void Awake()
    {
        if (cachedPlayerTransform == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                cachedPlayerTransform = playerObject.transform;
            }
        }

        if (cachedPlayerTransform != null)
        {
            playerExperience = cachedPlayerTransform.GetComponent<PlayerExperience>();
        }
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.IsTerminalState || !GameManager.Instance.GameplayRunning)
        {
            return;
        }

        if (cachedPlayerTransform == null)
        {
            return;
        }

        Vector3 toPlayer = cachedPlayerTransform.position - transform.position;
        float distance = toPlayer.magnitude;

        if (distance > AutoCollectRadius)
        {
            return;
        }

        if (distance <= 0.15f)
        {
            Collect();
            return;
        }

        Vector3 direction = toPlayer / Mathf.Max(distance, 0.0001f);
        transform.position += direction * AutoCollectSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerExperience hitPlayerExperience = other.GetComponent<PlayerExperience>();
        if (hitPlayerExperience == null || GameManager.Instance.IsTerminalState)
        {
            return;
        }

        playerExperience = hitPlayerExperience;
        Collect();
    }

    private void Collect()
    {
        if (playerExperience == null || GameManager.Instance == null || GameManager.Instance.IsTerminalState)
        {
            return;
        }

        playerExperience.AddExperience(xpAmount);
        Destroy(gameObject);
    }
}
