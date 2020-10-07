using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Effect : ScriptableObject
{
    abstract public void OnEffectPlay(Messiah.Logic.GameData gb, int number);
    abstract public KeyValuePair<string,int> GetNumberByString(string numberName, Messiah.Logic.GameData gb);

    //abstract public void OnEffectPlay(HealthAndStatuses caster, HealthAndStatuses casterOpponent, int power = 0);
    //abstract public KeyValuePair<string,int> GetNumberByString(string numberName, HealthAndStatuses caster);
   
}