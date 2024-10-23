using UnityEngine;

public class GhostRoom : MonoBehaviour
{
    public GameObject[] EMFLights;
    public AudioSource sfx;
    // Start is called before the first frame update
    public bool ghost1 = true;
    public bool ghost2 = false;
    public bool ghost3 = false;


    public bool EMF = false;
    public bool Freezing = false;
    
    void Start()
    {
        ghostAtts();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ghostAtts()
    {
        if (ghost1)
        {
            EMF = true;
            Freezing = false;
        }

        if (ghost2)
        {
            EMF = false;
            Freezing = true;
        }

        if (ghost3)
        {
            EMF = true;
            Freezing = true;
        }
    }

    #region EMFENABLE
    //EMF activatie
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && EMF == true)
        {
            Debug.Log("Entered " + gameObject.name);
            TurnOnEMF();
        }
    }
    //EMF deactivatie
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && EMF == true)
        {
            Debug.Log("Exited " + gameObject.name);
            TurnOffEMF();
        }
    }
    void TurnOnEMF()
    {
        foreach (GameObject emfLight in EMFLights)
        {
            if (emfLight != null)
            {
                emfLight.SetActive(true);
                Debug.Log("EMF light turned on in " + gameObject.name);
                sfx.Play();

            }
        }
    }
    void TurnOffEMF()
    {
        foreach (GameObject emfLight in EMFLights)
        {
            if (emfLight != null)
            {
                emfLight.SetActive(false);
                Debug.Log("EMF light turned off in " + gameObject.name);
                sfx.Stop();
            }
        }
    }
    #endregion
}
