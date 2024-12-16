using UnityEngine;
using TMPro;

[System.Serializable]
public class BaseProporty
{
    public float buildTime; 
}
[System.Serializable]
public class ResourceProporty
{
    public float limitResource;
    public int increase;
}
[System.Serializable]
public class PriceProporty
{
    public float foot;
    public float wood;
    public float stone;
    public float iron;
}
public class Building : MonoBehaviour
{
    private BaseProporty CurrentBaseProporty => baseProporties[CurrentLevel]; 
    private PriceProporty CurrentPriceProporty => _priceProporty[CurrentLevel]; 
    private ResourceProporty CurrentResourceLevel => resourceLevelProporty[CurrentLevel]; 
    public enum TypeBuilding
    {
        ResourceTypeBuilding,
        Quarry,
        Sawmill,
        MilitaryBase,
        Barracks,
        TownHall 
    }
    public enum WhatResource
    {
        Farm,
        Quarry,
        Sawmill,
        IronMine,
        TownHall
    }

    public TypeBuilding typeBuilding;
    public WhatResource whatResource;

    public int buildIndex;
    private int CurrentLevel => gm.buildingLevels[buildIndex] - 1; 
    public int cost;

    public float timeBetweenIncreases;
    private float nextIncreaseTime;

    private GameManager gm;

    private float currentFood;
    private float currentWood;
    private float currentStone;
    private float currentIron;

    [SerializeField] private float _storedFood = 0f;
    [SerializeField] private float _storedStone = 0f;
    [SerializeField] private float _storedWood = 0f;
    [SerializeField] private float _storedIron = 0f;

    private float buildStartTime; // Время начала строительства
    public bool isBuilding = true; // Флаг строительства
    private bool canSpeedUp = false; // Возможность ускорения

    [SerializeField] private TMP_Text levelText;
    [SerializeField] private GameObject TakeResource;
    [SerializeField] private TMP_Text timerText; // Текст таймера
    [SerializeField] private GameObject freeButton; // Кнопка "Бесплатно"
    [Space(5)]
    [SerializeField] private BaseProporty[] baseProporties; 
    [SerializeField] private ResourceProporty[] resourceLevelProporty;
    [SerializeField] private PriceProporty[] _priceProporty;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();

        // Инициализация строительства
        buildStartTime = Time.time;
        freeButton.SetActive(false); // Кнопка скрыта изначально
        nextIncreaseTime = Time.time + timeBetweenIncreases;
    }
    private void Update()
    {
        if (isBuilding)
        {
            UpdateBuildTimer();
        }

        if (typeBuilding != TypeBuilding.TownHall)
            levelText.text = "Level: " + gm.buildingLevels[buildIndex].ToString();
        else
            levelText.text = $"Level: {gm.townHallLevel}";  

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
                float limitResource = resourceLevelProporty[CurrentLevel].limitResource; 
                if (_storedFood < limitResource)
                {
                    _storedFood += CurrentResourceLevel.increase;
                    if (_storedFood > limitResource)
                    {
                        _storedFood = limitResource; 
                    }

                    TakeResource.SetActive(true);
                }
            }
            if (whatResource == WhatResource.Quarry)
            {
                if (_storedStone < CurrentResourceLevel.limitResource)
                {
                    _storedStone += CurrentResourceLevel.increase;
                    if (_storedStone > CurrentResourceLevel.limitResource)
                    {
                        _storedStone = CurrentResourceLevel.limitResource; // ограничение лимитом
                    }

                    if (_storedStone == CurrentResourceLevel.limitResource)
                    {

                    }
                    TakeResource.SetActive(true);
                }
            }
            if (whatResource == WhatResource.Sawmill)
            {
                if (_storedWood < CurrentResourceLevel.limitResource)
                {
                    _storedWood += CurrentResourceLevel.increase;
                    if (_storedWood > CurrentResourceLevel.limitResource)
                    {
                        _storedWood = CurrentResourceLevel.limitResource; // Ограничение лимитом
                    }

                    if (_storedWood == CurrentResourceLevel.limitResource)
                    {

                    }
                    TakeResource.SetActive(true);
                }
            }
            if (whatResource == WhatResource.IronMine)
            {
                if (_storedIron < CurrentResourceLevel.limitResource)
                {
                    _storedIron += CurrentResourceLevel.increase;
                    if (_storedIron > CurrentResourceLevel.limitResource)
                    {
                        _storedIron = CurrentResourceLevel.limitResource; // Ограничение лимитом
                    }

                    if (_storedIron == CurrentResourceLevel.limitResource)
                    {
      
                    }
                    TakeResource.SetActive(true);
                }
            }
        }
    }
    private bool _firstUpgradeAdded; 
    private void UpdateBuildTimer()
    {
        float elapsedTime = Time.time - buildStartTime;
        float remainingTime = CurrentBaseProporty.buildTime - elapsedTime;

        if (remainingTime > 0)
        {
            timerText.text = $"{FormatTime(remainingTime)}";

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
        timerText.text = "";
        freeButton.SetActive(false);

        if (TypeBuilding.TownHall == typeBuilding)
            gm.townHallLevel++;
        else if (_firstUpgradeAdded)
            gm.buildingLevels[buildIndex] += 1;
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
        if (isBuilding) return; 

        int nextLevel = gm.buildingLevels[buildIndex] + 1; // Получаем следующий уровень здания

        if (typeBuilding != TypeBuilding.TownHall)
        {
            if (nextLevel == 1 && gm.townHallLevel < 1)
            {
                Debug.Log("TownHall level is too low for building level up");
                return;
            }
            if (nextLevel == 2 && gm.townHallLevel < 2)
            {
                Debug.Log("TownHall level is too low for building level up");
                return;
            }
            if (nextLevel == 3 && gm.townHallLevel < 3)
            {
                Debug.Log("TownHall level is too low for building level up");
                return;
            }
            if (nextLevel == 4 && gm.townHallLevel < 4)
            {
                Debug.Log("TownHall level is too low for building level up");
                return;
            }
            if (nextLevel == 5 && gm.townHallLevel < 5)
            {
                Debug.Log("TownHall level is too low for building level up");
                return;
            }
        }
        else if(gm.townHallLevel == 5)
        {
            return; 
        }

        bool canBuy = currentFood >= CurrentPriceProporty.foot 
            && currentWood >= CurrentPriceProporty.wood && currentStone >= CurrentPriceProporty.stone && currentIron >= CurrentPriceProporty.iron; 
        if (canBuy)
        {
            isBuilding = true;
            _firstUpgradeAdded = true;
            gm.food -= CurrentPriceProporty.foot;
            gm.wood -= CurrentPriceProporty.wood;
            gm.stone -= CurrentPriceProporty.stone;
            gm.iron -= CurrentPriceProporty.iron;
        }
        else
        {
            Debug.Log("Not enought resourses");
        }
    }
}