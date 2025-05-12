using UnityEngine;
using Invector.vCharacterController;

namespace Player
{
    public class PlayerMovementCustom : MonoBehaviour
    {
        private vThirdPersonAnimator characterAnimator;

        void Awake()
        {
            characterAnimator = GetComponent<vThirdPersonAnimator>();
        }

        void Update()
        {
            characterAnimator.isAiming = Input.GetMouseButton(1); // Right mouse button
            characterAnimator.isAimingAndShooting = Input.GetMouseButton(1) && Input.GetMouseButton(0); // Right + Left mouse
            characterAnimator.isReloading = Input.GetKey(KeyCode.R);
            characterAnimator.isCrouching = Input.GetKey(KeyCode.LeftControl);
        }
    }
}
