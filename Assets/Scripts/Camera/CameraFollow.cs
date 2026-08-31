using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 fixedPosition = new(0f, 0f, -10f);

    public void Initialize(Vector3 worldPosition)
    {
        fixedPosition = worldPosition;
        transform.position = fixedPosition;
    }

    private void LateUpdate()
    {
        transform.position = fixedPosition;
    }
}
