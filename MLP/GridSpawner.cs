using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpawner : MonoBehaviour {

    private int width = 9;
    private int height = 5;
    private float gridSpaceSize = 5.3f;
    [SerializeField] private Transform gridCellPrefab;
    [SerializeField] private Button clearBtn;
    [SerializeField] private List<int> tensor;

    private Transform[,] gridCellsList;

    private void Start() {
        gridCellsList = new Transform[width,height];
        tensor = new List<int>();
        SpawnGrid(width, height, 0);
        clearBtn.onClick.AddListener(Clear);
    }

    private void SpawnGrid(int width, int height,int index){
        if(index == width )return;
        //StartCoroutine(GridCreating(index, height, 0));
        GridCreating(index, height, 0);
        SpawnGrid(width, height,index += 1);
    }

    private void GridCreating(int widthVal, int height, int index){
        if(index == height )return;
        //yield return null;
        gridCellsList[widthVal,index] = Instantiate(gridCellPrefab, new Vector3(widthVal * gridSpaceSize, 0f, index* +gridSpaceSize), Quaternion.identity);
        gridCellsList[widthVal,index].parent = this.transform;
        //Debug.Log($"{widthVal.ToString()},{index.ToString()}");
        gridCellsList[widthVal,index].gameObject.GetComponent<GridCell>().SetPosition(widthVal, index);
        gridCellsList[widthVal,index].name = $"{widthVal.ToString()},{index.ToString()}";
        
        GridCreating(widthVal, height, index+=1);
        //yield return new WaitForSeconds(.025f);
    }

    private string allr = "[";

    private void Clear(){
        tensor.Clear();
        foreach (var item in gridCellsList){
            tensor.Add(item.GetChild(0).gameObject.activeSelf?255:0);
            item.GetChild(0).gameObject.SetActive(false);
        }
        string result = "[";
        foreach (var item in tensor){
            result += item.ToString() + ", ";
        }
        result.Remove(result.Length - 1);
        result.Remove(result.Length - 1);
        result += "]";
        //Debug.Log(result);
        allr += result + ", ";
        Debug.Log(allr);

        /*foreach (var item in gridCellsList){
            item.GetChild(0).gameObject.SetActive(false);
        }*/

    }


    public Transform[,] GetGridCellList()=>gridCellsList;
    
}

