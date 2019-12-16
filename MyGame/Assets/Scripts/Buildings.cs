using UnityEngine;

public abstract class Buildings : MonoBehaviour, IActionWithObject
{
    [Header("Description")]
    public string Name;
    public string Description;

    [Header("Start Attribute")]
    public float s_MaxHealth;
    public float s_CurHealth;

    public ETypesUnits Type;

    public float maxHealth { get; private set; }
    public float curHealth { get; private set; }

    protected virtual void Start()
    {
        maxHealth = s_MaxHealth;
        curHealth = s_CurHealth <= 0 ? maxHealth: s_CurHealth;
    }

    protected virtual void Update()
    {

    }


    //Реализация интерфейса IActionWithBuilding

    public virtual void Select()
    {//Метод вызывается при выборе объекта
        GameHUD.Instance.m_ObjectPanel.SetActive(true);
        //Устанавливаем значения для п. интерфейса
        GameHUD.Instance.m_TextName.text = Name;
        GameHUD.Instance.m_TextHealth.text = curHealth + " / " + maxHealth;
        GameHUD.Instance.m_TextDescription.text = Description;
    }

    public virtual void UpdateUI()
    {//Метод вызывается при обновлении пользовательского интерфейса
        GameHUD.Instance.m_TextHealth.text = curHealth + " / " + maxHealth;
    }

    public virtual void Diselect()
    {//Метод вызывается при снятии выделения с объекта
        GameHUD.Instance.m_ObjectPanel.SetActive(false);
    }
}
