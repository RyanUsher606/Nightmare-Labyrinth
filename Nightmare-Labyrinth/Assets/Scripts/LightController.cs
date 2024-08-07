using UnityEngine;

public class EndLightController : MonoBehaviour
{
    public Light endLight;
    public Transform player;
    public Color farColor;
    public Color closeColor;
    public float maxDistance; // Adjust this based on your maze size

    private Vector3 endPosition = new Vector3(29f, 0f, 29f);

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(player.position, endPosition);
            float t = Mathf.Clamp01(1 - (distance / maxDistance));

            print(1 - (distance / maxDistance));

            if (t > 0)
            {
                print(t);
                endLight.color = Color.Lerp(farColor, closeColor, t);
            }
        }
    }
}