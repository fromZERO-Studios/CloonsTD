using System;
using UnityEngine;

namespace CloonsTD;



[CreateAssetMenu(fileName = "NewCloon", menuName = "CloonsTD/Cloon Data")]
public class CloonData : ScriptableObject {
    [Header("Identidad y Visual")]
    public string cloonName = "Red Cloon";
    public Cloon prefab;
    public Color tintColor = Color.red;
    public Vector3 scale = Vector3.one;

    [Header("Estadísticas Base")]
    [Min(1)] public int maxHealth = 1;
    public float speed = 2.5f;
    public int cashReward = 1;

    [Header("Sistema de Capas (Hijos al reventar)")]
    [Tooltip("Cloons que aparecen al destruir este (ej: un Cloon Verde suelta un Azul)")]
    public CloonSpawnEntry[] childrenOnPop;
}

[Serializable]
public struct CloonSpawnEntry {
    public CloonData cloonData;
    [Min(1)] public int count;
    [Tooltip("Separación en el camino al dividirse (ej: Zebra suelta 1 Negro y 1 Blanco separados)")]
    public float pathSpacingOffset;
}