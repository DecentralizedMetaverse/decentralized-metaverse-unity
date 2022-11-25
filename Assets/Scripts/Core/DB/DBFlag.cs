using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBFlag", menuName = "DB/DBFlag")]
public class DBFlag : ScriptableObject
{
    public byte scenario_flag = 0;
    public byte sub_scenario_flag = 0;
    public ushort flag = 0b_00000000_00000000;
    public void SetFlag(byte n, bool b)
    {
        if (b) flag |= (ushort)(1 << n - 1);
        else flag &= (ushort)(~(1 << n - 1));
    }
    public ushort GetFlag(byte n)
    {
        if ((flag & (1 << n - 1)) >= 1) return 1;
        else return 0;
    }
    public enum type
    {
        none, scenario, subScenario, flag
    }
    public enum exe
    {
        none, hit, key, first, always
    }
    public enum exe2
    {
        equal, greater, less
    }
}
