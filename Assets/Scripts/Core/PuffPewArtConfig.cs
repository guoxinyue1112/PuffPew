using UnityEngine;

[CreateAssetMenu(fileName = "PuffPewArtConfig", menuName = "PuffPew/Art Config")]
public class PuffPewArtConfig : ScriptableObject
{
    [Header("Matched Paths")]
    public string backgroundPath;
    public string playerTexturePath;
    public string enemyPath;
    public string bulletPath;
    public string pistolPath;
    public string axePath;
    public string bombPath;

    [Header("Sprites")]
    public Sprite backgroundSprite;
    public Sprite playerLeftSprite;
    public Sprite playerRightSprite;
    public Sprite enemySprite;
    public Sprite bulletSprite;
    public Sprite pistolSprite;
    public Sprite axeSprite;
    public Sprite bombSprite;
}
