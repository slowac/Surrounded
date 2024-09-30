using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace slowac_Helpers
{
    public class DDOL : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this); // When loading a new scene, do not destroy this game object
            SceneManager.LoadScene(1); // Load the scene with scene index 1
        }
    }
}
