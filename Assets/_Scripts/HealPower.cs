using UnityEngine;
using System.Collections;

public class HealPower : MonoBehaviour {
    //Set health amount to whatever makes sense
    public int healAmount = 50;
    public float lolliSpeed = 1f;
	public float cooldown = 8f;
	public GameObject cooldownMask;
	float lastUseTime;
	float cooldownMaskHeight = 400f;
    bool cooling = false, targeting = false;
    public GameObject targetPrefab, target;
    public GameObject lollipopPrefab;
    public int numLollipops = 12;
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
				Collider[] toHeal = Physics.OverlapSphere (target.transform.position, coll.radius);
				foreach (Collider heal in toHeal) {
					if (heal.gameObject.tag == "Unit") {
						heal.SendMessage ("Heal", healAmount);
					}
				}
				UI.S.PlaySound ("Heal");
				GameObject[] lolliDest = new GameObject[numLollipops];
				for (int i = 0; i < numLollipops; ++i) {
					GameObject lolli = Instantiate (lollipopPrefab, target.transform.position, Quaternion.Euler (new Vector3 (0f, 0f, 360f - (i * 360f / numLollipops)))) as GameObject;
					lolliDest [i] = lolli;
					lolli.GetComponent<Rigidbody> ().velocity = lolli.transform.TransformDirection (Vector3.up) * lolliSpeed;
				}
				Destroy (target);
				StartCoroutine (lolliAnimate (lolliDest));
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
    IEnumerator lolliAnimate(GameObject[] lolliDest)
    {
        foreach (GameObject toDest in lolliDest)
        {
            Destroy(toDest, 2);
        }
        yield return new WaitForSeconds(2f);
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
