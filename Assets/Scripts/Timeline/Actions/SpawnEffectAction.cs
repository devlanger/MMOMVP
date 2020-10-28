using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Actions/Spawn Effect")]
public class SpawnEffectAction : TimelineAction
{
    public bool self = true;
    public float destroyAfter = 0.5f;
    public GameObject effect;

    public override void Execute(Character user)
    {
        Character target = MobsManager.Instance.GetCharacter(1);

        if(target == null)
        {
            return;
        }

        GameObject inst = GameObject.Instantiate(effect, self ? user.transform.position : target.transform.position, Quaternion.identity);

        if (destroyAfter != 0)
        {
            Destroy(inst, destroyAfter);
        }

        /*EffectController effectController = inst.GetComponent<EffectController>();
        if(effectController != null)
        {
            effectController.Run(user, target);
        }*/
    }
}
