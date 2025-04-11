using UnityEngine;

public class AttackScript : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] ParticleSystem projectTile;
    [SerializeField] private float attackRange = 5f;
    Transform target;

    void Update()
    {
        AimToTarget();
    }

    void AimToTarget()
    {
        float targetDistance = Vector3.Distance(transform.position, target.position);

        player.LookAt(target);

        if (targetDistance < attackRange)
        {
            Attack(true);
        }
        else
        {
            Attack(false);
        }
    }

    void Attack(bool isActive)
    {
        var Emmision = projectTile.emission;
        Emmision.enabled = isActive;
    }
}
