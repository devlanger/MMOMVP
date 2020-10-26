using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public SkillData skillData;

    [SerializeField]
    private Image icon;

    public void Fill(SkillData data)
    {
        this.skillData = data;
        if(data == null)
        {
            icon.GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            icon.GetComponent<CanvasGroup>().alpha = 1;
        }
    }
}
