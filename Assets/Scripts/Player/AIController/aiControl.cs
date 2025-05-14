using UnityEngine;
using UnityEngine.AI;

namespace Invector.vCharacterController
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SimpleAIControl : MonoBehaviour
    {
        [Header("Target Settings")]
        public Transform player;
        public float minDistanceToFollow = 2f;
        public bool alwaysFaceTarget = true;

        [Header("Movement Settings")]
        public float moveSpeed = 1f;
        public float rotationSpeed = 10f;
        public float updatePathInterval = 0.5f;

        [Header("Animation Settings")]
        public bool useRunAnimation = true;
        public bool useSprintAnimation = true;
        [Range(0f, 1f)]
        public float runThreshold = 0.5f;
        [Range(0f, 1f)]
        public float sprintThreshold = 0.8f;
        private bool wasRunning = false;
        private bool wasSprinting = false;

        // Private variables
        private NavMeshAgent navAgent;
        private vThirdPersonController controller;
        private float nextPathUpdate;
        private bool originalRootMotion;

        void Start()
        {
            // Get components
            navAgent = GetComponent<NavMeshAgent>();
            controller = GetComponent<vThirdPersonController>();
            
            // Disable any conflicting components
            vThirdPersonInput inputComponent = GetComponent<vThirdPersonInput>();
            if (inputComponent != null)
                inputComponent.enabled = false;
            
            // Save original root motion setting
            originalRootMotion = controller.useRootMotion;
            
            // Setup controller
            if (controller != null)
            {
                controller.Init();
                controller.isGrounded = true;
                controller.lockMovement = false;
                controller.lockRotation = false;
            }
            
            // Configure NavMeshAgent for best results with Invector
            navAgent.updateRotation = false;
            navAgent.updatePosition = false;
            navAgent.stoppingDistance = minDistanceToFollow;
            navAgent.speed = moveSpeed;
            
            // Initial path
            if (player != null)
                navAgent.SetDestination(player.position);
                
            // Configure animator
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = true;
                animator.updateMode = AnimatorUpdateMode.Normal;
            }
        }

        void Update()
        {
            if (player == null)
                return;
                
            float distToPlayer = Vector3.Distance(transform.position, player.position);
            bool shouldUpdatePath = Time.time >= nextPathUpdate;
            
            // Update path to player
            if (shouldUpdatePath)
            {
                nextPathUpdate = Time.time + updatePathInterval;
                navAgent.SetDestination(player.position);
                
                // For debugging
                Debug.DrawLine(transform.position + Vector3.up, player.position + Vector3.up, Color.yellow, 0.5f);
            }
            
            // Should the AI move?
            bool shouldMove = distToPlayer > minDistanceToFollow;
            
            // Calculate desired movement
            Vector3 desiredVelocity = navAgent.desiredVelocity;
            if (desiredVelocity.magnitude < 0.1f && navAgent.hasPath)
            {
                // Use path corners if velocity is too small
                if (navAgent.path.corners.Length > 1)
                {
                    Vector3 nextCorner = navAgent.path.corners[1];
                    desiredVelocity = (nextCorner - transform.position).normalized * moveSpeed;
                }
            }
            
            // Set controller input based on NavMesh desired velocity
            if (shouldMove)
            {
                // Convert world direction to local
                Vector3 localDesiredVelocity = transform.InverseTransformDirection(desiredVelocity.normalized);
                
                // Calculate distance factor (0-1) for speed determination
                float distanceFactor = Mathf.Clamp01(distToPlayer / (minDistanceToFollow * 5f));
                
                // Determine speed mode based on distance
                float speedMultiplier = 1.0f;
                bool shouldRun = useRunAnimation && distanceFactor > runThreshold;
                bool shouldSprint = useSprintAnimation && distanceFactor > sprintThreshold;
                
                if (shouldSprint)
                    speedMultiplier = 1.5f; // Sprint speed
                else if (shouldRun)
                    speedMultiplier = 1.25f; // Run speed
                
                // Apply to controller input with speed-based magnitude
                controller.input = new Vector3(
                    localDesiredVelocity.x, 
                    0f, 
                    localDesiredVelocity.z
                ) * speedMultiplier;
                
                // Update sprint state for animation
                controller.isSprinting = shouldSprint;
                
                // Improve input magnitude to better match animations
                if (shouldSprint)
                    controller.inputMagnitude = Mathf.Lerp(controller.inputMagnitude, 1.5f, Time.deltaTime * 4f);
                else if (shouldRun)
                    controller.inputMagnitude = Mathf.Lerp(controller.inputMagnitude, 1.0f, Time.deltaTime * 3f);
                else
                    controller.inputMagnitude = Mathf.Lerp(controller.inputMagnitude, 0.5f, Time.deltaTime * 2f);
                
                // Debug visualization
                Debug.DrawRay(transform.position + Vector3.up, desiredVelocity.normalized, 
                             shouldSprint ? Color.red : (shouldRun ? Color.yellow : Color.blue), 0.1f);
                
                // Handle transitions between movement states for smoother animations
                if (shouldSprint != wasSprinting || shouldRun != wasRunning)
                {
                    // Update animator params right away for faster response
                    controller.UpdateAnimator();
                }
                
                wasRunning = shouldRun;
                wasSprinting = shouldSprint;
            }
            else
            {
                controller.input = Vector3.zero;
                controller.isSprinting = false;
                controller.inputMagnitude = Mathf.Lerp(controller.inputMagnitude, 0f, Time.deltaTime * 3f);
                wasRunning = false;
                wasSprinting = false;
            }
            
            // Face target if needed
            if (alwaysFaceTarget)
            {
                Vector3 dirToTarget = player.position - transform.position;
                dirToTarget.y = 0;
                
                if (dirToTarget.magnitude > 0.1f)
                {
                    if (shouldMove)
                        controller.RotateToDirection(dirToTarget, rotationSpeed);
                    else
                        transform.rotation = Quaternion.Slerp(transform.rotation, 
                            Quaternion.LookRotation(dirToTarget), 
                            rotationSpeed * Time.deltaTime);
                }
            }
            
            // Set values for animation
            if (shouldMove)
            {
                // Force higher input magnitude for animations
                controller.inputMagnitude = Mathf.Max(controller.inputMagnitude, 0.5f);
            }
            
            // Sync positions with NavMeshAgent
            navAgent.nextPosition = transform.position;
            
            // Update animations
            controller.isGrounded = true;
            controller.UpdateAnimator();

            // Handle situational animations based on situation
            HandleSituationalAnimations(distToPlayer);
        }
        
        void FixedUpdate()
        {
            if (controller == null || player == null) return;
            
            // Use FixedUpdate for character movement/physics
            controller.UpdateMotor();
            
            // Smoothly interpolate input for animations
            controller.inputSmooth = Vector3.Lerp(controller.inputSmooth, controller.input, 8f * Time.fixedDeltaTime);
            
            // Set the move direction from input
            controller.moveDirection = controller.input;
            
            // THIS IS THE KEY PART FROM THE ORIGINAL SCRIPT - Force movement
            if (controller.input.magnitude > 0.1f)
            {
                // Disable root motion temporarily during AI movement
                controller.useRootMotion = false;
                
                // Force move with direct control - CRITICAL LINE
                controller.MoveCharacter(controller.moveDirection);
                
                // Restore original setting after movement
                controller.useRootMotion = originalRootMotion;
            }
        }

        // Call this from Update when appropriate (e.g., when getting close to player)
        private void TriggerSituationalAnimation(string triggerName)
        {
            Animator animator = controller.GetComponent<Animator>();
            if (animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName(triggerName))
            {
                animator.SetTrigger(triggerName);
            }
        }

        // Add this to your Update method to automatically trigger situational animations
        private void HandleSituationalAnimations(float distToPlayer)
        {
            // Example: Trigger a reaction when getting very close to the player
            if (distToPlayer < minDistanceToFollow * 0.5f && !wasSprinting)
            {
                // This will work if your animator has these triggers defined
                if (Random.Range(0, 100) < 5) // 5% chance per frame when close
                {
                    if (alwaysFaceTarget)
                    {
                        // If we're using strafe mode and facing the player
                        TriggerSituationalAnimation("StrafeLeft");
                        // or TriggerSituationalAnimation("StrafeRight");
                    }
                }
            }
            
            // Example: Trigger a jump animation when encountering a height difference
            if (controller.isGrounded && controller.input.magnitude > 0.1f)
            {
                Ray ray = new Ray(transform.position + Vector3.up * 0.1f, transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1.5f))
                {
                    if (hit.normal.y < 0.7f && hit.normal.y > 0.1f) // Sloped surface ahead
                    {
                        controller.Jump(); // This will trigger jump animation if available
                    }
                }
            }
        }
    }
}