using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreatureSO", menuName = "CreatureSO", order = 0)]
public class CreatureSO : ScriptableObject
{
    public GameObject prefab;
    public int health;
    public float speed;
    public int score;
    public int reward;
    public int damage;
}