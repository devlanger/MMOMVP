using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public Character Character { get; private set; }
        
    [SerializeField]
    private ThirdPersonCameraController camera;

    [SerializeField]
    private bool test = false;


    public event Action<Character> OnLocalPlayerChanged = delegate { };

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(SendInfo());
    }


    private IEnumerator SendInfo()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f / 30f);
            if (Character != null)
            {
                OutcomingPackets.SendPacket(ServerPacketType.MoveRequest, (byte)Character.transform.eulerAngles.y, Character.transform.position.x, Character.transform.position.y, Character.transform.position.z);
            }
        }
    }

    private void Update()
    {
        if(Character == null)
        {
            return;
        }

        if (Input.GetMouseButton(1))
        {
            camera.UpdateInput();
        }

        camera.UpdateCamera();

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 dir = input;

        Vector3 forward = camera.transform.forward;
        forward.y = 0;

        dir = Quaternion.LookRotation(forward) * dir;
        dir = dir.normalized;

        if(input != Vector3.zero)
        {
            if(input.z < 0)
            {
                Character.transform.rotation = Quaternion.LookRotation(-dir);
            }
            else
            {
                Character.transform.rotation = Quaternion.LookRotation(dir);
            }
        }

        Character.Move(dir);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Character.Jump();
        }
    }

    public void SetPlayer(Character target)
    {
        this.Character = target;
        camera.SetTarget(Character.transform);

        OnLocalPlayerChanged(target);
    }
}
