using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 지도 위의 선분
public class MapLine : MonoBehaviour
{
    [SerializeField]
    Material _choiceMaterial, _notChoiceMaterial;

    List<MapPoint> mapPoints = new List<MapPoint>();

    public bool moveState = false;

    public void NotChoiceMaterial()
    {
        GetComponent<LineRenderer>().material = _notChoiceMaterial;
    }

    public void ChoiceMaterial()
    {
        GetComponent<LineRenderer>().material = _choiceMaterial;
    }

    public void AddPoint(MapPoint point)
    {
        mapPoints.Add(point);
    }

    public bool ContainPoint(MapPoint point)
    {
        return mapPoints.Contains(point);
    }

    public void UsedLine()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }
}
