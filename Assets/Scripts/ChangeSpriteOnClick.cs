using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpriteOnClick : MonoBehaviour
{
    public Sprite[] sprites; // Массив спрайтов для изменения
    private SpriteRenderer spriteRenderer;
    private int currentSpriteIndex = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[0]; // Устанавливаем первый спрайт из массива
        }
    }

    void OnMouseDown()
    {
        if (sprites.Length == 0)
        {
            Debug.LogWarning("Массив спрайтов пуст!");
            return;
        }

        currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
        spriteRenderer.sprite = sprites[currentSpriteIndex];
    }
}