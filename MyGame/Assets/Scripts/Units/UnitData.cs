using UnityEngine;

[System.Serializable]
public struct UnitData
{
    public string Name;         //Имя юнита
    public int AmoundGold;      //Сколько золота требуется
    public int AmountFood;      //Сколько еды требуется
    public float HirigTime;     //Время найма юнита
    public Sprite Avatar;       //Иконка юинта
    public GameObject Prefab;   //Ссылка на префаб юнита

}
