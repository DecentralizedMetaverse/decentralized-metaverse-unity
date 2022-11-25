using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ExeEvent
{
    string GetHint();
    void Exe(Vector3 vec);
}
public interface Damage
{
    void Damage(int id, int point, Vector3 vec);
}
