using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class StatLabel : MonoBehaviour
{
    public StatType stat = StatType.STA;
    public string format0 = "{0}";
    
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();    
    }

    public void Start()
    {
        text.text = string.Format(format0, 1);
    }
}
