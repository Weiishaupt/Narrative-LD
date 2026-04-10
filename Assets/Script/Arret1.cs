using UnityEngine;

public class StopAnimator : MonoBehaviour
{
    public string idleTriggerName = "Idle"; // nom du trigger dans l'Animator

    private void OnTriggerEnter(Collider other)
    {
        Animator anim = other.GetComponent<Animator>();
        if (anim != null)
        {
            anim.speed = 0f; // met l'animation actuelle en pause
            anim.SetTrigger(idleTriggerName); // force le changement d'animation
        }
    }
}
