using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackPower : MonoBehaviour {
    //Set attackBoostth amount to whatever makes sense
    public int attackAmount = 10;
    public float sliceSpeed = 1f;
	public float cooldown = 5f;
	public GameObject cooldownMask;
	float lastUseTime;
	float cooldownMaskHeight = 400f;
    bool cooling = false, targeting = false;
    public GameObject targetPrefab, target;
    public GameObject slicePrefab;
    public int numPizza = 12;
    public SphereCollider coll;
	// Update is called once per frame
	void Update ()
    {
		// update cooldown
		if (!cooling) {
			if (targeting && !Input.GetMouseButtonDown (0)) {
				Vector3 mousePos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0f));
				mousePos.z = 0;
				target.transform.position = mousePos;
			} else if (targeting) {
				targeting = false;
				Node.interactible = true;
				cooling = true;
				lastUseTime = Time.time;
                List<Collider> units = new List<Collider>();
				Collider[] toattackBoost = Physics.OverlapSphere (target.transform.position, coll.radius);
				foreach (Collider attackBoost in toattackBoost) {
					if (attackBoost.gameObject.tag == "Unit")
                    {
						attackBoost.SendMessage ("AttackBoost", attackAmount);
                        units.Add(attackBoost);
					}
				}
				UI.S.PlaySound ("AttackBoost");
				GameObject[] sliceDest = new GameObject[numPizza];
				for (int i = 0; i < numPizza; ++i) {
					GameObject slice = Instantiate (slicePrefab, target.transform.position, Quaternion.Euler (new Vector3 (0f, 0f, 360f - (i * 360f / numPizza)))) as GameObject;
					sliceDest [i] = slice;
					slice.GetComponent<Rigidbody> ().velocity = slice.transform.TransformDirection (Vector3.up) * sliceSpeed;
				}
				Destroy (target);
				StartCoroutine (sliceAnimate(sliceDest, units));
			}
		} else {
			float u = (Time.time - lastUseTime) / cooldown;
			if (u > 1) {
				u = 1;
			}
			cooldownMask.GetComponent<RectTransform>().sizeDelta = new Vector2(cooldownMask.GetComponent<RectTransform>().sizeDelta.x, (1-u) * cooldownMaskHeight);
			if (Time.time > lastUseTime + cooldown) {
				cooling = false;
			}
		}
    }
    IEnumerator sliceAnimate(GameObject[] sliceDest, List<Collider> units)
    {
        foreach (GameObject toDest in sliceDest)
        {
            Destroy(toDest, 2f);
        }
        yield return new WaitForSeconds(5f);     
        foreach(Collider unit in units)
        {
            if(unit != null)
            {
                unit.SendMessage("UndoAttackBoost", attackAmount);
            }
        }  
    }
    public void clicked()
    {
        if(!targeting && !cooling)
        {
            targeting = true;
			Node.interactible = false;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            mousePos.z = 0;
            target = Instantiate(targetPrefab, mousePos, Quaternion.identity) as GameObject;
            coll = target.GetComponent<SphereCollider>();
        }
    }

}
