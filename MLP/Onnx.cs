using UnityEngine;
using Unity.Barracuda;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;
public class Onnx : MonoBehaviour{
  public NNModel onnxAsset;
  private Model m_RuntimeModel;
  private IWorker worker;

  private List<float> tensors;

  [SerializeField] Button predictBtn;
  [SerializeField] TextMeshProUGUI predictTxt;
  [SerializeField] GridSpawner gridSpawner;
  [SerializeField] GameObject witch;
  private Animator animator;




  private List<float[]> test;
  private int Count = -1;


  private void Start() {

    tensors = new List<float>();
    test = new List<float[]>();
    test.Add(new float[]{0, 0, 255, 0, 0, 255, 255, 255, 0, 0, 255, 0, 255, 0, 0, 0, 0, 255, 0, 0, 0, 0, 255, 0, 0, 0, 0, 255, 0, 0, 0, 0, 255, 0, 0, 0, 0, 255, 0, 0, 255, 255, 255, 255, 255, });
    test.Add(new float[]{0, 255, 255, 255, 0, 255, 255, 255, 255, 255, 0, 0, 0, 0, 255, 0, 0, 0, 0, 255, 0, 0, 255, 255, 0, 0, 0, 255, 255, 255, 0, 0, 0, 0, 255, 255, 0, 0, 0, 255, 255, 255, 255, 255, 255, });
    test.Add(new float[]{255, 255, 255, 255, 255, 255, 0, 0, 0, 0, 255, 0, 0, 0, 0, 255, 255, 255, 255, 255, 255, 0, 0, 0, 255, 0, 0, 0, 0, 255, 0, 0, 0, 0, 255, 255, 0, 0, 0, 255, 255, 255, 255, 255, 255, });
    test.Add(new float[]{255, 255, 255, 255, 255, 255, 0, 0, 0, 255, 0, 0, 0, 255, 255, 0, 0, 0, 255, 0, 0, 0, 255, 255, 0, 0, 0, 255, 0, 0, 0, 0, 255, 0, 0, 0, 255, 255, 0, 0, 0, 255, 0, 0, 0, });
    test.Add(new float[]{255, 255, 255, 255, 255, 255, 0, 0, 0, 255, 255, 0, 0, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 0, 255, 0, 0, 0, 0, 255, 0, 0, 0, 0, 255, 0, 0, 0, 255, 255, 255, 255, 255, 255, 0, });
    test.Add(new float[]{0, 255, 255, 255, 0, 255, 255, 255, 255, 0, 255, 0, 0, 255, 255, 0, 0, 0, 0, 255, 0, 0, 0, 0, 255, 0, 0, 0, 255, 255, 0, 255, 255, 255, 0, 0, 255, 0, 0, 0, 255, 255, 255, 255, 255, });
    test.Add(new float[]{0, 0, 255, 255, 0, 0, 0, 255, 255, 0, 0, 255, 0, 255, 0, 255, 255, 0, 255, 0, 255, 255, 255, 255, 255, 255, 0, 0, 255, 0, 0, 0, 0, 255, 0, 0, 0, 0, 255, 0, 0, 0, 0, 255, 0, });
    test.Add(new float[]{0, 0, 255, 255, 255, 255, 255, 255, 0, 0, 255, 255, 0, 0, 0, 255, 0, 0, 0, 0, 255, 0, 0, 0, 0, 255, 255, 255, 255, 255, 255, 0, 0, 0, 255, 255, 255, 0, 255, 255, 0, 255, 255, 255, 0, });
    test.Add(new float[]{0, 255, 255, 255, 255, 255, 255, 0, 0, 255, 255, 0, 0, 0, 255, 255, 0, 0, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 0, 255, 0, 0, 255, 255, 255, 0, 0, 0, 255, 255, 255, 255, 255, 255, });
    test.Add(new float[]{0, 255, 255, 255, 0, 255, 255, 0, 0, 255, 255, 0, 0, 0, 255, 255, 0, 0, 0, 255, 255, 0, 0, 0, 255, 255, 0, 0, 0, 255, 255, 0, 0, 0, 255, 255, 255, 0, 0, 255, 0, 255, 255, 255, 255});

    animator = witch.GetComponent<Animator>();

    m_RuntimeModel = ModelLoader.Load(onnxAsset);
    var worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharp, m_RuntimeModel);



    predictBtn.onClick.AddListener(()=>{

      animator.SetTrigger("Attack");

      Count += Count==9?-9:1;

      float[] q = test[Count];
      

      Tensor input = new Tensor(1, 1, 1,45, q);
      tensors.Clear();
      worker.Execute(input);
      var output = worker.PeekOutput();

      for(var i = 0; i < 10; i++){
        tensors.Add(output[i]);
      }

      float maxTensor = tensors.Max();
      predictTxt.text = tensors.IndexOf(maxTensor).ToString();
      tensors.Clear();

      Transform[,] datas = gridSpawner.GetGridCellList();
      int gt = 0;
      foreach (var item in datas){
        item.GetChild(0).gameObject.SetActive(test[Count][gt] == 0 ? false : true);
        gt += 1;
      }





      /*animator.SetTrigger("Attack");
      
      Transform[,] datas = gridSpawner.GetGridCellList();
      foreach (var item in datas){
          tensors.Add(item.GetChild(0).gameObject.activeSelf?255:0);
      }
      
      float[] q = new float[tensors.Count];
      for(var i = 0; i < tensors.Count -1; i++){
        q[i] = tensors[i];
      }
      

      Tensor input = new Tensor(1, 1, 1,tensors.Count, q);
      tensors.Clear();
      worker.Execute(input);
      var output = worker.PeekOutput();

      for(var i = 0; i < 10; i++){
        tensors.Add(output[i]);
        //Debug.Log(output[i]);
      }

      float maxTensor = tensors.Max();
      //Debug.Log(maxTensor);
      //Debug.Log(tensors.IndexOf(maxTensor));
      predictTxt.text = tensors.IndexOf(maxTensor).ToString();
      tensors.Clear();*/
    });
    
    
  }

}
