using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIChat : MonoBehaviour
{
    [SerializeField]
    private Text messagesText;

    [SerializeField]
    private InputField messageField;

    [SerializeField]
    private Button sendMessageBt;

    private void Awake()
    {
        sendMessageBt.onClick.AddListener(SendChatMessage);
    }

    private void OnGUI()
    {
        
    }

    private void SendChatMessage()
    {
        if(messageField.text == "")
        {
            return;
        }

        OutcomingPackets.SendPacket(ServerPacketType.ChatMessageRequest, messageField.text);
        messageField.text = "";
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ReceiveMessage(string message)
    {
        messagesText.text += message + Environment.NewLine;
    }
}
