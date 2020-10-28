using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobsManager : MonoBehaviour
{
    public static MobsManager Instance { get; private set; }

    [SerializeField]
    private Character basePrefab;

    public Dictionary<uint, Character> characters = new Dictionary<uint, Character>();

    private void Awake()
    {
        Instance = this;
    }

    public Character SpawnCharacter(SpawnData data)
    {
        Debug.Log("Add character: " + data.id);
        Character inst = Instantiate(basePrefab, new Vector3(data.pos.X, data.pos.Y, data.pos.Z), Quaternion.identity, transform);
        inst.Initialize(data.id);

        Debug.Log("Mob id: " + data.mobId);
        MobData mob = DataManager.Instance.GetMob(data.mobId);
        if(mob != null)
        {
            inst.GetComponent<ModelController>().Load("Mobs/" + mob.baseModel);
        }

        characters.Add(inst.id, inst);
        return inst;
    }

    public void RemoveCharacter(uint id)
    {
        if(characters.ContainsKey(id))
        {
            Destroy(characters[id].gameObject);
            characters.Remove(id);
        }
    }

    public Character GetCharacter(uint id)
    {
        if(!characters.ContainsKey(id))
        {
            return null;
        }

        return characters[id];
    }
}