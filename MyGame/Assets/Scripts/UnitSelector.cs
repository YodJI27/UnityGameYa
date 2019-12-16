using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public bool Selected = false;
    public bool FindUnit = false;

    public Texture BoxSelect = null;

    private float _width = 0.0f;
    private float _height = 0.0f;

    private Vector3 _startPoint = Vector3.zero;
    private Vector3 _endPoint = Vector3.zero;

    private Rect s_rect;

    private Camera m_Camera;

    private void Start()
    {
        m_Camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameMod.Instance.SelectSquad.DiselectUnit();
            _startPoint = Input.mousePosition;
            Selected = true;
        }

        if (Selected)
        {
            _endPoint = Input.mousePosition;
            if (Input.GetMouseButtonUp(0))
            {
                s_rect = SelectRect(_startPoint, _endPoint);
                FindUnit = true;
                Selected = false;
            }
        }
        else if (FindUnit)
        {
            if (Mathf.Abs(_startPoint.x - _endPoint.x) >= 5.0f)
            {
                foreach(Unit tmp in GameMod.Instance.PlayerUnits)
                {
                    var pos = m_Camera.WorldToScreenPoint(tmp.transform.position);
                    pos.y = InvertY(pos.y);
                    if (s_rect.Contains(pos))
                    {
                        GameMod.Instance.SelectSquad.AddUnit(tmp);
                    }
                }
            }
            else
            {
                OnSelectRayUnit();
            }
            FindUnit = false;
        }
    }

    private void OnSelectRayUnit()
    {
        Vector2 mousPos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D _hit = Physics2D.Raycast(mousPos, Vector2.down, 10.0f);
        
        if (_hit.collider.CompareTag("Unit"))
        {
            if (_hit.collider.GetComponent<Unit>().m_Team == Team.Player)
                GameMod.Instance.SelectSquad.AddUnit(_hit.collider.GetComponent<Unit>());
        }
    }

    private void OnGUI()
    {
        if (Selected)
        {
            _width = _endPoint.x - _startPoint.x;
            _height = InvertY(_endPoint.y) - InvertY(_startPoint.y);
            GUI.DrawTexture(new Rect(_startPoint.x, InvertY(_startPoint.y), _width, _height), BoxSelect);
        }
    }

    private Rect SelectRect(Vector3 _start, Vector3 _end)
    {
        if (_width < 0.0f)
        {
            _width = Mathf.Abs(_width);
        }
        if (_height < 0.0f)
        {
            _height = Mathf.Abs(_height);
        }

        if (_endPoint.x < _startPoint.x)
        {
            _start.z = _start.x;
            _start.x = _end.x;
            _end.x = _start.z;
        }
        if (_endPoint.y > _startPoint.y)
        {
            _start.z = _start.y;
            _start.y = _end.y;
            _end.y = _start.z;
        }

        return new Rect(_start.x, InvertY(_start.y), _width, _height);
    }

    private float InvertY(float _y)
    {
        return Screen.height - _y;
    }
}

