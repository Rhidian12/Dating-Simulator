using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Json
{
    public class JsonReader : MonoBehaviour
    {
        public static T ReadFile<T>(string file)
        {
            StreamReader streamReader = new StreamReader(file);

            T convertedObject = JsonUtility.FromJson<T>(streamReader.ReadToEnd());

            return convertedObject;
        }
    }
}
