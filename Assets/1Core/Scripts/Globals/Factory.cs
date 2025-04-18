using System;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

[Serializable]
public class Factory<T> where T : Component
{
    [SerializeField] private List<T> _prefab;
    [SerializeField] private Transform _parent;

    public T Create(Vector3 position, Quaternion quaternion, Transform parent)
    {
        return LeanPool.Spawn(_prefab[0], position, quaternion, parent);
    }

    public T Create(int indexPrefab, Vector3 position, Quaternion quaternion, Transform parent)
    {
        return LeanPool.Spawn(_prefab[indexPrefab], position, quaternion, parent);
    }

    public T Create(Vector3 position, Quaternion quaternion)
    {
        return LeanPool.Spawn(_prefab[0], position, quaternion, _parent);
    }

    public T Create(int indexPrefab, Vector3 position, Quaternion quaternion)
    {
        return LeanPool.Spawn(_prefab[indexPrefab], position, quaternion, _parent);
    }
}