using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    [SerializeField]
    private Transform parent;

    [SerializeField]
    private GameObject defaultModel;

    public void Load(string baseModel)
    {
        Debug.Log("Load model: " + baseModel);
        var model = Resources.Load<GameObject>(baseModel);
        if(model != null)
        {
            var inst = Instantiate(model, parent.position, parent.rotation, parent);
            defaultModel.SetActive(false);
        }
    }
}
