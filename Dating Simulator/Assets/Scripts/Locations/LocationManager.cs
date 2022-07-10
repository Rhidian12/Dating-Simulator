using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationManager : MonoBehaviour
{
    public List<Location> Locations { get; set; }

    public void OnLocationSelected(Location selectedLocation)
    {
        SceneManager.LoadScene(selectedLocation.LocationToGoTo);
    }
}
