using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyLibrary", menuName = "Scriptable Objects/EnemyLibrary")]
public class EnemyLibrary : ScriptableObject
{
    [field: SerializeField]public List<EnemyData> Enemies { get; private set; }
}
