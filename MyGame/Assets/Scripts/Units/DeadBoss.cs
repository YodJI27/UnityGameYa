using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBoss : Unit
{
    public GameObject deadbosspanel;

    protected override void Update()
    {
        if(p_Attribute.Death)
        {
            deadbosspanel.SetActive(true);
            Time.timeScale = 0;
        }

        base.Update();
    }
}
