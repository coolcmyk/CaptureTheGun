using UnityEngine;
using Invector.vCharacterController;

public class gunMechanismInput : MonoBehaviour
{
    private vThirdPersonController controller;
    private Animator animator;

    //GetSet
    public bool IsAiming { get; private set; }
    public bool IsShooting { get; private set; }
    public bool IsReloading { get; private set; }

    void Awake()
    {
        controller = GetComponent<vThirdPersonController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Custom input logic
        IsAiming = Input.GetMouseButton(1);
        IsShooting = Input.GetMouseButton(0) && Input.GetMouseButton(1);
        IsReloading = Input.GetKey(KeyCode.R);

        // Set custom animator parameters (these must exist in your Animator Controller)
        animator.SetBool("isAiming", IsAiming);
        animator.SetBool("isShooting", IsAiming && IsShooting);
        animator.SetBool("isReloading", IsReloading);
        
        // Optionally, call controller methods or set controller fields if needed
        // controller.SomeCustomMethod();

        //Logging for debugging
        Debug.Log($"Is Aiming: {IsAiming}, Is Shooting: {IsShooting}, Is Reloading: {IsReloading}");
    }
}