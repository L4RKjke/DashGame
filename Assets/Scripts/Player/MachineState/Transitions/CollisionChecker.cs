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