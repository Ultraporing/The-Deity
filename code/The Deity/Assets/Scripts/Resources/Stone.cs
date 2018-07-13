/*
    Written by Tobias Lenz
 */

using Assets.Scripts.AI;
using Assets.Scripts.AI.Creature;
using Assets.Scripts.AI.Creature.Villager;
using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Planet;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to attach to the stone object, kills villagers and provides a Stone Resource
/// </summary>
public class Stone : MonoBehaviour {

    public AudioClip m_Impact;

    /// <summary>
    /// If the Ground was hit embed the stone and provide it as a resource.
    /// If a creature was hit with enough velocity kill it.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Basic Ground")
        {
            AudioSource.PlayClipAtPoint(m_Impact, transform.position);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;

            transform.position = new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z);
            var guo = new GraphUpdateObject(GetComponentInChildren<Collider>().bounds);
            guo.updatePhysics = true;
            AstarPath.active.UpdateGraphs(guo);
        }
        else if (collision.collider.tag == "Creature" && collision.relativeVelocity.magnitude > 2)
        {
            collision.collider.GetComponent<CreatureAI>().CreatureStats.Die();
            if(PlanetDatalayer.Instance.GetManager<GoalManager>().m_KillAVillager == false && PlanetDatalayer.Instance.GetManager<GoalManager>().m_CycleNumber == 3)
            {
                PlanetDatalayer.Instance.GetManager<GoalManager>().m_KillAVillager = true;
                PlanetDatalayer.Instance.GetManager<GoalManager>().m_SomethingChanged = true;
            }

            if (collision.collider.GetComponent<CreatureAI>().CreatureStats.m_CreatureType == Assets.Scripts.Creatures.CreatureStats.CreatureType.Villager)
            {
                foreach (VillagerAI v in PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList)
                {
                    PlanetDatalayer.Instance.GetManager<FoMManager>().IncreaseFoM(PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList.IndexOf(v), 7);
                }
            }
        }
    }
}
