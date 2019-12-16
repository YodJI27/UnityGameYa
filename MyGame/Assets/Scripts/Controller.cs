using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public bool startBay = false;
    public int Indexshop = -1;

    private Ray _ray;
    private RaycastHit _hit;

    Camera m_Camera;

    private void Awake()
    {
    }
    private void Start()
    {
        m_Camera = Camera.main;
    }
    void Update()
    {
        if (startBay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameMod.Instance.m_Gold >= GameMod.Instance.Shop[Indexshop].Cost)
                {   //Создаем 2D рейкаст
                    Vector2 mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D _hit = Physics2D.Raycast(mousePos, Vector2.down, 10.0f);
                    //Проверяем попали в область для строительства или нет
                    if (_hit.collider != null && _hit.collider.CompareTag("Area"))
                    {
                        Area _area = _hit.collider.GetComponent<Area>();
                        GameMod.Instance.m_Gold -= GameMod.Instance.Shop[Indexshop].Cost;

                        _area.Status = true;
                        _area.Enable = false;
                        _area.Gobject = Instantiate(GameMod.Instance.Shop[Indexshop].GObject, new Vector3(_hit.point.x, _hit.point.y, -1.0f), Quaternion.identity);
                        Indexshop = -1;

                        foreach (Area _tmp in GameMod.Instance.Areas)
                                _tmp.gameObject.SetActive(false);

                        startBay = false;
                    }
                }
            }
        }else
        {   //Если нет процесса покупки то можем выберать объекты по кнопке ЛКМ
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousPos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D _hit = Physics2D.Raycast(mousPos, Vector2.down, 10.0f);

                if (_hit.collider != null && _hit.collider.CompareTag("Building") || _hit.collider.CompareTag("Unit"))
                {
                    //Если есть выбрынный объект то снимаем с него выделение
                    if(GameMod.Instance.ActionWithSelectedObject != null)
                        GameMod.Instance.ActionWithSelectedObject.Diselect();

                    //Добавляем новый обьект выделения
                    GameMod.Instance.ActionWithSelectedObject = _hit.collider.GetComponent<IActionWithObject>();
                    GameMod.Instance.ActionWithSelectedObject.Select();
                    GameMod.Instance.SelectedObject = _hit.collider.gameObject;
                }
            }
        }

        //При нажатии на ПКМ, отдаем приказ двигатся отряду или атаковать
        if(Input.GetMouseButtonUp(1))
        {
            if (GameMod.Instance.SelectSquad.Count > 0) //Если кол-во юнитов в отряде больше 0 то можем отдавать приказы
            {
                Vector2 mousPos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D _hit = Physics2D.Raycast(mousPos, Vector2.down, 10.0f);
                
                //Если попали во врага то отдаем приказ атаковать отряду
                if (_hit.collider != null && _hit.collider.CompareTag("Unit") && _hit.collider.GetComponent<Unit>().m_Team == Team.Hostile)
                {
                    GameMod.Instance.SelectSquad.Attack(_hit.collider.GetComponent<Unit>());
                }else //Иначе двигаем отряд в точку
                {
                    //Если есть выбрынный объект то снимаем с него выделение
                    if (GameMod.Instance.ActionWithSelectedObject != null)
                        GameMod.Instance.ActionWithSelectedObject.Diselect();

                    Vector2 dest = GameMod.Instance.m_Camera.ScreenToWorldPoint(Input.mousePosition);
                    GameMod.Instance.SelectSquad.MoveToPoint(dest);
                }
            }
        }

        //Снимаем выделение по нажатию на Escape
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            GameMod.Instance.ActionWithSelectedObject.Diselect();
            GameMod.Instance.ActionWithSelectedObject = null;
        }
    }

    public void StartBay(int _index)
    {
        if(GameMod.Instance.m_Gold >= GameMod.Instance.Shop[_index].Cost )
        {
            Indexshop = _index;
            foreach(Area _tmp in GameMod.Instance.Areas)
            {
                if(!_tmp.Status)
                {
                    _tmp.gameObject.SetActive(true);
                }
            }
            startBay = true;
        }
    }
}
