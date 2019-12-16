using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]   //Нужен для рейкаста
[RequireComponent(typeof(Rigidbody2D))]     //Нужен для передвижения в 2D
[RequireComponent(typeof(MovingUnit))]      //Компонент передивжения
[RequireComponent(typeof(Attributes))]      //Компонент с атрибутами
public abstract class Unit : MonoBehaviour, IActionWithUnit, IActionWithObject
{
    [Header("Description")]
    public string Name;
    public string Description;

    public ETypesUnits Type;
    public Team m_Team;

    [Header("Attack")]
    public bool AutoAttack;
    public float RangeAttack;
    public float OrderRange;
    public float LostOrderRange;
    public float ReloadAttack;

    [Header("HealthBarUI")]
    public Image m_ProgressBar;

    private float p_TimerAttack;
    protected Vector3 p_startPositionAttack;
    
    [HideInInspector]
    public bool p_back;

    protected bool StatusAttack;
    protected Unit TargetAttack;
    protected float targetDistance;

    //Ссылка на компонент с атрибутами
    protected Attributes p_Attribute;

    protected MovingUnit p_Moving;

    protected virtual void Awake()
    {
        p_Attribute = GetComponent<Attributes>();
        p_Moving = GetComponent<MovingUnit>();

        p_TimerAttack = ReloadAttack;
    }

    protected virtual void Update()
    {
        if (!p_Attribute.Death)
        {
            //Если статус атаки true и цель есть
            if (StatusAttack && TargetAttack != null)
            {
                targetDistance = (TargetAttack.transform.position - transform.position).sqrMagnitude;
                if (targetDistance <= RangeAttack * RangeAttack) //Если цель входить в радиус атаки то атакуем
                {
                    p_Moving.Status = false; //Отключаем передвижение
                    p_TimerAttack -= Time.deltaTime;
                    if (p_TimerAttack <= 0.0f)
                    {
                        StatusAttack = TargetAttack.AnyDamage(p_Attribute.Damage); //Наносим урон
                        p_TimerAttack = ReloadAttack;   //Обновляем перезарядку оружия
                    }
                }else //Иначе двигаемся к цели, пока она не будет в радиусе атаки
                {
                    p_Moving.PrepareToMove(TargetAttack.transform.position);
                }

                //Если мы враг то проверяем не вышли ли за радиус отмены приказа атаки
                if (m_Team == Team.Hostile)
                {
                    float pos = (p_startPositionAttack - transform.position).sqrMagnitude;
                    if (pos >= LostOrderRange * LostOrderRange)
                    {
                        StatusAttack = false;
                        p_back = true;
                        p_Moving.PrepareToMove(p_startPositionAttack);
                    }
                }

            }else if (AutoAttack) //Если включена автоатака то ищем цель в радиусе приказа атаки
            {
                if(m_Team == Team.Player && !p_Moving.Status) //Если нет передвижения то ищем цель для атаки (Игрок)
                    FindTarget();

                if (m_Team == Team.Hostile && !p_back) //Если не возвращаемся в точку получения приказа то ищем цель
                    FindTarget();
            }
        }else
        {
            if(m_Team == Team.Player)
            {
                GameMod.Instance.PlayerUnits.Remove(this);
            }else
            {
                GameMod.Instance.HostileUnits.Remove(this);
            }
            Destroy(gameObject);
        }
    }

    protected virtual void Start()
    {   //Добавляем юнита в список для игрока или врага
        if(m_Team == Team.Player)
        {
            GameMod.Instance.PlayerUnits.Add(this);
        }else
        {
            GameMod.Instance.HostileUnits.Add(this);
        }
    }

    //Передвижение юнита
    public virtual void Destination(Vector2 dest)
    {
        StatusAttack = false;
        if(p_Moving != null)
            p_Moving.PrepareToMove(dest);
    }

    //Метод поиска цели в радиусе приказа атаки
    protected virtual void FindTarget()
    {
        if (m_Team == Team.Player)
        {
            foreach (Unit tmp in GameMod.Instance.HostileUnits)
            {
                targetDistance = (tmp.transform.position - transform.position).sqrMagnitude;
                if (targetDistance <= OrderRange * OrderRange)
                {
                    TargetAttack = tmp;
                    StatusAttack = true;
                }
            }
        }else
        {
            foreach (Unit tmp in GameMod.Instance.PlayerUnits)
            {
                targetDistance = (tmp.transform.position - transform.position).sqrMagnitude;
                if (targetDistance <= OrderRange * OrderRange)
                {
                    p_startPositionAttack = transform.position;

                    TargetAttack = tmp;
                    StatusAttack = true;
                }
            }
        }
    }

    //Метод устанавливает цель для атаки
    public virtual void Attack(Unit unit)
    {
        TargetAttack = unit;
        StatusAttack = true;
    }

    //Метод получения урона
    public virtual bool AnyDamage(float amount)
    {
        return p_Attribute.ApplayDamage(amount);
    }

    //Виртуальная реализация интерфейса IActionWithObject
    
    public virtual void Select()
    {//Метод вызывается при выборе объекта
        GameHUD.Instance.m_ObjectPanel.SetActive(true);
        //Устанавливаем значения для п. интерфейса
        GameHUD.Instance.m_TextName.text = Name;
        GameHUD.Instance.m_TextHealth.text = p_Attribute.curHealth + " / " + p_Attribute.maxHealth;
        GameHUD.Instance.m_TextDescription.text = Description;
    }

    public virtual void UpdateUI()
    {//Метод вызывается при обновлении пользовательского интерфейса
        GameHUD.Instance.m_TextHealth.text = p_Attribute.curHealth + " / " + p_Attribute.maxHealth;
    }

    public virtual void Diselect()
    {//Метод вызывается при снятии выделения с объекта
        GameHUD.Instance.m_ObjectPanel.SetActive(false);
    }
}
