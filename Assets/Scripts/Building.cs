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
        Barracks
    }

    public enum WhatResource
    {
        Farm,
        Quarry,
        Sawmill,
    }

    public TypeBuilding typeBuilding;
    public WhatResource whatResource;

    public int cost;
    public int goldIncrease;
    public float timeBetweenIncreases;
    private float nextIncreaseTime;

    private GameManager gm;

    [SerializeField] private float FoodLvl1, WoodLvl1, StoneLvl1;
    [SerializeField] private float FoodLvl2, WoodLvl2, StoneLvl2;
    [SerializeField] private float FoodLvl3, WoodLvl3, StoneLvl3;
    [SerializeField] private float FoodLvl4, WoodLvl4, StoneLvl4;
    [SerializeField] private float FoodLvl5, WoodLvl5, StoneLvl5;

    [SerializeField] private GameObject TakeResource;

    private float currentFood;
    private float currentWood;
    private float currentStone;

    private float _currentLevel = 0;
    [SerializeField] private TMP_Text levelText;

    private float _storedFood = 0f;
    private float _storedStone = 0f;
    private float _storedWood = 0f;

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
      

        levelText.text = "Level: " + _currentLevel.ToString();

        currentFood = gm.food;
        currentWood = gm.wood;
        currentStone = gm.stone;

        if (!isBuilding &&  Time.time > nextIncreaseTime)
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
            Debug.Log("Collected food: " + gm.food);
        }
    }

    public void CollectResourcesWood()
    {
        // Передача накопленных ресурсов в GameManager
        if (whatResource == WhatResource.Sawmill)
        {
            gm.wood += _storedWood;
            _storedWood = 0f; // Сброс накопленных ресурсов
            Debug.Log("Collected food: " + gm.food);
        }
    }

    public void LevelUp()
    {
        if (_currentLevel == 0 && currentFood >= FoodLvl1 && currentWood >= WoodLvl1 && currentStone >= StoneLvl1)
        {
            goldIncrease = 4000;
            timeBetweenIncreases = 1f;
            _limitResource = 20000f;
            _currentLevel++;
            Debug.Log("1 Level");

            gm.food -= FoodLvl1;
            gm.wood -= WoodLvl1;
            gm.stone -= StoneLvl1;

            buildTime = 140;
            UpdateBuildTimer();
        }
        else if (_currentLevel == 1 && currentFood >= FoodLvl2 && currentWood >= WoodLvl2 && currentStone >= StoneLvl2)
        {
            goldIncrease = 510;
            timeBetweenIncreases = 1f;
            _limitResource = 25500f;
            _currentLevel++;
            Debug.Log("2 Level");

            gm.food -= FoodLvl1;
            gm.wood -= WoodLvl1;
            gm.stone -= StoneLvl1;

            buildTime = 320;
        }
        else if (_currentLevel == 2 && currentFood >= FoodLvl3 && currentWood >= WoodLvl3 && currentStone >= StoneLvl3)
        {
            goldIncrease = 630;
            timeBetweenIncreases = 1f;
            _limitResource = 31500f;
            _currentLevel++;
            Debug.Log("3 Level");

            gm.food -= FoodLvl1;
            gm.wood -= WoodLvl1;
            gm.stone -= StoneLvl1;

            buildTime = 560;
        }
        else if (_currentLevel == 3 && currentFood >= FoodLvl4 && currentWood >= WoodLvl4 && currentStone >= StoneLvl4)
        {
            goldIncrease = 760;
            timeBetweenIncreases = 1f;
            _limitResource = 38000f;
            _currentLevel++;
            Debug.Log("4 Level");

            gm.food -= FoodLvl1;
            gm.wood -= WoodLvl1;
            gm.stone -= StoneLvl1;

            buildTime = 1120;
        }
        else if (_currentLevel == 4 && currentFood >= FoodLvl5 && currentWood >= WoodLvl5 && currentStone >= StoneLvl5)
        {
            goldIncrease = 900;
            timeBetweenIncreases = 1f;
            _limitResource = 45000f;
            _currentLevel++;
            Debug.Log("5 Level");

            gm.food -= FoodLvl1;
            gm.wood -= WoodLvl1;
            gm.stone -= StoneLvl1;
        }
    }  
}