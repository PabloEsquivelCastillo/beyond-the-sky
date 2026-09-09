using UnityEngine;

public class DamageInfo
{
 
    public int Amount { get; }
    public GameObject Source { get; }

    public DamageInfo(int amount, GameObject soruce)
    {
        Amount= amount;
        Source = soruce;
    }
}
