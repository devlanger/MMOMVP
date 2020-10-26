using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public uint id;

    [SerializeField]
    private float jumpSpeed = 5;

    [SerializeField]
    private float moveSpeed = 5;

    private Rigidbody rb;
    private Vector3 velocity;
    private Vector3 jumpVelocity;
    private float jumpTime;

    private bool grounded = true;

    public StatsContainer Stats;

    public Dictionary<ItemsContainerId, ItemsContainer<ItemsContainerId, ItemData>> containers = new Dictionary<ItemsContainerId, ItemsContainer<ItemsContainerId, ItemData>>();
    public Dictionary<SkillsContainerId, ItemsContainer<SkillsContainerId, SkillData>> skills = new Dictionary<SkillsContainerId, ItemsContainer<SkillsContainerId, SkillData>>();

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 1.1f);
    }

    public void Initialize(uint id)
    {
        this.id = id;
        Stats = new StatsContainer(id);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        foreach (var item in Enum.GetValues(typeof(ItemsContainerId)))
        {
            containers.Add((ItemsContainerId)item, new ItemsContainer<ItemsContainerId, ItemData>((ItemsContainerId)item, id));
        }

        foreach (var item in Enum.GetValues(typeof(SkillsContainerId)))
        {
            skills.Add((SkillsContainerId)item, new ItemsContainer<SkillsContainerId, SkillData>((SkillsContainerId)item, id));
        }
    }

    public void Move(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    private void FixedUpdate()
    {
        Vector3 vel = rb.velocity;
        if (grounded)
        {
            vel.x = velocity.x * moveSpeed;
            vel.z = velocity.z * moveSpeed;
        }
        else
        {
            vel.x = jumpVelocity.x * moveSpeed;
            vel.z = jumpVelocity.z * moveSpeed;
        }

        rb.velocity = vel;

        if(!grounded)
        {
            if(IsGrounded() && Time.time > jumpTime + 0.2f)
            {
                grounded = true;
            }
        }
        else
        {
            if (!IsGrounded())
            {
                grounded = false;
            }
        }
    }

    public void Jump()
    {
        jumpTime = Time.time;
        jumpVelocity = velocity;

        Vector3 vel = rb.velocity;
        vel.y = jumpSpeed;

        rb.velocity = vel;
    }
}
