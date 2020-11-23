using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public float radius = 10;
    public float sphereCount = 50;

    void Start()
    {
        CreateCubes();
    }

    void CreateCubes()
    {
        for (int i = 0; i < sphereCount; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = transform.position + transform.forward * radius + UnityEngine.Random.insideUnitSphere * radius;
        }
    }
}
