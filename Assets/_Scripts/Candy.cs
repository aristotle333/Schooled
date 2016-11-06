using UnityEngine;
using System.Collections;

public class Candy : MonoBehaviour {
    public int value;
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        transform.Rotate(0f, 2f, 0f);
	}

    void OnTriggerEnter(Collider coll)
    {
        if (!coll.CompareTag("Unit"))
            return;
        
        UI.S.PlaySound("CandyCollect");
        ResourceManager.S.AddResources(value);
        HitText.CreateYellow(this.transform.position, value);
        Destroy(gameObject);
    }
}
