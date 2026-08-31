using UnityEngine;

public class BombExplosionEffect : MonoBehaviour
{
    private const float Duration = 0.28f;

    private SpriteRenderer spriteRenderer;
    private float elapsed;
    private Color baseColor;

    public static void Create(Vector3 position, float radius)
    {
        GameObject effectObject = new("BombExplosionEffect");
        effectObject.transform.position = position;

        SpriteRenderer renderer = effectObject.AddComponent<SpriteRenderer>();
        Sprite hitSprite = PuffPewArt.GetBombHitSprite();
        renderer.sprite = hitSprite != null ? hitSprite : RuntimeSpriteFactory.GetCircleSprite();
        renderer.color = hitSprite != null ? Color.white : new Color(1f, 0.78f, 0.32f, 0.9f);
        renderer.sortingOrder = 30;

        float visualSize = Mathf.Max(1.8f, radius * 2.2f);
        if (hitSprite != null)
        {
            PuffPewArt.SetUniformWorldSize(effectObject.transform, hitSprite, visualSize);
        }
        else
        {
            effectObject.transform.localScale = new Vector3(visualSize, visualSize, 1f);
        }

        BombExplosionEffect effect = effectObject.AddComponent<BombExplosionEffect>();
        effect.spriteRenderer = renderer;
        effect.baseColor = renderer.color;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        float progress = Mathf.Clamp01(elapsed / Duration);

        transform.localScale *= 1f + (1.6f * Time.deltaTime);

        if (spriteRenderer != null)
        {
            Color color = baseColor;
            color.a = Mathf.Lerp(baseColor.a, 0f, progress);
            spriteRenderer.color = color;
        }

        if (progress >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
