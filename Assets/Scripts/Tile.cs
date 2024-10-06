using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour //Lea's
{
    public GameManager gm;
    public GameObject collidobj;
    public ParticleSystem hitEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    //Collision with tile
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == collidobj.name)
        {
            Debug.Log(this.gameObject);
            Debug.Log(" has collided with the tile");
            gm.decreaseScore();

            // Instantiate the hit effect at a custom position and destroy it after 2 seconds
            ParticleSystem hitEffectInstance = Instantiate(hitEffectPrefab, transform.position + Vector3.down, Quaternion.identity);
            Destroy(hitEffectInstance.gameObject, 2f);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
