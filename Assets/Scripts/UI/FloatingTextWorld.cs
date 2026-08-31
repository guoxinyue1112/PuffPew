using UnityEngine;

public class FloatingTextWorld : MonoBehaviour
{
    public void Spawn(string message, Vector3 worldPosition, Color color)
    {
        GameObject textObject = new("FloatingText");
        textObject.transform.position = worldPosition;
        FloatingText floatingText = textObject.AddComponent<FloatingText>();
        floatingText.Initialize(message, color);
    }
}
