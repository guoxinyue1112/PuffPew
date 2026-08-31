using UnityEngine;

public static class PuffPewArt
{
    private static PuffPewArtConfig cachedConfig;
    private static Sprite cachedBombHitSprite;
    private static Sprite cachedHealthPickupSprite;

    public static PuffPewArtConfig Config
    {
        get
        {
            if (cachedConfig == null)
            {
                cachedConfig = Resources.Load<PuffPewArtConfig>("PuffPewArtConfig");
            }

            return cachedConfig;
        }
    }

    public static Sprite GetBackgroundSprite()
    {
        return Config != null ? Config.backgroundSprite : null;
    }

    public static Sprite GetPlayerRightSprite()
    {
        if (Config == null)
        {
            return null;
        }

        return Config.playerRightSprite != null ? Config.playerRightSprite : Config.playerLeftSprite;
    }

    public static Sprite GetPlayerLeftSprite()
    {
        if (Config == null)
        {
            return null;
        }

        return Config.playerLeftSprite != null ? Config.playerLeftSprite : Config.playerRightSprite;
    }

    public static Sprite GetEnemySprite()
    {
        return Config != null ? Config.enemySprite : null;
    }

    public static Sprite GetBulletSprite()
    {
        return Config != null ? Config.bulletSprite : null;
    }

    public static Sprite GetPistolSprite()
    {
        return Config != null ? Config.pistolSprite : null;
    }

    public static Sprite GetAxeSprite()
    {
        return Config != null ? Config.axeSprite : null;
    }

    public static Sprite GetBombSprite()
    {
        return Config != null ? Config.bombSprite : null;
    }

    public static Sprite GetBombHitSprite()
    {
        if (cachedBombHitSprite == null)
        {
            cachedBombHitSprite = LoadNamedSprite("Bomb_hit", "Bomb_hit_0");
        }

        return cachedBombHitSprite;
    }

    public static Sprite GetHealthPickupSprite()
    {
        if (cachedHealthPickupSprite == null)
        {
            cachedHealthPickupSprite =
                LoadFirstAvailableSprite("HealthPack", "health_pack", "HealthPickup", "healthpickup", "medkit", "Medkit");
        }

        return cachedHealthPickupSprite;
    }

    public static bool HasSprite(Sprite sprite)
    {
        return sprite != null;
    }

    public static void SetUniformWorldSize(Transform target, Sprite sprite, float desiredMaxWorldSize)
    {
        if (target == null || sprite == null)
        {
            return;
        }

        Vector2 spriteSize = sprite.bounds.size;
        float maxSize = Mathf.Max(spriteSize.x, spriteSize.y);
        if (maxSize <= Mathf.Epsilon)
        {
            return;
        }

        float scale = desiredMaxWorldSize / maxSize;
        target.localScale = Vector3.one * scale;
    }

    public static void SetCoverWorldSize(Transform target, Sprite sprite, float worldWidth, float worldHeight)
    {
        if (target == null || sprite == null)
        {
            return;
        }

        Vector2 spriteSize = sprite.bounds.size;
        if (spriteSize.x <= Mathf.Epsilon || spriteSize.y <= Mathf.Epsilon)
        {
            return;
        }

        float scale = Mathf.Max(worldWidth / spriteSize.x, worldHeight / spriteSize.y);
        target.localScale = Vector3.one * scale;
    }

    private static Sprite LoadFirstAvailableSprite(params string[] resourceNames)
    {
        for (int i = 0; i < resourceNames.Length; i++)
        {
            Sprite sprite = Resources.Load<Sprite>(resourceNames[i]);
            if (sprite != null)
            {
                return sprite;
            }

            Sprite[] spriteSheet = Resources.LoadAll<Sprite>(resourceNames[i]);
            if (spriteSheet.Length > 0)
            {
                return spriteSheet[0];
            }
        }

        return null;
    }

    private static Sprite LoadNamedSprite(string resourceName, string preferredSpriteName)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(resourceName);
        Sprite fallback = null;
        for (int i = 0; i < sprites.Length; i++)
        {
            Sprite sprite = sprites[i];
            if (sprite == null)
            {
                continue;
            }

            if (fallback == null)
            {
                fallback = sprite;
            }

            if (sprite.name == preferredSpriteName)
            {
                return sprite;
            }
        }

        return fallback;
    }
}
