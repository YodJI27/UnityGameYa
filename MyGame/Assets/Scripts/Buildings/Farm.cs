using UnityEngine;

public class Farm : Buildings
{
    public float IntervalAddFood;
    public int FoodPerTime;

    private float p_Timer;

    protected override void Start()
    {
        base.Start();

        Description = "Ферма приносит " + FoodPerTime + "ед. еды, каждые " + IntervalAddFood + "сек.";
    }

    protected override void Update()
    {
        base.Update();

        //Добавляем еду игроку каждые IntervalAddFood секунд
        p_Timer -= Time.deltaTime;
        if(p_Timer <= 0.0f)
        {
            p_Timer = IntervalAddFood;
            GameMod.Instance.m_Food += FoodPerTime;
            GameHUD.Instance.UpdateResourcesPanel();
        }
    }
}
