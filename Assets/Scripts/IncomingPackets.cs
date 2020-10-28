using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class IncomingPackets : MonoBehaviour
{
    public static void ReceivePacket(ClientPacketType packet, BinaryReader reader)
    {
        switch (packet)
        {
            case ClientPacketType.SpawnCharacter:
                SpawnCharacter(reader);
                break;
            case ClientPacketType.DespawnCharacter:
                DespawnCharacter(reader);
                break;
            case ClientPacketType.SetLocalPlayer:
                SetLocalPlayer(reader);
                break;
            case ClientPacketType.SetPlayerState:
                SetPlayerState(reader);
                break;
            case ClientPacketType.ChatMessageReceive:
                ReceiveChatMessage(reader);
                break;
            case ClientPacketType.SyncItems:
                ReceiveItems(reader);
                break;
            case ClientPacketType.DamageInfoReceive:
                DamageReceive(reader);
                break;
            case ClientPacketType.SyncStat:
                StatReceive(reader);
                break;
        }
    }

    private static void StatReceive(BinaryReader reader)
    {
        uint targetId = reader.ReadUInt32();
        var stat = Packets.ReadStat(reader);
        Debug.Log(targetId + " " + stat.Key + " " + stat.Value);

        var character = MobsManager.Instance.GetCharacter(targetId);
        if(character != null)
        {
            character.Stats.SetStat(stat.Key, stat.Value);
        }
    }

    private static void DamageReceive(BinaryReader reader)
    {
        CombatManager.Instance.ReceiveDamageInfo(new DamageInfo()
        {
            targetId = reader.ReadUInt32(),
            damage = reader.ReadInt32()
        });
    }

    private static void ReceiveItems(BinaryReader reader)
    {
        int itemsType = reader.ReadInt32();

        byte containerId = reader.ReadByte();
        byte count = reader.ReadByte();
        Debug.Log("Receive list " + itemsType  + " / " + containerId);

        switch (itemsType)
        {
            //items
            case 0:
                Debug.Log("Receive items" + containerId);
                var container = PlayerController.Instance.Character.containers[(ItemsContainerId)containerId];
                container.Clear();
                for (int i = 0; i < count; i++)
                {
                    int slot = reader.ReadInt32();
                    int id = reader.ReadInt32();
                    container.AddItem(slot, new ItemData() { id = id }, true);
                }

                container.Refresh();
                break;
            //skills
            case 1:
                var skills = PlayerController.Instance.Character.skills[(SkillsContainerId)containerId];
                skills.Clear();
                for (int i = 0; i < count; i++)
                {
                    int slot = reader.ReadInt32();
                    int id = reader.ReadInt32();
                    Debug.Log(slot + " / " + id);
                    skills.AddItem(slot, DataManager.Instance.GetSkill(id), true);
                }

                skills.Refresh();
                break;
        }
    }

    private static void ReceiveChatMessage(BinaryReader reader)
    {
        string message = reader.ReadString();

        FindObjectOfType<UIChat>().ReceiveMessage(message);
    }

    private static void SetPlayerState(BinaryReader reader)
    {
        uint id = reader.ReadUInt32();
        float x = reader.ReadSingle();
        float y = reader.ReadSingle();
        float z = reader.ReadSingle();

        var m = MobsManager.Instance.GetCharacter(id);
        if(m != null)
        {
            m.transform.position = new Vector3(x, y, z);
        }
    }

    private static void DespawnCharacter(BinaryReader reader)
    {
        uint id = reader.ReadUInt32();
        MobsManager.Instance.RemoveCharacter(id);
    }

    public static void SetLocalPlayer(BinaryReader reader)
    {
        uint id = reader.ReadUInt32();
        var mob = MobsManager.Instance.GetCharacter(id);
        Debug.Log(string.Format("Set character {0}", id));
        if (mob != null)
        {
            FindObjectOfType<PlayerController>().SetPlayer(mob);
        }
    }

    public static uint SpawnCharacter(BinaryReader reader)
    {
        uint id = reader.ReadUInt32();
        int baseId = reader.ReadInt32();
        float x = reader.ReadSingle();
        float y = reader.ReadSingle();
        float z = reader.ReadSingle();

        List<KeyValuePair<StatType, object>> statsReceived = new List<KeyValuePair<StatType, object>>();

        for (int i = 0; i < 3; i++)
        {
            statsReceived.Add(Packets.ReadStat(reader));
        }

        Debug.Log(string.Format("Spawn character {0} {1} {2} {3}", id, x, y, z));
        var data = new SpawnData()
        {
            id = id,
            mobId = baseId,
            pos = new System.Numerics.Vector3(x, y, z)
        };

        var mob = MobsManager.Instance.SpawnCharacter(data);
        foreach (var item in statsReceived)
        {
            mob.Stats.SetStat(item.Key, item.Value);
        }
        return id;
    }
}
