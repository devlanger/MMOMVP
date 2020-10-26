using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OutcomingPackets : MonoBehaviour
{
    public static void SendPacket(ServerPacketType packet, params object[] args)
    {
        var writer = Packets.CreatePacket(packet);

        switch (packet)
        {
            case ServerPacketType.LoginRequest:
                break;
            case ServerPacketType.MoveRequest:
                writer.Write((float)args[0]);
                writer.Write((float)args[1]);
                writer.Write((float)args[2]);
                break;
            case ServerPacketType.ChatMessageRequest:
                writer.Write((string)args[0]);
                break;
            case ServerPacketType.UseItem:
                writer.Write((byte)args[0]);
                writer.Write((byte)args[1]);
                writer.Write((int)args[2]);
                break;
            case ServerPacketType.ClickNpcRequest:
                writer.Write((uint)args[0]);
                writer.Write((byte)args[1]);
                break;
            case ServerPacketType.MoveEntitySlot:
                writer.Write((byte)(MoveEntityType)args[0]);
                writer.Write((int)args[1]);

                switch((MoveEntityType)args[0])
                {
                    case MoveEntityType.ITEM:
                        writer.Write((byte)(ItemsContainerId)args[2]);
                        writer.Write((byte)(ItemsContainerId)args[3]);
                        writer.Write((int)args[4]);
                        break;
                }

                break;
        }

        //Send
        Client.Instance.SendPacket(writer);
    }
}
