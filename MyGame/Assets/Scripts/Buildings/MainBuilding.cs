using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuilding : Buildings
{

    //Доролняем стандартыне методы выделения обьекта, что бы отображался магазин
    public override void Select()
    {
        base.Select();
        GameHUD.Instance.m_ShopPanel.SetActive(true);
    }

    public override void Diselect()
    {
        base.Diselect();
        GameHUD.Instance.m_ShopPanel.SetActive(false);
    }
}
