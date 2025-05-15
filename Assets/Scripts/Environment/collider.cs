using UnityEngine;

public class AutoBoxColliderGenerator : MonoBehaviour
{
    void Start()
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mr in meshRenderers)
        {
            GameObject obj = mr.gameObject;

            if (!obj.GetComponent<Collider>())
            {
                BoxCollider box = obj.AddComponent<BoxCollider>();
                box.center = mr.bounds.center - obj.transform.position;
                box.size = mr.bounds.size;
            }
        }
    }
}
