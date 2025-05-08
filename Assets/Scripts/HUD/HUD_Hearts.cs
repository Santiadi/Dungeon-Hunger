using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Hearts : MonoBehaviour
{
    public GameObject heartPrefab; // Prefab con un Image dentro
    public Sprite fullHeartSprite;
    public Sprite halfHeartSprite;
    public Sprite emptyHeartSprite;
    public Sprite ghostFullHeartSprite;
    public Sprite ghostHalfHeartSprite;


    private List<GameObject> heartObjects = new List<GameObject>();

    public void UpdateHearts(float currentHealth, float maxHearts, float ghostHearts = 0f)
    {
        int totalHearts = Mathf.CeilToInt(maxHearts + ghostHearts);
        int ghostStartIndex = Mathf.CeilToInt(maxHearts);

        // Asegura suficientes objetos
        while (heartObjects.Count < totalHearts)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            heartObjects.Add(heart);
        }

        // --- Corazones normales ---
        for (int i = 0; i < Mathf.CeilToInt(maxHearts); i++)
        {
            Image heartImage = heartObjects[i].GetComponent<Image>();

            if (i < Mathf.FloorToInt(currentHealth))
                heartImage.sprite = fullHeartSprite;
            else if (i < currentHealth)
                heartImage.sprite = halfHeartSprite;
            else
                heartImage.sprite = emptyHeartSprite;

            heartObjects[i].SetActive(true);
        }

        // --- Ghost Hearts ---
        for (int i = 0; i < Mathf.CeilToInt(ghostHearts); i++)
        {
            Image heartImage = heartObjects[ghostStartIndex + i].GetComponent<Image>();

            if (i < Mathf.FloorToInt(ghostHearts))
                heartImage.sprite = ghostFullHeartSprite;
            else
                heartImage.sprite = ghostHalfHeartSprite;

            heartImage.gameObject.SetActive(true);
        }

        // --- Ocultar los que sobran ---
        for (int i = totalHearts; i < heartObjects.Count; i++)
        {
            heartObjects[i].SetActive(false);
        }
    }

}
