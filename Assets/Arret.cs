using UnityEngine;

public class StopAnim : MonoBehaviour
{
    public RuntimeAnimatorController newController; // ton controller "Normal"

    private void OnTriggerEnter(Collider other)
    {
        Animator anim = other.GetComponent<Animator>();
        if (anim != null)
        {
            // Change le controller en premier
            anim.runtimeAnimatorController = newController;

            // Optionnel : remettre la vitesse à 1 si besoin
            anim.speed = 1f;
        }
    }
}