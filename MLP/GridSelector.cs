using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSelector : MonoBehaviour{
    
    [SerializeField] GridSpawner gridSpawner;
    [SerializeField] private LayerMask gridLayer;
    private Transform lastSelected = null;

    private bool isDrawing = false;
    //[SerializeField] Material hover;

    //[SerializeField] public GameObject housePrefab;

    private void Update() {
        GridCell theGridCellwhichIsTargeted = GetGridCellwhichIsTargeted();

        /*if(Input.GetMouseButtonDown(0)){
            lastSelected.GetComponent<GridCell>().SetOccupied();
            Debug.Log("house!");
            GameObject Entity = Instantiate(housePrefab, new Vector3(lastSelected.position.x,lastSelected.position.y,lastSelected.position.z),Quaternion.Euler(0,-90,0));
            Entity.transform.parent = lastSelected.transform;
            Entity.transform.localPosition = new Vector3(0,0,0);
        }*/
        if(Input.GetMouseButtonDown(0)){
            isDrawing = true;
        }
        if(Input.GetMouseButtonUp(0)){
            isDrawing = false;
        }

        if(theGridCellwhichIsTargeted != null && isDrawing){
            Transform gt = theGridCellwhichIsTargeted.transform;
            //theGridCellwhichIsTargeted.GetComponent<MeshRenderer>().material = hover;
            gt.GetChild(0).gameObject.SetActive(true);
            /*if(lastSelected)lastSelected.GetChild(0).gameObject.SetActive(false);
            lastSelected = gt;*/
        }
    }

    private GridCell GetGridCellwhichIsTargeted(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out RaycastHit hitObj, 200f, gridLayer)?
                hitObj.transform.GetComponent<GridCell>() : null; 
    }

}

