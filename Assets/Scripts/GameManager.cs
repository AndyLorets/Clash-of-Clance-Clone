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
    public float iron; // Добавлено железо

    public Text goldDisplay;
    public Text foodDisplay;
    public Text woodDisplay;
    public Text stoneDisplay;
    public Text ironDisplay; // Добавлено для железа

    private Building buildingToPlace;

    public CustomCursor customCursor;
    public Tile[] tiles;

    // Уровень ратуши
    public int townHallLevel = 1;
    public int maxMarchQueues = 1; // Максимальное количество марш очередей (стартовое значение)

    // Для отслеживания улучшений зданий, в будущем можно добавить зависимость улучшений от уровней ратуши
    public Dictionary<Building.TypeBuilding, int> buildingLevels = new Dictionary<Building.TypeBuilding, int>();
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); 
    }
    private void Start()
    {
        // Инициализация уровней зданий
        buildingLevels[Building.TypeBuilding.ResourceTypeBuilding] = 0;
        buildingLevels[Building.TypeBuilding.MilitaryBase] = 0;

    }

    private void Update()
    {
        goldDisplay.text = "Gold: " + gold.ToString();
        woodDisplay.text = "Wood: " + wood.ToString();
        foodDisplay.text = "Food: " + food.ToString();
        stoneDisplay.text = "Stone: " + stone.ToString();
        ironDisplay.text = "Iron: " + iron.ToString(); // Обновление для железа

        // Обработка клика для размещения здания
        if (Input.GetMouseButtonDown(0) && buildingToPlace != null)
        {
            PlaceBuilding();
        }
        if(Input.GetMouseButtonDown(1))
        {
            UpgradeTownHall();
        }
    }


    private void PlaceBuilding()
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

        if (nearestTile != null && !nearestTile.isOccupied)
        {
            if (CanPlaceBuildingOnTile(nearestTile))
            {
                Instantiate(buildingToPlace, nearestTile.transform.position, Quaternion.identity);
                buildingToPlace = null;
                nearestTile.isOccupied = true;

                customCursor.gameObject.SetActive(false);
                Cursor.visible = true;
            }

        }
    }

    private bool CanPlaceBuildingOnTile(Tile tile)
    {

        if (buildingToPlace.typeBuilding == Building.TypeBuilding.MilitaryBase && tile.tileType == Tile.TileType.Military)
        {
            return true;
        }
        else if (buildingToPlace.typeBuilding == Building.TypeBuilding.ResourceTypeBuilding && tile.tileType == Tile.TileType.Resource)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BuyBuilding(Building building)
    {
        if (CanBuyBuilding(building))
        {
            customCursor.gameObject.SetActive(true);
            customCursor.GetComponent<SpriteRenderer>().sprite = building.GetComponent<SpriteRenderer>().sprite;
            Cursor.visible = false;

            buildingToPlace = building;

            // Вычитаем ресурсы, после успешной установки здания

        }

    }
    private bool CanBuyBuilding(Building building)
    {
        if (gold < building.cost)
        {
            Debug.Log("Not enough gold");
            return false;
        }
        if (building.typeBuilding != Building.TypeBuilding.TownHall)
        {
            if (buildingLevels.ContainsKey(building.typeBuilding) && buildingLevels[building.typeBuilding] >= townHallLevel)
            {
                Debug.Log("TownHall level is too low");
                return false;
            }
        }


        if (building.whatResource == Building.WhatResource.Farm && townHallLevel < 1)
        {
            Debug.Log("TownHall need to be 1 level");
            return false;
        }
        if (building.whatResource == Building.WhatResource.Sawmill && townHallLevel < 1)
        {
            Debug.Log("TownHall need to be 1 level");
            return false;
        }
        if (building.whatResource == Building.WhatResource.Quarry && townHallLevel < 3)
        {
            Debug.Log("TownHall need to be 3 level");
            return false;
        }

        if (building.whatResource == Building.WhatResource.IronMine && townHallLevel < 5)
        {
            Debug.Log("TownHall need to be 5 level");
            return false;
        }

        gold -= building.cost;

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

        return true;
    }

    public void UpgradeTownHall()
    {
        int nextLevel = townHallLevel + 1;

        if (nextLevel == 2 && food >= 1400 && wood >= 1400)
        {

            food -= 1400;
            wood -= 1400;
            townHallLevel = nextLevel;
            maxMarchQueues = 1;

            Debug.Log("Town Hall upgraded to level " + townHallLevel);
        }
        else if (nextLevel == 3 && food >= 2800 && wood >= 2800)
        {

            food -= 2800;
            wood -= 2800;
            townHallLevel = nextLevel;
            maxMarchQueues = 1;
            Debug.Log("Town Hall upgraded to level " + townHallLevel);
        }
        else if (nextLevel == 4 && food >= 5600 && wood >= 5600 && stone >= 5600)
        {
            food -= 5600;
            wood -= 5600;
            stone -= 5600;
            townHallLevel = nextLevel;
            maxMarchQueues = 1;
            Debug.Log("Town Hall upgraded to level " + townHallLevel);
        }
        else if (nextLevel == 5 && food >= 10200 && wood >= 10200 && stone >= 10200)
        {
            food -= 10200;
            wood -= 10200;
            stone -= 10200;
            townHallLevel = nextLevel;
            maxMarchQueues = 2;
            Debug.Log("Town Hall upgraded to level " + townHallLevel);
        }

        else
        {
            Debug.Log("Not enought resourses");
        }
    }
}