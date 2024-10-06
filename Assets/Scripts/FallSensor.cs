using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSensor : MonoBehaviour  // Lea's
{
    public List<GameObject> zombieStartpos; // start position
    public List<GameObject> zombies;
    public GameManager gm;  // reference GameManager list

    // Start is called before the first frame update
    void Start()
    {
        
    }


    //Trgger Function 

    private void OnTriggerEnter(Collider other)  
    {
        gm.decreaseLives(); // lose a life when get to below function

        if (other.gameObject.tag == "YellowTag")   // if an obj is a yellow tag
        {
            zombies[0].transform.position = zombieStartpos[0].transform.position; // if a zombie is at 0 then change the position to the start position
            Debug.Log("Yellow zombie tagged");
        }

        else if (other.gameObject.tag == "BlueTag")
        {
            zombies[1].transform.position = zombieStartpos[1].transform.position;
            Debug.Log("Blue zombie tagged");
        }

        else if (other.gameObject.tag == "GreenTag")
        {
            zombies[2].transform.position = zombieStartpos[2].transform.position;
            Debug.Log("Green zombie tagged");
        }

        else if (other.gameObject.tag == "RedTag")
        {
            zombies[3].transform.position = zombieStartpos[3].transform.position;
            Debug.Log("Red zombie tagged");
        }

        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
