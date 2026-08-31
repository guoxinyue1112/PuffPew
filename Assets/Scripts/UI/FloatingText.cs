using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float lifetime = 0.8f;
    private Vector3 velocity = new(0f, 1.2f, 0f);

    public void Initialize(string message, Color color)
    {
        textMesh = gameObject.AddComponent<TextMeshPro>();
        textMesh.text = message;
        textMesh.fontSize = 4f;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.color = color;
        textMesh.sortingOrder = 100;
    }

    private void Update()
    {
        transform.position += velocity * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
