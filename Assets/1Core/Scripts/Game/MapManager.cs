using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    [SerializeField] private MapModel[] _maps;
    
    public void EnableSlots(int index, List<Slot> slots)
    {
        int rows = _maps[index].boolMatrix.GetLength(0);
        int cols = _maps[index].boolMatrix.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                int flatIndex = i * cols + j;
                if (flatIndex < slots.Count)
                {
                    slots[flatIndex].box.gameObject.SetActive(_maps[index].boolMatrix[i, j]);
                }
            }
        }
    }

}

[Serializable]
public class MapModel
{
    [TableMatrix, SerializeReference] public bool[,] boolMatrix = new bool[9, 9];

    [Button]
    public void SetEnable(bool isEnable)
    {
        for (int i = 0; i < boolMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < boolMatrix.GetLength(1); j++)
            {
                boolMatrix[i, j] = isEnable;
            }
        }
    }
}