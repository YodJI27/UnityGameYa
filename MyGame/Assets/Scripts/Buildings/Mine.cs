using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Mine : Buildings
{
    [Header("Start Attribute")]
    public float s_MaxGold;         //Максимальное кол-во золота в руднике
    public float s_StartGold;       //Начальное кол-во золота в руднике, если 0 то будет установлено как макс. значение
    public int s_GoldPerSecond;     //Золота в секунду

    public float m_MaxGold { get; private set; }
    public float m_CurGold { get; private set; }
    private int p_GoldPerSecond;

    private bool p_HeroStay;
    private float p_Timer;

    protected override void Start()
    {
        base.Start();

        p_Timer = 1.0f;
        m_MaxGold = s_MaxGold;
        m_CurGold = s_StartGold != 0 ? s_StartGold : m_MaxGold;
        p_GoldPerSecond = s_GoldPerSecond;

        Description = "Рудник приносит " + p_GoldPerSecond + "ед. золота, каждую секунду";
    }

    protected override void Update()
    {
        base.Update();

        if(p_HeroStay)
        {
            p_Timer -= Time.deltaTime;
            if(p_Timer <= 0.0f)
            {
                p_Timer = 1.0f;
                GameMod.Instance.m_Gold += p_GoldPerSecond;
                m_CurGold -= p_GoldPerSecond;
                GameHUD.Instance.UpdateResourcesPanel();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll) //Когда юнит вошел в рудник
    {
        if(coll.gameObject.CompareTag("Unit"))
            p_HeroStay = true; //Включаем добычу золота

    }

    void OnCollisionExit2D(Collision2D coll) //Когда юнит вышел с рудника
    {
        if(coll.gameObject.CompareTag("Unit"))
            p_HeroStay = false; //Отключаем добычу золота

        p_Timer = 1.0f; //Обнуляем таймер добычи
    }

    public override void Select()
    {
        GameHUD.Instance.m_ObjectPanel.SetActive(true);
        GameHUD.Instance.m_TextName.text = Name;
        GameHUD.Instance.m_TextHealth.text = "<color=#CFFA2E>Золото: " + m_CurGold + " / " + m_MaxGold + "</color>";
        GameHUD.Instance.m_TextDescription.text = Description;
    }

    public override void UpdateUI()
    {
        GameHUD.Instance.m_TextHealth.text = "<color=#CFFA2E>Золото: " + m_CurGold + " / " + m_MaxGold + "</color>";
    }
}
