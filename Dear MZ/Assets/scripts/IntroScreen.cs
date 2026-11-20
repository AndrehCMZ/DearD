using UnityEngine;
using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class IntroScreen : MonoBehaviour
{
    [Header("Referencias de UI")]
    public CanvasGroup blackScreenGroup; // Asigna FondoNegro (que contiene el Canvas Group)
    public CanvasGroup textGroup;         // Asigna TextoIntro (agrégale también un Canvas Group)

    [Header("Configuración de Tiempos")]
    public float blackScreenDuration = 2f; // Tiempo que dura la primera pantalla negra sola (en segundos)
    public float textFadeInSpeed = 1.5f;   // Velocidad con la que aparece el texto
    public float fadeOutSpeed = 1.5f;       // Velocidad con la que desaparece todo al hacer clic

    private bool canClickToStart = false;
    private bool isFadingOut = false;

    void Start()
    {
        // Al inicio: fondo negro visible, texto completamente invisible
        if (blackScreenGroup != null) blackScreenGroup.alpha = 1f;
        if (textGroup != null) textGroup.alpha = 0f;

        // Inicia la secuencia automática
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        // 1. Mantiene la pantalla negra en silencio durante el tiempo configurado
        yield return new WaitForSecondsRealtime(blackScreenDuration);

        // 2. Transición suave para que el texto aparezca poco a poco
        if (textGroup != null)
        {
            while (textGroup.alpha < 1f)
            {
                textGroup.alpha += Time.unscaledDeltaTime * textFadeInSpeed;
                yield return null;
            }
        }

        // 3. Permite que el jugador haga clic para avanzar
        canClickToStart = true;
    }

    void Update()
    {
        if (!canClickToStart || isFadingOut) return;

        bool keyPressed = false;

        #if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) keyPressed = true;
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) keyPressed = true;
        #endif

        #if ENABLE_LEGACY_INPUT_MANAGER
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown) keyPressed = true;
        #endif

        if (keyPressed)
        {
            StartCoroutine(FadeOutScreen());
        }
    }

    IEnumerator FadeOutScreen()
    {
        isFadingOut = true;

        // Desvanece todo el fondo y el texto hacia el juego (abrir los ojos)
        if (blackScreenGroup != null)
        {
            while (blackScreenGroup.alpha > 0)
            {
                blackScreenGroup.alpha -= Time.unscaledDeltaTime * fadeOutSpeed;
                yield return null;
            }
        }

        gameObject.SetActive(false);
    }
}