using System.Collections.Generic;
using UnityEngine;

public class SequentialActivator : MonoBehaviour
{
    public Transform Walls;
    private int currentIndex = 1;

    private bool _stop = true;

    public GameObject[] PlaceToActivate;
    private int currentIndexToPlace = -1;

    private float scaleIncrement = 0.4f; // Значение увеличения

    public void ActivateNextObject()
    {    
        if (currentIndex > 0 && _stop)
        {
            Walls.localScale += new Vector3(scaleIncrement, scaleIncrement, 0f);
            currentIndex++;
        }              

        if(currentIndex == 5)
        {
            _stop = false;
        }
    }

    public void ActiveNextPlace()
    {     
        // Увеличиваем индекс
        currentIndexToPlace = (currentIndexToPlace + 1) % PlaceToActivate.Length;

        // Активируем следующий объект
        PlaceToActivate[currentIndexToPlace].SetActive(true);
    }
}