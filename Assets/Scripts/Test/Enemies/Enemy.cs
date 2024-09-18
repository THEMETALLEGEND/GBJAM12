using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Enemy 
{
    public void Damage(int damageAmount);
    public void SetRoom(int r);
    public void KnockBack_(Vector2 pos); 
}
