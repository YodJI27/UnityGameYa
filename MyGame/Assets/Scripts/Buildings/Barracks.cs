using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barracks : Buildings
{
    public int MaxQueue = 3;

    [Header("References UI")]
    public GameObject btnPrefab;

    [Header("Shop")]
    public List<UnitData> m_ShopUnit = new List<UnitData>();

    [Header("References")]
    public Transform SpawnPoint;

    private bool p_Status;
    private List<int> p_UnitQueue = new List<int>();
    private float p_MaxTime;
    private float p_Timer;

    protected override void Start()
    {
        base.Start();

        Text[] textTemp = GameHUD.Instance.m_UnitShopPanel.GetComponentsInChildren<Text>();
        Image[] imgAvatar = GameHUD.Instance.m_UnitShopPanel.GetComponentsInChildren<Image>();

        for(int i = 0; i < m_ShopUnit.Count; i++)
        {
            textTemp[i].text = m_ShopUnit[i].Name + '\n' + "<size=12><color=#000000>ЗОЛОТО:" + m_ShopUnit[i].AmoundGold + '\n' + 
                                                                                    "Еда:" + m_ShopUnit[i].AmountFood + "</color></size>";
            imgAvatar[i == 0 ? 2 : 2 * i + 2].sprite = m_ShopUnit[i].Avatar;
        }
    }

    protected override void Update()
    {
        base.Update();

        //Проверяем статус найма юнитов
        if(p_Status)
        {
            //Проверяем кол-во юнитов в очереди
            if (p_UnitQueue.Count > 0)
            {
                //Устанавливаем время нужное для найма
                p_MaxTime = m_ShopUnit[p_UnitQueue[0]].HirigTime;
                p_Timer += Time.deltaTime;

                //Если время вышло, то нанимаем юнита
                if(p_Timer >= p_MaxTime)
                {
                    //Спавним юнита
                    Instantiate(m_ShopUnit[p_UnitQueue[0]].Prefab, SpawnPoint.position, Quaternion.identity);
                    //Удаляем юнита с очереди
                    p_UnitQueue.RemoveAt(0);
                    //Обнуляем таймер для следующего по очереди
                    p_Timer = 0.0f;
                }

            }else
            {
                //Если статус false прекращаем найм юнитов
                p_Status = false;
            }

        }
    }

    //Нужен для перевода времени в проценты для прогресс бара
    private float HiringTimeToPercent()
    {
        return p_Timer / p_MaxTime;
    }

    //Метод добавляет юнита в очередь найма
    public void HiringUnit(int index)
    {
        p_UnitQueue.Add(index);
        p_Status = true;
    }

    public override void Select()
    {
        base.Select();

        //Отображаем панель с магазином юнитов
        GameHUD.Instance.m_UnitShopPanel.SetActive(true);

        //Вызываем метод отображающий прогресс найма юнитов
        ShowProgressHiring();
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        
        //Вызываем метод отображающий прогесс найма юнитов
        ShowProgressHiring();
    }

    public override void Diselect()
    {
        base.Diselect();
        
        //Скрываем панели нужные для казармы
        GameHUD.Instance.HiringUnitPanel.SetActive(false);
        GameHUD.Instance.m_UnitShopPanel.SetActive(false);
    }

    //Метод отображающий прогесс найма юнитов
    private void ShowProgressHiring()
    {
        if (p_Status)
        {
            GameHUD.Instance.m_TextDescription.gameObject.SetActive(false);
            //Устанавливаем прогресс бар найма юнита
            GameHUD.Instance.ProgressBar.fillAmount = HiringTimeToPercent();

            //Цикл по установке иконок
            for (int i = 0; i < GameHUD.Instance.QueueHiringUnitAvatar.Count; i++)
            {
                if (i < p_UnitQueue.Count)
                {   //Отображаем иконки юнитов в очереди
                    GameHUD.Instance.QueueHiringUnitAvatar[i].sprite = m_ShopUnit[p_UnitQueue[i]].Avatar;
                    GameHUD.Instance.QueueHiringUnitAvatar[i].gameObject.SetActive(true);
                }
                else
                {   //Скрываем если иконка не нужна
                    GameHUD.Instance.QueueHiringUnitAvatar[i].gameObject.SetActive(false);
                }
            }
            //Отображаем панель с прогрессом найма
            GameHUD.Instance.HiringUnitPanel.SetActive(true);
        }else
        {
            //Скрываем панель с прогрессом найма
            GameHUD.Instance.HiringUnitPanel.SetActive(false);
            GameHUD.Instance.m_TextDescription.gameObject.SetActive(true);
        }
    }
}
