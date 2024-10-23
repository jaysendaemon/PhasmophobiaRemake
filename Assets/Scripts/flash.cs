using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flash : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject flashlightlight;
    [SerializeField] public GameObject flashlightobject;
    [SerializeField] public AudioSource sfx;

    public bool on = true;
    public bool off = false;
    void Start()
    {
        flashlightlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       if(off && Input.GetKeyDown(KeyCode.F))
        {
            flashlightlight.SetActive(true);
            sfx.Play();
            off = false;
            on = true;
        }else if(on && Input.GetKeyDown(KeyCode.F))
        {
            flashlightlight.SetActive(false);
            sfx.Play();
            off = true;
            on = false ;
        }
    }
}
