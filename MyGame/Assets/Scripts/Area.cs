using UnityEngine;

public class Area : MonoBehaviour
{
    public bool Status = false;
    public bool Enable = false;

    public GameObject Gobject = null;

    private void Start()
    {
        GameMod.Instance.Areas.Add(this);
        gameObject.SetActive(false);
    }
}
