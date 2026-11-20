using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [Header("Sonidos de Pasos")]
    public AudioClip[] walkFootsteps; // Array de clips para la caminata
    public AudioClip[] runFootsteps;  // Array de clips para la carrera

    private AudioSource audioSource;
    private Animator animator;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Método que llamaremos desde los eventos de la animación de CAMINAR
    public void PlayWalkFootstep()
    {
        if (walkFootsteps.Length > 0 && audioSource != null)
        {
            // Elige un sonido de paso al azar para que no suene repetitivo
            AudioClip clip = walkFootsteps[Random.Range(0, walkFootsteps.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    // Método que llamaremos desde los eventos de la animación de CORRER
    public void PlayRunFootstep()
    {
        if (runFootsteps.Length > 0 && audioSource != null)
        {
            AudioClip clip = runFootsteps[Random.Range(0, runFootsteps.Length)];
            audioSource.PlayOneShot(clip, 1.2f); // Un poco más fuerte al correr
        }
    }
}