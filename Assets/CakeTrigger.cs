using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CakeTrigger : MonoBehaviour
{
    public float fadeDuration = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            Transform finalTextGroup = GameObject.Find("FinalText")?.transform;
            if (finalTextGroup == null)
            {
                Debug.LogError("❌ No se encontró FinalText en la jerarquía.");
                return;
            }

            Image fadeImage = finalTextGroup.Find("FadePanel")?.GetComponent<Image>();
            Text congratsText = finalTextGroup.Find("CongratulationsText")?.GetComponent<Text>();
            Text subText = finalTextGroup.Find("SubText")?.GetComponent<Text>();
            Image cake = finalTextGroup.Find("Cake")?.GetComponent<Image>();

            if (fadeImage == null || congratsText == null)
            {
                Debug.LogError("❌ No se encontró FadePanel o CongratulationsText dentro de FinalText.");
                return;
            }

            // Iniciar la animación pero NO destruir todavía
            StartCoroutine(FadeAndShowText(fadeImage, congratsText, subText, cake));

        }
    }


    private IEnumerator FadeAndShowText(Image fadePanel, Text congratulationsText, Text SubText, Image cake)
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
        SubText.gameObject.SetActive(true);
        cake.gameObject.SetActive(true);

        // Espera unos segundos y luego destruye el pastel

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("IntroScene");
        Destroy(gameObject);
    }

}
