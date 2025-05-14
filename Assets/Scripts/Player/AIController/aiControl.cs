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

        [Header("Physics Settings")]
        [Range(1f, 20f)]
        public float accelerationRate = 8f;  // How quickly the AI reaches target speed
        [Range(1f, 20f)] 
        public float decelerationRate = 10f; // How quickly the AI slows down
        [Range(0f, 1f)]
        public float groundFriction = 0.6f;  // Friction coefficient for movement
        private Vector3 currentHorizontalVelocity = Vector3.zero;

        [Header("Dynamic Movement")]
        [Tooltip("Distance at which AI will start sprinting to catch up")]
        public float sprintDistance = 15f;
        [Tooltip("Distance at which AI will start running")]
        public float runDistance = 8f;
        [Tooltip("Distance at which AI will walk slowly/cautiously")]
        public float cautionDistance = 3f;
        [Tooltip("Speed multiplier when in caution zone (very close to target)")]
        [Range(0.1f, 1f)]
        public float cautionSpeedMultiplier = 0.5f;

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
                
                // ANTI-SLIDING FIX: Set gravity and ground settings
                controller.groundDistance = 0.1f;
                controller.groundMinDistance = 0.02f;
                controller.groundMaxDistance = 0.15f;
                controller.groundLayer = LayerMask.GetMask("Default");
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
                
                // ANTI-SLIDING FIX: Reset all animation parameters
                animator.SetFloat("InputHorizontal", 0);
                animator.SetFloat("InputVertical", 0);
                animator.SetFloat("InputMagnitude", 0);
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
                
                // DYNAMIC MOVEMENT: Calculate speed based on distance to player
                float speedMultiplier = 1.0f;
                bool shouldRun = false;
                bool shouldSprint = false;
                bool cautionMode = false;
                
                // Determine movement behavior based on distance
                if (distToPlayer > sprintDistance)
                {
                    // Far away - sprint to catch up quickly
                    speedMultiplier = 1.5f;
                    shouldSprint = true;
                }
                else if (distToPlayer > runDistance)
                {
                    // Medium distance - run
                    speedMultiplier = 1.25f;
                    shouldRun = true;
                }
                else if (distToPlayer < cautionDistance)
                {
                    // Very close - move cautiously
                    speedMultiplier = cautionSpeedMultiplier;
                    cautionMode = true;
                }
                else
                {
                    // Normal walking distance
                    speedMultiplier = 1.0f;
                }
                
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
                else if (cautionMode)
                    controller.inputMagnitude = Mathf.Lerp(controller.inputMagnitude, 0.3f, Time.deltaTime * 5f); // Slower animation for caution
                else
                    controller.inputMagnitude = Mathf.Lerp(controller.inputMagnitude, 0.7f, Time.deltaTime * 2f);
                
                // Debug visualization with color coding based on movement mode
                Color debugColor = Color.blue; // Default walking
                if (shouldSprint) debugColor = Color.red;
                else if (shouldRun) debugColor = Color.yellow;
                else if (cautionMode) debugColor = Color.green;
                
                Debug.DrawRay(transform.position + Vector3.up, desiredVelocity.normalized, debugColor, 0.1f);
                
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

            // ANTI-SLIDING FIX: Force proper ground detection
            if (controller != null)
            {
                // Can't call CheckGround() directly as it's protected
                // Instead, call UpdateMotor() which calls CheckGround() internally
                controller.UpdateMotor();
                
                // Ensure we're using proper friction parameters
                if (controller.isGrounded)
                {
                    // Apply a downward force to stay grounded
                    controller.verticalVelocity = -0.1f;  // Fixed: This should be a float, not Vector3
                    
                    // Sync positions with NavMeshAgent only when grounded
                    navAgent.nextPosition = transform.position;
                }
                
                // Update animations with correct grounded state
                controller.UpdateAnimator();
            }
        }
        
        void FixedUpdate()
        {
            if (controller == null || player == null) return;
            
            // Use FixedUpdate for character movement/physics
            controller.UpdateMotor();
            
            // DYNAMIC MOVEMENT: Adjust smoothing based on movement type
            float smoothFactor;
            if (wasSprinting)
                smoothFactor = 12f; // Quick response for sprinting
            else if (wasRunning)
                smoothFactor = 9f;  // Medium response for running
            else if (controller.inputMagnitude < 0.5f) 
                smoothFactor = 4f;  // Very smooth for caution movement
            else
                smoothFactor = 7f;  // Normal walking
            
            // Smoothly interpolate input for animations
            controller.inputSmooth = Vector3.Lerp(controller.inputSmooth, controller.input, smoothFactor * Time.fixedDeltaTime);
            
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

        // Update your HandleSituationalAnimations method:
        private void HandleSituationalAnimations(float distToPlayer)
        {
            // DYNAMIC MOVEMENT: Add more situation-specific animations
            
            // Very close to player - occasional dodging/strafing
            if (distToPlayer < cautionDistance)
            {
                // Lower chance when very close (5% per frame)
                if (Random.Range(0, 100) < 5 && !wasSprinting)
                {
                    if (alwaysFaceTarget)
                    {
                        // Randomly pick left or right strafe
                        string strafeDir = Random.Range(0, 2) == 0 ? "StrafeLeft" : "StrafeRight";
                        TriggerSituationalAnimation(strafeDir);
                    }
                }
            }
            // Medium distance - occasional defensive postures
            else if (distToPlayer < runDistance)
            {
                // Very rare chance (0.5% per frame) of special animation in medium range
                if (Random.Range(0, 1000) < 5)
                {
                    TriggerSituationalAnimation("Defend");
                }
            }
            
            // Jump over obstacles when moving at speed
            if (controller.isGrounded && controller.input.magnitude > 0.5f)
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