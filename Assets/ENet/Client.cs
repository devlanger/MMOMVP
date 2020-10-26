using ENet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Client : MonoBehaviour
{
	public static Client Instance { get; private set; }

	[SerializeField]
	private string ip;

	[SerializeField]
	private ushort port;

	private Host client;
	private Peer peer;


    IEnumerator Start()
    {
		Instance = this;

		using (client = new Host())
		{
			Address address = new Address();

			address.SetHost(ip);
			address.Port = port;
			client.Create();

			peer = client.Connect(address);

            ENet.Event netEvent;

			while (true)
			{
				bool polled = false;

				while (!polled)
				{
					if (client.CheckEvents(out netEvent) <= 0)
					{
						if (client.Service(15, out netEvent) <= 0)
							break;

						polled = true;
					}

					switch (netEvent.Type)
					{
						case ENet.EventType.None:
							break;

						case ENet.EventType.Connect:
							Debug.Log("Client connected to server");
							break;

						case ENet.EventType.Disconnect:
							Debug.Log("Client disconnected from server");
							break;

						case ENet.EventType.Timeout:
							Debug.Log("Client connection timeout");
							break;

						case ENet.EventType.Receive:
							Debug.Log("Packet received from server - Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);
							byte[] buffer = new byte[1024];

							netEvent.Packet.CopyTo(buffer);
							MemoryStream stream = new MemoryStream(buffer);
							BinaryReader reader = new BinaryReader(stream);
							while (stream.Position < netEvent.Packet.Length)
							{
								byte packetId = reader.ReadByte();
								ClientPacketType packetType = (ClientPacketType)packetId;
								IncomingPackets.ReceivePacket(packetType, reader);
							}
							netEvent.Packet.Dispose();
							break;
					}
				}

				yield return 0;
			}

			client.Flush();
		}
	}

    public void SendPacket(BinaryWriter writer)
    {
		byte[] data = ((MemoryStream)writer.BaseStream).ToArray();
		Packet p = new Packet();
		p.Create(data);

		peer.Send(0, ref p);
		p.Dispose();
	}

    private void OnDestroy()
    {
		client.Flush();
    }
}
