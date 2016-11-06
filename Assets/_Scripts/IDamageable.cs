using UnityEngine;
using System.Collections;

public interface IDamageable
{
    void receiveDamage(int amount);

    int Health { get; }
    int MaxHealth { get; }
}
