// using UnityEngine;
// using UnityEngine.AI;

// namespace Invector.vCharacterController
// {
//     [RequireComponent(typeof(NavMeshAgent))]
//     public class SimpleAIControl : MonoBehaviour
//     {
//         [Header("Target Settings")]
//         public Transform player;
//         public float minDistanceToFollow = 2f;
//         public bool alwaysFaceTarget = true;

//         [Header("Movement Settings")]
//         public float moveSpeed = 3f;
//         public float rotationSpeed = 10f;
//         public float updatePathInterval = 0.5f;
        
//         // Private variables
//         private NavMeshAgent navAgent;
//         private vThirdPersonController controller;
//         private float nextPathUpdate;
//         private bool originalRootMotion;

//         void Start()
//         {
//             // Get components
//             navAgent = GetComponent<NavMeshAgent>();
//             controller = GetComponent<vThirdPersonController>();
            
//             // Disable any conflicting components
//             vThirdPersonInput inputComponent = GetComponent<vThirdPersonInput>();
//             if (inputComponent != null)
//                 inputComponent.enabled = false;
            
//             // Save original root motion setting
//             originalRootMotion = controller.useRootMotion;
            
//             // Setup controller
//             if (controller != null)
//             {
//                 controller.Init();
//                 controller.isGrounded = true;
//                 controller.lockMovement = false;
//                 controller.lockRotation = false;
//             }
            
//             // Configure NavMeshAgent for best results with Invector
//             navAgent.updateRotation = false;
//             navAgent.updatePosition = false;
//             navAgent.stoppingDistance = minDistanceToFollow;
//             navAgent.speed = moveSpeed;
            
//             // Initial path
//             if (player != null)
//                 navAgent.SetDestination(player.position);
                
//             // Configure animator
//             Animator animator = GetComponent<Animator>();
//             if (animator != null)
//             {
//                 animator.enabled = true;
//                 animator.updateMode = AnimatorUpdateMode.Normal;
//             }
//         }

//         void Update()
//         {
//             if (player == null)
//                 return;
                
//             float distToPlayer = Vector3.Distance(transform.position, player.position);
//             bool shouldUpdatePath = Time.time >= nextPathUpdate;
            
//             // Update path to player
//             if (shouldUpdatePath)
//             {
//                 nextPathUpdate = Time.time + updatePathInterval;
//                 navAgent.SetDestination(player.position);
                
//                 // For debugging
//                 Debug.DrawLine(transform.position + Vector3.up, player.position + Vector3.up, Color.yellow, 0.5f);
//             }
            
//             // Should the AI move?
//             bool shouldMove = distToPlayer > minDistanceToFollow;
            
//             // Calculate desired movement
//             Vector3 desiredVelocity = navAgent.desiredVelocity;
//             if (desiredVelocity.magnitude < 0.1f && navAgent.hasPath)
//             {
//                 // Use path corners if velocity is too small
//                 if (navAgent.path.corners.Length > 1)
//                 {
//                     Vector3 nextCorner = navAgent.path.corners[1];
//                     desiredVelocity = (nextCorner - transform.position).normalized * moveSpeed;
//                 }
//             }
            
//             // Set controller input based on NavMesh desired velocity
//             if (shouldMove)
//             {
//                 // Convert world direction to local
//                 Vector3 localDesiredVelocity = transform.InverseTransformDirection(desiredVelocity.normalized);
                
//                 // Apply to controller input with INCREASED magnitude for more reliable movement
//                 controller.input = new Vector3(
//                     localDesiredVelocity.x, 
//                     0f, 
//                     localDesiredVelocity.z
//                 ) * 1.25f;  // Increase input strength by 25%
                
//                 // Ensure input is strong enough to move
//                 if (controller.input.magnitude < 0.5f)
//                     controller.input = controller.input.normalized * 0.75f;
                    
//                 // Debug visualization
//                 Debug.DrawRay(transform.position + Vector3.up, desiredVelocity.normalized, Color.blue, 0.1f);
//             }
//             else
//             {
//                 controller.input = Vector3.zero;
//             }
            
//             // Face target if needed
//             if (alwaysFaceTarget)
//             {
//                 Vector3 dirToTarget = player.position - transform.position;
//                 dirToTarget.y = 0;
                
//                 if (dirToTarget.magnitude > 0.1f)
//                 {
//                     if (shouldMove)
//                         controller.RotateToDirection(dirToTarget, rotationSpeed);
//                     else
//                         transform.rotation = Quaternion.Slerp(transform.rotation, 
//                             Quaternion.LookRotation(dirToTarget), 
//                             rotationSpeed * Time.deltaTime);
//                 }
//             }
            
//             // Set values for animation
//             if (shouldMove)
//             {
//                 // Force higher input magnitude for animations
//                 controller.inputMagnitude = Mathf.Max(controller.inputMagnitude, 0.5f);
//             }
            
//             // Sync positions with NavMeshAgent
//             navAgent.nextPosition = transform.position;
            
//             // Update animations
//             controller.isGrounded = true;
//             controller.UpdateAnimator();
//         }
        
//         void FixedUpdate()
//         {
//             if (controller == null || player == null) return;
            
//             // Use FixedUpdate for character movement/physics
//             controller.UpdateMotor();
            
//             // Smoothly interpolate input for animations
//             controller.inputSmooth = Vector3.Lerp(controller.inputSmooth, controller.input, 8f * Time.fixedDeltaTime);
            
//             // Set the move direction from input
//             controller.moveDirection = controller.input;
            
//             // THIS IS THE KEY PART FROM THE ORIGINAL SCRIPT - Force movement
//             if (controller.input.magnitude > 0.1f)
//             {
//                 // Disable root motion temporarily during AI movement
//                 controller.useRootMotion = false;
                
//                 // Force move with direct control - CRITICAL LINE
//                 controller.MoveCharacter(controller.moveDirection);
                
//                 // Restore original setting after movement
//                 controller.useRootMotion = originalRootMotion;
//             }
//         }
//     }
// }