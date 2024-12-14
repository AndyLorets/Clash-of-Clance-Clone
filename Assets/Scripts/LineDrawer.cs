using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    private LineRenderer lineRend;
    private Vector3 mousePos;
    public Transform startMousePos; // ������, �� �������� ���������� �����
    public float disappearSpeed = 5f; // �������� ������������ �����

    private Vector3 currentStartPos;
    private bool isDrawing = false;

    private void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = 2;

        // ��������� ������� ����� ����� ������� �������
        if (startMousePos != null)
        {
            currentStartPos = startMousePos.position;
            lineRend.SetPosition(0, new Vector3(currentStartPos.x, currentStartPos.y, 0f));
        }
    }

    private void Update()
    {
        // �������� �������� ����� ��� ������ ����� ����
        if (Input.GetMouseButtonDown(0) && startMousePos != null)
        {
            // ��������� ��������� � �������� �������
            currentStartPos = startMousePos.position;
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            lineRend.SetPosition(0, new Vector3(currentStartPos.x, currentStartPos.y, 0f));
            lineRend.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 0f));

            // �������� ���� ��� ���������� �����
            isDrawing = true;
        }

        // ���� ����� ������, ��������� � �����
        if (isDrawing)
        {
            // ������ ���������� ��������� ����� � ��������
            currentStartPos = Vector3.MoveTowards(currentStartPos, mousePos, disappearSpeed * Time.deltaTime);
            lineRend.SetPosition(0, new Vector3(currentStartPos.x, currentStartPos.y, 0f));

            // ���� ��������� ����� �������� ��������, ������������� ����������
            if (currentStartPos == mousePos)
            {
                isDrawing = false;
            }
        }
    }
}
