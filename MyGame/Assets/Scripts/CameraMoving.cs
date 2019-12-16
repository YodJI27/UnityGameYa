using UnityEngine;

public class CameraMoving : MonoBehaviour
{

    public int _width;
    public int _height;
    public float speed = 50;

    private void Start()
    {
        _width = Screen.width;
        _height = Screen.height;
    }

    private void Update()
    {
        Vector2 campos = transform.position;

        if (Input.mousePosition.x <= 20)
        {
            campos.x -= Time.deltaTime * speed;
        }
        else if (Input.mousePosition.x >= _width - 20)
        {
            campos.x += Time.deltaTime * speed;
        }
        else if (Input.mousePosition.y <= 20)
        {
            campos.y -= Time.deltaTime * speed;
        }
        else if (Input.mousePosition.y >= _height - 20)
        {
            campos.y += Time.deltaTime * speed;
        }
        transform.position = new Vector2(Mathf.Clamp(campos.x, -23.17f, 26f), Mathf.Clamp(campos.y,-14.17f, 15f));
        transform.position += new Vector3(0f, 0f, -10f);
    }
}

