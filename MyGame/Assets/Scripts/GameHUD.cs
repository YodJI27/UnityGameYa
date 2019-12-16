using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    //Применяем шаблон синглтон
    public static GameHUD Instance { get; private set; }

    [Header("References Attribute UI")]
    public Text m_TextResources;

    [Header("Reference Select Panel")]
    public GameObject m_ObjectPanel;
    public Text m_TextName;
    public Text m_TextHealth;
    public Text m_TextDescription;

    [Header("References Shop")]
    public GameObject m_ShopPanel;
    public GameObject m_UnitShopPanel;

    [Header("References HiringUnitPanel")]
    public GameObject HiringUnitPanel;
    public Image ProgressBar;
    public List<Image> QueueHiringUnitAvatar = new List<Image>();

    private void Awake()
    {
        //Только один экземпляр класса может быть, удаляем себя если уже есть иначе устанавливаем
        if(Instance != null)
        {
            Destroy(this);
        }else
        {
            Instance = this;
        }
    }
    void Start()
    {
        UpdateResourcesPanel();
    }

    void Update()
    {
        //Если появляется выделенный объект, то вызываем у него метод для обновления статистики интерфейса
        if(GameMod.Instance.ActionWithSelectedObject != null)
        {
            GameMod.Instance.ActionWithSelectedObject.UpdateUI();
        }
    }

    //Метод обновляющий статистику на экране
    public void UpdateResourcesPanel()
    {
        m_TextResources.text = "Золото: " + GameMod.Instance.m_Gold + 
            "    Еда: " + GameMod.Instance.m_Food +
            "    Дерево:" + GameMod.Instance.m_Tree + 
            "   Камень:" + GameMod.Instance.m_Stone;
    }

}
