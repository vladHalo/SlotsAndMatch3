using System;
using System.Collections.Generic;
using UnityEngine;

public class BoolArrayVisualizer : MonoBehaviour
{
    [SerializeField] public List<ListMatrix> list = new List<ListMatrix>();

    private void OnValidate()
    {
        foreach (var matrix in list)
        {
            matrix.Validate();
        }
    }
}

[Serializable]
public class ListMatrix
{
    [SerializeField] public bool[] array;
    [SerializeField] public int rows = 3;
    [SerializeField] public int columns = 3;

    public void Validate()
    {
        if (array == null || array.Length != rows * columns)
        {
            array = new bool[rows * columns];
        }
    }
}