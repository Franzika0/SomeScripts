using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour {

    private int posX;
    private int posZ;

    private GameObject theObjOnTheGrid = null;
    private bool isOccupied = false;

    public void SetPosition(int x, int z){
        posX = x;
        posZ = z;
    }

    public void SetOccupied()=>isOccupied=!isOccupied;

    public Vector2Int GetPosition()=>new Vector2Int(posX,posZ);
}
