using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public static Tile SetTileToMove(Unit unit, Tile playerTile)
    //{

    //    /*
    //     전반적으로 BattleManager의 battleType을 길이로 활용
    //    BoardManager의 GetTile을 활용

    //    그외 여러 코드 수정 필요
    //     */
    //    //Vector3 unitTileVec = unit.transform.parent.position;
    //    //List<int> tempAlliesIdx = BoardManager._instance.GetTeamIdxTiles;
    //    //Tile[,] tempTileArrays = BoardManager._instance.GetTileIdxArrays;
    //    //Tile tempTile = null;
    //    //float tempDist = float.MaxValue;

    //    //if (BoardManager._instance.GetTeamIdxTiles.Count > 0)
    //    //{
    //    //    BoardManager._instance.ResetListTile();

    //    //    for(int i = 0; i < tempAlliesIdx.Count; i++)
    //    //    {
    //    //        int x, y;
    //    //        x = tempAlliesIdx[i] / tempTileArrays.GetLength(0);
    //    //        y = tempAlliesIdx[i] % tempTileArrays.GetLength(0);
                

    //    //        if (x - 1 > 0 && tempDist > Vector3.Distance(unitTileVec, tempTileArrays[x - 1, y].transform.position))
    //    //        {
    //    //            tempTile = tempTileArrays[x - 1, y];
    //    //            tempDist = Vector3.Distance(unitTileVec, tempTileArrays[x - 1, y].transform.position);
    //    //        }
    //    //        if (x + 1 < tempTileArrays.GetLength(0) && tempDist > Vector3.Distance(unitTileVec, tempTileArrays[x + 1, y].transform.position))
    //    //        {
    //    //            tempTile = tempTileArrays[x + 1, y];
    //    //            tempDist = Vector3.Distance(unitTileVec, tempTileArrays[x + 1, y].transform.position);
    //    //        }
    //    //        if (y - 1 > 0 && tempDist > Vector3.Distance(unitTileVec, tempTileArrays[x, y - 1].transform.position))
    //    //        {
    //    //            tempTile = tempTileArrays[x, y - 1];
    //    //            tempDist = Vector3.Distance(unitTileVec, tempTileArrays[x, y - 1].transform.position);
    //    //        }
    //    //        if (y + 1 < tempTileArrays.GetLength(0) && tempDist > Vector3.Distance(unitTileVec, tempTileArrays[x, y + 1].transform.position))
    //    //        {
    //    //            tempTile = tempTileArrays[x, y + 1];
    //    //            tempDist = Vector3.Distance(unitTileVec, tempTileArrays[x, y + 1].transform.position);
    //    //        }


    //    //    }

    //    //}
    //    //else
    //    //{
    //    //    float dist;
    //    //    while (BoardManager._instance.GetResetListIdxTile.Count > 0)
    //    //    {
    //    //        int tempIdx = BoardManager._instance.GetResetListIdxTile.Dequeue();
    //    //        int x, y;
    //    //        x = tempIdx / tempTileArrays.GetLength(0);
    //    //        y = tempIdx % tempTileArrays.GetLength(0);
    //    //        dist = Vector3.Distance(playerTile.transform.position, BoardManager._instance.GetTile(x, y).transform.position);

    //    //        if (tempDist > dist)
    //    //        {
    //    //            tempTile = BoardManager._instance.GetTile(x, y);
    //    //            tempDist = dist;
    //    //        }
    //    //    }

    //    //}
        
    //    //return tempTile;

    //}
}
