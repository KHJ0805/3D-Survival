using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{

    public int damage;
    public float damageRate;

    List<IDamageAble> things = new List<IDamageAble>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DealDamage",0,damageRate);
    }
    void DealDamage()
    {
        for(int i =0; i<things.Count; i++)
        {
            things[i].TakePhysicalDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamageAble damageAble))
        {
            things.Add(damageAble);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IDamageAble damageAble))
        {
            things.Remove(damageAble);
        }
    }
}
