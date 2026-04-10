// using UnityEngine;
//
// public class ClimbLadderCC : MonoBehaviour
// {
//     public float climbSpeed = 3f;
//     private bool isClimbing = false;
//
//     private CharacterController controller;
//     private PlayerMovement PlayerController; // ton script de déplacement normal
//
//     void Start()
//     {
//         controller = GetComponent<CharacterController>();
//         playerMovement = GetComponent<PlayerMovement>(); 
//     }
//
//     void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Echelle"))
//         {
//             isClimbing = true;
//             playerMovement.enabled = false; // désactive le mouvement normal
//         }
//     }
//
//     void OnTriggerExit(Collider other)
//     {
//         if (other.CompareTag("Echelle"))
//         {
//             isClimbing = false;
//             playerMovement.enabled = true; // réactive le mouvement normal
//         }
//     }
//
//     void Update()
//     {
//         if (isClimbing)
//         {
//             float vertical = Input.GetAxis("Vertical");
//
//             Vector3 climbDirection = new Vector3(0, vertical * climbSpeed, 0);
//
//             controller.Move(climbDirection * Time.deltaTime);
//         }
//     }
// }