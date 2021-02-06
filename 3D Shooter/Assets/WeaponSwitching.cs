using UnityEngine;

public class WeaponSwitching : MonoBehaviour {

    private int selectedWeapon = 0;
    [SerializeField] private PlayerStatus status;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int prevSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKeyDown(KeyCode.Q)) {
            if (selectedWeapon >= transform.childCount - 1) {
                selectedWeapon = 0;
            } else {
                selectedWeapon++;
            }
        } 
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKeyDown(KeyCode.E)) {
            if (selectedWeapon <= 0) {
                selectedWeapon = transform.childCount - 1;
            } else {
                selectedWeapon--;
            }
        }

        if (selectedWeapon != prevSelectedWeapon) {
            SelectWeapon();
        }
    }

    void SelectWeapon() {
        int i = 0;

        foreach (Transform weapon in transform) {
            if (i == selectedWeapon) {
                var gunScript = weapon.GetComponent<Gun>();
                var childCount = weapon.childCount;

                for (int j = 0; j < childCount; j++) {
                    var renderer = weapon.GetChild(j).GetComponent<MeshRenderer>();
                    if (renderer != null) {
                        renderer.enabled = true;
                    }
                }
                status.UpdateGunIndex(i);

            } else {
                var childCount = weapon.childCount;

                for (int j = 0; j < childCount; j++) {
                    var renderer = weapon.GetChild(j).GetComponent<MeshRenderer>();
                    if (renderer != null) {
                        renderer.enabled = false;
                    }
                }
            }

            i++;
        }
    }
}
