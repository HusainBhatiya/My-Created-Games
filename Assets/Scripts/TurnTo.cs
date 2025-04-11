using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTo : MonoBehaviour
{
    [SerializeField] List<AttackPermission> givenPermissions = new List<AttackPermission>();
    int currentAttackIndex = 0;
    void Start()
    {
        if (givenPermissions.Count > 0)
        {
            givenPermissions[currentAttackIndex].StartTurn();
        }

        EnemyAI[] enemies = Object.FindObjectsByType<EnemyAI>(FindObjectsSortMode.InstanceID);
        AttackPermission player = givenPermissions.Find(u => u.isPlayerUnit);

        foreach (EnemyAI enemy in enemies)
        {
            //enemy.SetTarget(player);
        }

        StartTurn();
    }

    public void StartTurn()
    {
        AttackPermission currentParmission = givenPermissions[currentAttackIndex];

        if (currentParmission.TryGetComponent<EnemyAI>(out EnemyAI enemyAI))
        {
            //enemyAI.ExecuteTurn();

            Invoke(nameof(EndTurn), 1f); // Wait for 1 second before ending the turn
        }
        else
        {
            Debug.Log("Player's turn");
            //currentParmission.StartTurn();
        }
    }

    public void EndTurn()
    {
        givenPermissions[currentAttackIndex].EndTurn();

        currentAttackIndex = (currentAttackIndex + 1) % givenPermissions.Count;
        givenPermissions[currentAttackIndex].StartTurn();
    }

    public AttackPermission GetCurrentAttackPermission()
    {
        return givenPermissions[currentAttackIndex];
    }

}
