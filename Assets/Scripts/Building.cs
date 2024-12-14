using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Building : MonoBehaviour
{
    public enum TypeBuilding
    {
        ResourceTypeBuilding,
        Quarry,
        Sawmill,
        MilitaryBase,
        Barracks,
        TownHall //Добавили ратушу
    }

    public enum WhatResource
    {
        Farm,
        Quarry,
        Sawmill,
        IronMine //добавили железный рудник
    }

    public TypeBuilding typeBuilding;
    public WhatResource whatResource;

    public int cost;
    public int goldIncrease;
    public float timeBetweenIncreases;
    private float nextIncreaseTime;

    private GameManager gm;

    [SerializeField] private float FoodLvl1, WoodLvl1, StoneLvl1, IronLvl1;
    [SerializeField] private float FoodLvl2, WoodLvl2, StoneLvl2, IronLvl2;
    [SerializeField] private float FoodLvl3, WoodLvl3, StoneLvl3, IronLvl3;
    [SerializeField] private float FoodLvl4, WoodLvl4, StoneLvl4, IronLvl4;
    [SerializeField] private float FoodLvl5, WoodLvl5, StoneLvl5, IronLvl5;

    [SerializeField] private GameObject TakeResource;

    private float currentFood;
    private float currentWood;
    private float currentStone;
    private float currentIron;

    [SerializeField] private TMP_Text levelText;

    private float _storedFood = 0f;
    private float _storedStone = 0f;
    private float _storedWood = 0f;
    private float _storedIron = 0f;

    private float _limitResource;

    // Новые переменные для таймера строительства
    public float buildTime; // Общее время строительства (в секундах)
    private float buildStartTime; // Время начала строительства
    private bool isBuilding = true; // Флаг строительства
    private bool canSpeedUp = false; // Возможность ускорения

    [SerializeField] private TMP_Text timerText; // Текст таймера
    [SerializeField] private GameObject freeButton; // Кнопка "Бесплатно"

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        TakeResource.SetActive(false);

        // Инициализация строительства
        buildStartTime = Time.time;
        isBuilding = true;
        freeButton.SetActive(false); // Кнопка скрыта изначально
    }

    private void Update()
    {
        if (isBuilding)
        {
            UpdateBuildTimer();
        }


        levelText.text = "Level: " + gm.buildingLevels[typeBuilding].ToString();

        currentFood = gm.food;
        currentWood = gm.wood;
        currentStone = gm.stone;
        currentIron = gm.iron;

        if (!isBuilding && Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeBetweenIncreases;

            // Накопление ресурсов
            if (whatResource == WhatResource.Farm)
            {
                if (_storedFood < _limitResource)
                {
                    _storedFood += goldIncrease;
                    if (_storedFood > _limitResource)
                    {
                        _storedFood = _limitResource; // Ограничение лимитом
                    }

                    if (_storedFood == _limitResource)
                    {
                        TakeResource.SetActive(true);
                    }
                }
            }

            if (whatResource == WhatResource.Quarry)
            {
                if (_storedStone < _limitResource)
                {
                    _storedStone += goldIncrease;
                    if (_storedStone > _limitResource)
                    {
                        _storedStone = _limitResource; // ограничение лимитом
                    }

                    if (_storedStone == _limitResource)
                    {
                        TakeResource.SetActive(true);
                    }
                }
            }

            if (whatResource == WhatResource.Sawmill)
            {
                if (_storedWood < _limitResource)
                {
                    _storedWood += goldIncrease;
                    if (_storedWood > _limitResource)
                    {
                        _storedWood = _limitResource; // Ограничение лимитом
                    }

                    if (_storedWood == _limitResource)
                    {
                        TakeResource.SetActive(true);
                    }
                }
            }
            if (whatResource == WhatResource.IronMine)
            {
                if (_storedIron < _limitResource)
                {
                    _storedIron += goldIncrease;
                    if (_storedIron > _limitResource)
                    {
                        _storedIron = _limitResource; // Ограничение лимитом
                    }

                    if (_storedIron == _limitResource)
                    {
                        TakeResource.SetActive(true);
                    }
                }
            }
        }
    }

    private void UpdateBuildTimer()
    {
        float elapsedTime = Time.time - buildStartTime;
        float remainingTime = buildTime - elapsedTime;

        if (remainingTime > 0)
        {
            // Обновляем текст таймера
            timerText.text = $"Time Left: {FormatTime(remainingTime)}";

            // Если оставшееся время ≤ 5 минут (300 секунд), показываем кнопку "Бесплатно"
            if (remainingTime <= 300f && !freeButton.activeSelf)
            {
                canSpeedUp = true;
                freeButton.SetActive(true);
            }
        }
        else
        {
            FinishBuilding();
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:D2}:{seconds:D2}";
    }

    private void FinishBuilding()
    {
        isBuilding = false;
        timerText.text = "Building Complete!";
        freeButton.SetActive(false);
    }

    public void SpeedUpBuilding()
    {
        if (canSpeedUp)
        {
            FinishBuilding();
        }
    }

    public void CollectResourcesFood()
    {
        // Передача накопленных ресурсов в GameManager
        if (whatResource == WhatResource.Farm)
        {
            gm.food += _storedFood;
            _storedFood = 0f; // Сброс накопленных ресурсов
            Debug.Log("Collected food: " + gm.food);
        }
    }

    public void CollectResourcesStone()
    {
        // Передача накопленных ресурсов в GameManager
        if (whatResource == WhatResource.Quarry)
        {
            gm.stone += _storedStone;
            _storedStone = 0f; // Сброс накопленных ресурсов
            Debug.Log("Collected stone: " + gm.stone);
        }
    }

    public void CollectResourcesWood()
    {
        // Передача накопленных ресурсов в GameManager
        if (whatResource == WhatResource.Sawmill)
        {
            gm.wood += _storedWood;
            _storedWood = 0f; // Сброс накопленных ресурсов
            Debug.Log("Collected wood: " + gm.wood);
        }
    }
    public void CollectResourcesIron()
    {
        // Передача накопленных ресурсов в GameManager
        if (whatResource == WhatResource.IronMine)
        {
            gm.iron += _storedIron;
            _storedIron = 0f; // Сброс накопленных ресурсов
            Debug.Log("Collected iron: " + gm.iron);
        }
    }

    public void LevelUp()
    {
        if (gm.buildingLevels[typeBuilding] == 0 && currentFood >= FoodLvl1 && currentWood >= WoodLvl1 && currentStone >= StoneLvl1 && currentIron >= IronLvl1)
        {
            goldIncrease = 4000;
            timeBetweenIncreases = 1f;
            _limitResource = 20000f;
            gm.buildingLevels[typeBuilding] = 1; // Обновление уровня в GameManager
            Debug.Log("1 Level");

            gm.food -= FoodLvl1;
            gm.wood -= WoodLvl1;
            gm.stone -= StoneLvl1;
            gm.iron -= IronLvl1;
            buildTime = 140;
            UpdateBuildTimer();
        }
        else if (gm.buildingLevels[typeBuilding] == 1 && currentFood >= FoodLvl2 && currentWood >= WoodLvl2 && currentStone >= StoneLvl2 && currentIron >= IronLvl2)
        {
            goldIncrease = 510;
            timeBetweenIncreases = 1f;
            _limitResource = 25500f;
            gm.buildingLevels[typeBuilding] = 2;
            Debug.Log("2 Level");

            gm.food -= FoodLvl2;
            gm.wood -= WoodLvl2;
            gm.stone -= StoneLvl2;
            gm.iron -= IronLvl2;
            buildTime = 320;
            UpdateBuildTimer();
        }
        else if (gm.buildingLevels[typeBuilding] == 2 && currentFood >= FoodLvl3 && currentWood >= WoodLvl3 && currentStone >= StoneLvl3 && currentIron >= IronLvl3)
        {
            goldIncrease = 630;
            timeBetweenIncreases = 1f;
            _limitResource = 31500f;
            gm.buildingLevels[typeBuilding] = 3;
            Debug.Log("3 Level");

            gm.food -= FoodLvl3;
            gm.wood -= WoodLvl3;
            gm.stone -= StoneLvl3;
            gm.iron -= IronLvl3;
            buildTime = 560;
            UpdateBuildTimer();
        }
        else if (gm.buildingLevels[typeBuilding] == 3 && currentFood >= FoodLvl4 && currentWood >= WoodLvl4 && currentStone >= StoneLvl4 && currentIron >= IronLvl4)
        {
            goldIncrease = 760;
            timeBetweenIncreases = 1f;
            _limitResource = 38000f;
            gm.buildingLevels[typeBuilding] = 4;
            Debug.Log("4 Level");

            gm.food -= FoodLvl4;
            gm.wood -= WoodLvl4;
            gm.stone -= StoneLvl4;
            gm.iron -= IronLvl4;
            buildTime = 1120;
            UpdateBuildTimer();
        }
        else if (gm.buildingLevels[typeBuilding] == 4 && currentFood >= FoodLvl5 && currentWood >= WoodLvl5 && currentStone >= StoneLvl5 && currentIron >= IronLvl5)
        {
            goldIncrease = 900;
            timeBetweenIncreases = 1f;
            _limitResource = 45000f;
            gm.buildingLevels[typeBuilding] = 5;
            Debug.Log("5 Level");

            gm.food -= FoodLvl5;
            gm.wood -= WoodLvl5;
            gm.stone -= StoneLvl5;
            gm.iron -= IronLvl5;
            UpdateBuildTimer();
        }
        else
        {
            Debug.Log("Not enought resourses");
        }
    }
}