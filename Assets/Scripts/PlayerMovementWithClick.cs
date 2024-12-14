using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementWithClick : MonoBehaviour
{
    public float Speed = 10f;

    public GameObject cross;

    Vector2 pos;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(cross, pos, Quaternion.identity);
        }

        transform.position = Vector2.MoveTowards(transform.position, pos, Speed * Time.deltaTime);
    }
}
