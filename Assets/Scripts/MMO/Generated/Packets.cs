using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class Packets
{
    public static BinaryWriter CreatePacket(ClientPacketType packetId)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write((byte)packetId);

        return writer;
    }

    public static BinaryWriter CreatePacket(ServerPacketType packetId)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write((byte)packetId);

        return writer;
    }

    public static void Write(this BinaryWriter writer, StatType stat, object val)
    {
        StatObjectType type = StatDataTypes.types[stat];
        writer.Write((byte)stat);

        switch(type)
        {
            case StatObjectType.STRING:
                writer.Write((string)val);
                break;
            case StatObjectType.INT16:
                writer.Write((short)val);
                break;
            case StatObjectType.INT32:
                writer.Write((int)val);
                break;
            case StatObjectType.FLOAT:
                writer.Write((float)val);
                break;
            case StatObjectType.BYTE:
                writer.Write((byte)val);
                break;
        }
    }

    public struct Stat
    {
        public StatType stat;
        public object val;
    }

    public static KeyValuePair<StatType, object> ReadStat(this BinaryReader reader)
    {
        var t = (StatType)reader.ReadByte();
        StatObjectType type = StatDataTypes.types[t];

        switch (type)
        {
            case StatObjectType.STRING:
                return new KeyValuePair<StatType, object>(t, reader.ReadString());

            case StatObjectType.INT16:
                return new KeyValuePair<StatType, object>(t, reader.ReadInt16());

            case StatObjectType.INT32:
                return new KeyValuePair<StatType, object>(t, reader.ReadInt32());

            case StatObjectType.FLOAT:
                return new KeyValuePair<StatType, object>(t, reader.ReadSingle());

            case StatObjectType.BYTE:
                return new KeyValuePair<StatType, object>(t, reader.ReadByte());
        }

        return new KeyValuePair<StatType, object>(t, 0);
    }
}