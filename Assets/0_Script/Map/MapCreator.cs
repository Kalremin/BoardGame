using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelaunatorSharp;
using DelaunatorSharp.Unity;
using DelaunatorSharp.Unity.Extensions;

// 지도 씬의 지도 생성
public class MapCreator : MonoBehaviour
{

    [SerializeField]
    GameObject _mapPointPrb, _mapLinePrb;

    [SerializeField]
    Transform _lowerLeft, _topRight;

    [SerializeField]
    Transform _lineContainer, _pointContainer;

    [SerializeField]
    float minimumDistance = 0.3f;
    Delaunator delaunator;
    List<IPoint> points;
    List<MapPoint> mapPoints = new List<MapPoint>();
    List<Vector3[]> edges = new List<Vector3[]>();

    MapPoint startPoint, endPoint;

    private void OnEnable()
    {
        if (delaunator == null)
            CreateMapStage();
    }

    // 스테이지 지도 생성
    public void CreateMapStage()
    {
        var stagePoints = UniformPoissonDiskSampler.SampleRectangle(_lowerLeft.position, _topRight.position, minimumDistance);
        points = stagePoints.Select(point => new Vector2(point.x, point.y)).ToPoints().ToList();
        mapPoints.Clear();

        delaunator = new Delaunator(points.ToArray());

        // 포인트 생성
        delaunator.ForEachTriangleEdge(edge =>
        {
            Vector3 tempVecP = new Vector3(Mathf.Round(edge.P.ToVector3().x), Mathf.Round(edge.P.ToVector3().y));
            Vector3 tempVecQ = new Vector3(Mathf.Round(edge.Q.ToVector3().x), Mathf.Round(edge.Q.ToVector3().y));
            edges.Add(new Vector3[] { tempVecP, tempVecQ }); 

            if (mapPoints.Exists(vec => vec.transform.position == tempVecP))
                return;

            MapPoint point = Instantiate(_mapPointPrb, _pointContainer).GetComponent<MapPoint>();
            point.transform.SetPositionAndRotation(tempVecP, Quaternion.identity);
            point.GetComponent<SpriteRenderer>().sortingOrder = 2;
            point.SetAddtionalState();
            mapPoints.Add(point);

        });

        SetPointState();
       

        // 선 생성
        foreach(var tempVec in edges)
        {
            if (Vector3.Distance(tempVec[0], tempVec[1]) > Vector3.Distance(_lowerLeft.position, _topRight.position) / 2)
                continue;

            MapLine lineObj = Instantiate(_mapLinePrb, _lineContainer).GetComponent<MapLine>();
            lineObj.GetComponent<LineRenderer>().SetPositions(new Vector3[] { tempVec[0], tempVec[1] });
            lineObj.GetComponent<LineRenderer>().startWidth = .05f;
            lineObj.GetComponent<LineRenderer>().endWidth = .05f;
            lineObj.GetComponent<LineRenderer>().sortingOrder = 1;


            foreach (var tempPoint in mapPoints)
            {
                if (tempPoint.transform.position.Equals(tempVec[0]) || tempPoint.transform.position.Equals(tempVec[1]))
                {
                    if (tempPoint.ContainLine(lineObj))
                        continue;

                    tempPoint.AddLine(lineObj);
                    lineObj.AddPoint(tempPoint);

                }


            }
        }

        
    }


    void SetPointState()
    {
        for (int i = 0; i < 5; i++)
        {
            mapPoints[Random.Range(0, mapPoints.Count)].SetSemiEssentialState();
        }
        int ranInt = Random.Range(0, mapPoints.Count);
        mapPoints[ranInt].SetState(eMapPointState.UpStair);
        startPoint = mapPoints[ranInt];


        while (true)
        {
            var tempint = Random.Range(0, mapPoints.Count);

            if (mapPoints[tempint].GetState() != eMapPointState.UpStair)
            {
                mapPoints[tempint].SetState(eMapPointState.Boss);
                endPoint = mapPoints[tempint];
                break;
            }
        }

        while (true)
        {
            var tempint = Random.Range(0, mapPoints.Count);

            if (mapPoints[tempint].GetState() != eMapPointState.UpStair && mapPoints[tempint].GetState() != eMapPointState.Boss)
            {
                mapPoints[tempint].SetState(eMapPointState.Town);
                break;
            }
        }
    }

    public MapPoint GetStartPoint() => startPoint;
    public MapPoint GetEndPoint() => endPoint;

}
