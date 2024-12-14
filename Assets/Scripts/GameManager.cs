using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int gold;

    public float wood;
    public float food;
    public float stone;

    public Text goldDisplay;
    public Text foodDisplay;
    public Text woodDisplay;
    public Text stoneDisplay;

    private Building buildingToPlace;
    public GameObject grid;

    public CustomCursor customCursor;

    public Tile[] tiles;

    private void Update()
    {
        goldDisplay.text = "Gold: " + gold.ToString();
        woodDisplay.text = "Wood: " + wood.ToString();
        foodDisplay.text = "Food: " + food.ToString();
        stoneDisplay.text = "Stone: " + stone.ToString();

        // Обработка клика для размещения здания
        if (Input.GetMouseButtonDown(0) && buildingToPlace != null)
        {
            Tile nearestTile = null;
            float nearestDistance = float.MaxValue;
   
            // Поиск ближайшей плитки
            foreach (Tile tile in tiles)
            {
                float dist = Vector2.Distance(tile.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

                if (dist < nearestDistance)
                {
                    nearestDistance = dist;
                    nearestTile = tile;
                }
            }

            // Проверка, можно ли строить на этой плитке
            if (nearestTile != null && !nearestTile.isOccupied)
            {            
               // if (CanPlaceBuildingOnTile(nearestTile))
               if(nearestTile.tileType == Tile.TileType.Military && buildingToPlace.typeBuilding == Building.TypeBuilding.MilitaryBase)
                {
                    Instantiate(buildingToPlace, nearestTile.transform.position, Quaternion.identity);
                    buildingToPlace = null;
                    nearestTile.isOccupied = true;
                   

                    customCursor.gameObject.SetActive(false);
                    Cursor.visible = true;
                }
                else if(nearestTile.tileType == Tile.TileType.Resource && buildingToPlace.typeBuilding == Building.TypeBuilding.ResourceTypeBuilding)
                {
                    Instantiate(buildingToPlace, nearestTile.transform.position, Quaternion.identity);
                    buildingToPlace = null;
                    nearestTile.isOccupied = true;
                   
                    customCursor.gameObject.SetActive(false);
                    Cursor.visible = true;
                }

              
            }
        }
    }

    public void BuyBuilding(Building building)
    {
        if (gold >= building.cost)
        {
            customCursor.gameObject.SetActive(true);
            customCursor.GetComponent<SpriteRenderer>().sprite = building.GetComponent<SpriteRenderer>().sprite;
            Cursor.visible = false;

            gold -= building.cost;
            buildingToPlace = building;
            grid.SetActive(true);
         

            if (building.whatResource == Building.WhatResource.Farm)
            {
                wood -= 200;
               
            }

            if (building.whatResource == Building.WhatResource.Quarry)
            {
                wood -= 100;
                food -= 100;
               
            }

            if (building.whatResource == Building.WhatResource.Sawmill)
            {
                food -= 200;
              
            }
        }
    }
}