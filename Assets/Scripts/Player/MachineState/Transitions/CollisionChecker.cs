using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : Transition
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<Wall>(out _))
        {
            NeedTransit = true;
        }
    }
}