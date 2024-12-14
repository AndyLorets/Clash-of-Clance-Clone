using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public enum TileType
    {
        Military,
        Resource
    }

    public TileType tileType; // ��� ������: ������� ��� ���������
    public bool isOccupied;
    public Color greenColor;
    public Color redColor;

    private SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isOccupied)
        {
            rend.color = redColor;
            rend.enabled = false; // ������ �� ����� �����, ���� ������
        }
        else
        {
            rend.color = greenColor;
        }
    }
}
