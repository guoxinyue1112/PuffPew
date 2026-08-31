using UnityEngine;

public static class RuntimeSpriteFactory
{
    private static Sprite squareSprite;
    private static Sprite circleSprite;

    public static Sprite GetSquareSprite()
    {
        if (squareSprite == null)
        {
            Texture2D texture = new(1, 1, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            squareSprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
        }

        return squareSprite;
    }

    public static Sprite GetCircleSprite()
    {
        if (circleSprite == null)
        {
            const int size = 64;
            Texture2D texture = new(size, size, TextureFormat.RGBA32, false);
            Vector2 center = new(size / 2f, size / 2f);
            float radius = size * 0.45f;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center);
                    texture.SetPixel(x, y, distance <= radius ? Color.white : Color.clear);
                }
            }

            texture.Apply();
            circleSprite = Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), size);
        }

        return circleSprite;
    }
}
