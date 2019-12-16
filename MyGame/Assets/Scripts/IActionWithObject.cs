//Действия с любыми обьектами в игре
public interface IActionWithObject
{
    //Вызывается при выборе объекта мышкой
    void Select();

    //Вызывается при снятии выделения
    void Diselect();

    //Вызывается при обновлении п. интерфейса
    void UpdateUI();
}
