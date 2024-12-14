using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    private LineRenderer lineRend;
    private Vector3 mousePos;
    public Transform startMousePos; // Объект, от которого начинается линия
    public float disappearSpeed = 5f; // Скорость исчезновения линии

    private Vector3 currentStartPos;
    private bool isDrawing = false;

    private void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = 2;

        // Начальная позиция линии равна позиции объекта
        if (startMousePos != null)
        {
            currentStartPos = startMousePos.position;
            lineRend.SetPosition(0, new Vector3(currentStartPos.x, currentStartPos.y, 0f));
        }
    }

    private void Update()
    {
        // Начинаем рисовать линию при первом клике мыши
        if (Input.GetMouseButtonDown(0) && startMousePos != null)
        {
            // Фиксируем начальную и конечную позиции
            currentStartPos = startMousePos.position;
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            lineRend.SetPosition(0, new Vector3(currentStartPos.x, currentStartPos.y, 0f));
            lineRend.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 0f));

            // Включаем флаг для уменьшения линии
            isDrawing = true;
        }

        // Если линия начата, уменьшаем её длину
        if (isDrawing)
        {
            // Плавно перемещаем начальную точку к конечной
            currentStartPos = Vector3.MoveTowards(currentStartPos, mousePos, disappearSpeed * Time.deltaTime);
            lineRend.SetPosition(0, new Vector3(currentStartPos.x, currentStartPos.y, 0f));

            // Если начальная точка достигла конечной, останавливаем уменьшение
            if (currentStartPos == mousePos)
            {
                isDrawing = false;
            }
        }
    }
}
