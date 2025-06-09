using UnityEngine;

public class gunMechanism : MonoBehaviour
{
    [Header("Gun Settings")]
    public float range = 100f;
    public float damage = 10f;
    public Camera playerCamera;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public LayerMask hitLayers;

    public gunMechanismInput inputScript;

    void Update()
    {
        if (inputScript == null)
        {
            Debug.LogWarning("Input script not assigned to gunMechanism.");
            return;
        }
        if (inputScript.IsAiming)
        {
            Debug.Log("Aiming...");
        }
        else if (inputScript.IsReloading)
        {
            Debug.Log("Reloading...");
        }
        else if (inputScript.IsShooting && inputScript.IsAiming)
        {
            // Handle shooting logic here
            Debug.Log("Shooting...");
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        RaycastHit hit;
        if (playerCamera == null)
        {
            Debug.LogWarning("Player Camera not assigned to gunMechanism.");
            return;
        }
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range, hitLayers))
        {
            Debug.Log($"Hit {hit.transform.name} for {damage} damage.");
            // Example: apply damage if the object has a health script
            var health = hit.transform.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            if (impactEffect != null)
            {
                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 1f);
            }
        }
    }
}
