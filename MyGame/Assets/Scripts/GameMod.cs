using System.Collections.Generic;
using UnityEngine;

public class GameMod : MonoBehaviour
{
    //Паттерн синглтон
    public static GameMod Instance { get; private set; }

    [Header("Start Resources")]
    public int s_Gold;
    public int s_Food;
    public int s_Stone;
    public int s_Tree;

    [HideInInspector]
    public int m_Gold;
    [HideInInspector]
    public int m_Food;
    [HideInInspector]
    public int m_Stone;
    [HideInInspector]
    public int m_Tree;

    [Header("Player Units")]
    public List<Unit> PlayerUnits = new List<Unit>();   //Лист юнитов игрока
    public List<Unit> HostileUnits = new List<Unit>();  //Лист врагов
    public unitSquad SelectSquad = new unitSquad();     //Отряд юнитов игрока

    [Header("Reference")]
    public GameObject SelectedObject;
    public IActionWithObject ActionWithSelectedObject;
    public List<Buinding> Shop = new List<Buinding>();
    public List<Area> Areas = new List<Area>();

    public Camera m_Camera { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }else
        {
            Instance = this;
        }

        //Устанавливаем стартовые ресурсы
        m_Gold = s_Gold;
        m_Food = s_Food;
        m_Stone = s_Stone;
        m_Tree = s_Tree;
    }

    private void Start()
    {
        m_Camera = Camera.main;
    }

    //Вызывается при нажатии кнопки покупки в магазине юнитов
    public void BuyUnit(int index)
    {
        //Извлекаем данные по юниту для оптимальной работы
        UnitData _unitData = SelectedObject.GetComponent<Barracks>().m_ShopUnit[index];

        //Проверяем условия покупки
        if(_unitData.AmoundGold <= m_Gold && _unitData.AmountFood <= m_Food)
        {   //Если условия соблюдены то нанимаем юнита
            SelectedObject.GetComponent<Barracks>().HiringUnit(index);
            //Списываем ресурсы
            m_Gold -= _unitData.AmoundGold;
            m_Food -= _unitData.AmountFood;
            //Обновляем статистику ресурсов на экране
            GameHUD.Instance.UpdateResourcesPanel();
        }
    }

}