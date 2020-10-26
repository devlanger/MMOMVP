using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public SkillData skillData;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text nameText;

    public void Fill(SkillData data)
    {
        this.skillData = data;
        if(data == null)
        {
            icon.GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            if(nameText != null)
            {
                nameText.text = data.name;
            }
            icon.sprite = FindObjectOfType<SpritesManager>().GetIcon(data.iconId);

            icon.GetComponent<CanvasGroup>().alpha = 1;
        }
    }
}
