using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Test : MonoBehaviour
{
    public CharacterData_SO player_data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            string jsonData = JsonUtility.ToJson(player_data);
            string path;
            if (Application.isEditor)
            {
                path = Path.Combine(Application.dataPath, "save.json");
            }
            else { path = Path.Combine(Application.persistentDataPath, "save.json"); }
            File.WriteAllText(path, jsonData);
            Debug.Log(path);
        }
    }
}
