using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Bullet
{
    protected bool initialized;
    protected GlobalVariables globals;
    protected int damage;

    public abstract void Init(GlobalVariables globals);
    public abstract void OnHit(RaycastHit hitInfo, Vector3 origin);
}
