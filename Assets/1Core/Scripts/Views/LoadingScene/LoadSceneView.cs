using System;
using Core.Scripts.Views;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneView : MonoBehaviour
{
    [SerializeField] private bool _isStartLoadScene;

    private void Start()
    {
        //KTGameCenter.SharedCenter().Authenticate();
        if (_isStartLoadScene) LoadingManager.Instantiate.LoadScene(Str.Main);
    }

    [Serializable]
    private class LoadSceneModel
    {
        public Button btnLoadScene;
        public NameScene nameScene;
    }

    private enum NameScene
    {
        Load,
        Main
    }
}