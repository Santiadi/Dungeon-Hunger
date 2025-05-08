using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Hearts : MonoBehaviour
{
    public GameObject heartPrefab; // Prefab con un Image dentro
    public Sprite fullHeartSprite;
    public Sprite halfHeartSprite;
    public Sprite emptyHeartSprite;

    private List<GameObject> heartObjects = new List<GameObject>();

    public void UpdateHearts(float currentHealth, float maxHearts)
    {
        // Aseg�rate de que haya suficientes objetos de coraz�n
        while (heartObjects.Count < maxHearts)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            heartObjects.Add(heart);
        }

        // Actualiza cada coraz�n
        for (int i = 0; i < heartObjects.Count; i++)
        {
            Image heartImage = heartObjects[i].GetComponent<Image>();

            if (i < Mathf.FloorToInt(currentHealth))
            {
                heartImage.sprite = fullHeartSprite;
                heartObjects[i].SetActive(true);
            }
            else if (i < currentHealth)
            {
                heartImage.sprite = halfHeartSprite;
                heartObjects[i].SetActive(true);
            }
            else if (i < maxHearts)
            {
                heartImage.sprite = emptyHeartSprite;
                heartObjects[i].SetActive(true);
            }
            else
            {
                // Oculta corazones extra
                heartObjects[i].SetActive(false);
            }
        }
    }
}
