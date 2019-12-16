using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class unitSquad
{
    public int Count { get { return _units.Count; } private set { } }

    private List<Unit> _units = new List<Unit>();

    public void AddUnit(Unit unit)
    {
        if (!_units.Contains(unit))
            _units.Add(unit);
    }

    public void DiselectUnit()
    {
        _units.Clear();
    }

    public void MoveToPoint(Vector2 _point)
    {
        for(int i = 0; i < _units.Count; i++)
        {
            _units[i].Destination(new Vector2(Random.Range(_point.x - 0.5f, _point.x + 0.5f), Random.Range(_point.y - 0.5f, _point.y + 0.5f)));
        }
    }

    public void Attack(Unit _target)
    {
        foreach(Unit tmp in _units)
        {
            tmp.Attack(_target);
        }
    }
}

