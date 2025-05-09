using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CakeTrigger : MonoBehaviour
{
    public float fadeDuration = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Pausar el juego
            Time.timeScale = 0f;

            // Buscar el grupo visual
            Transform finalTextGroup = GameObject.Find("FinalText")?.transform;
            if (finalTextGroup == null)
            {
                Debug.LogError("❌ No se encontró FinalText en la jerarquía.");
                return;
            }

            Image fadeImage = finalTextGroup.Find("FadePanel")?.GetComponent<Image>();
            Text congratsText = finalTextGroup.Find("CongratulationsText")?.GetComponent<Text>();

            if (fadeImage == null || congratsText == null)
            {
                Debug.LogError("❌ No se encontró FadePanel o CongratulationsText dentro de FinalText.");
                return;
            }

            // Activar panel y texto manualmente porque las coroutines se congelan con Time.timeScale = 0
            fadeImage.color = new Color(0, 0, 0, 1); // Aparece inmediatamente
            congratsText.gameObject.SetActive(true);

            // Destruir el pastel
            Destroy(gameObject);
        }
    }




    private IEnumerator FadeAndShowText(Image fadePanel, Text congratulationsText)
    {
        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = new Color(0, 0, 0, 1);

        float duration = 2f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            fadePanel.color = Color.Lerp(startColor, endColor, timer / duration);
            yield return null;
        }

        fadePanel.color = endColor;
        congratulationsText.gameObject.SetActive(true);

        // Espera unos segundos y luego destruye el pastel
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

}
