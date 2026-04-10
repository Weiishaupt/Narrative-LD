using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    [Header("Configuration")]
    public float openAngle = 90f;        // Angle d'ouverture de la porte
    public float openSpeed = 180f;       // Vitesse d'ouverture (degrés par seconde)
    public float detectionRange = 3f;    // Distance de détection du joueur
    public string playerTag = "Player";  // Tag du joueur
    
    [Header("Options")]
    public bool autoClose = true;        // Fermeture automatique
    public float closeDelay = 2f;        // Délai avant fermeture
    public bool openToRight = true;      // true = tourne à droite, false = tourne à gauche
    
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpening = false;
    private bool isClosing = false;
    private float closeTimer = 0f;
    private Transform playerTransform;
    
    void Start()
    {
        // Enregistre la rotation fermée
        closedRotation = transform.localRotation;
        
        // Calcule la rotation ouverte sur l'axe Z
        float direction = openToRight ? -1 : 1;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle * direction);
        //                                              ↑
        //                                        Rotation sur Z
        
        // Cherche le joueur automatiquement
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }
    
    void Update()
    {
        // Cherche le joueur périodiquement si pas trouvé
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            if (player != null)
            {
                playerTransform = player.transform;
            }
            return;
        }
        
        // Vérifie la distance avec le joueur
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        bool playerInRange = distanceToPlayer <= detectionRange;
        
        // Gère l'ouverture/fermeture
        if (playerInRange && !isOpening && transform.localRotation == closedRotation)
        {
            OpenDoor();
        }
        else if (!playerInRange && autoClose && !isClosing && transform.localRotation != closedRotation && !isOpening)
        {
            StartClosing();
        }
        
        // Animation d'ouverture
        if (isOpening)
        {
            transform.localRotation = Quaternion.RotateTowards(
                transform.localRotation, 
                openRotation, 
                openSpeed * Time.deltaTime
            );
            
            if (transform.localRotation == openRotation)
            {
                isOpening = false;
                closeTimer = closeDelay;
            }
        }
        
        // Animation de fermeture
        if (isClosing)
        {
            transform.localRotation = Quaternion.RotateTowards(
                transform.localRotation, 
                closedRotation, 
                openSpeed * Time.deltaTime
            );
            
            if (transform.localRotation == closedRotation)
            {
                isClosing = false;
            }
        }
        
        // Timer avant fermeture
        if (autoClose && !isOpening && !isClosing && transform.localRotation != closedRotation)
        {
            closeTimer -= Time.deltaTime;
            if (closeTimer <= 0)
            {
                StartClosing();
            }
        }
    }
    
    void OpenDoor()
    {
        isOpening = true;
        isClosing = false;
    }
    
    void StartClosing()
    {
        isClosing = true;
        isOpening = false;
    }
    
    // Pour visualiser la zone de détection dans l'éditeur
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}