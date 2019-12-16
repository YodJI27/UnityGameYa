using UnityEngine;

public class Attributes : MonoBehaviour
{
    [Header("Start Attribute")]
    public float s_Health;
    public float s_Damage;

    //Состояние смерти
    public bool Death { get; private set; }
    public float maxHealth { get; private set; }
    public float curHealth { get; private set; }
    public float Damage { get; private set; }

    private Unit p_Unit;
    private void Start()
    {
        //Устанавливаем стартовые аттрибуты
        maxHealth = s_Health;
        curHealth = maxHealth;
        Damage = s_Damage;

        p_Unit = GetComponent<Unit>();
    }

    private void Update()
    {

    }

    //Метод принимает урон, и возвращает статус смерти
    public bool ApplayDamage(float amount)
    {
        if((curHealth - amount) <= 0.0f)
        {
            curHealth = 0.0f;
            p_Unit.m_ProgressBar.fillAmount = curHealth / maxHealth;
            Death = true;
            return false;
        }
        else
        {
            curHealth -= amount;
            p_Unit.m_ProgressBar.fillAmount = curHealth / maxHealth;
            return true;
        }
    }
}
