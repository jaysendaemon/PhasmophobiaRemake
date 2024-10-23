using System.Collections.Generic;
using UnityEngine;

public class inventory1 : MonoBehaviour
{
    public List<GameObject> inventoryItems = new List<GameObject>(); // List to store inventory items

    public int selectedItemIndex = -1;
    public GameObject[] itemSets; // Array of item sets (EMF_X)
    public GameObject[] specifiedWeapons; // Array of specified weapons
    public float throwForce = 10f;
    public GameObject camera;
    private List<Transform> unparentedItems = new List<Transform>(); // List of unparented items
    public float detectionRadius = 5f;
    public LayerMask detectionLayer; // Layer mask for player or any object to detect
    public bool playerInRange = false;
    public bool flashlighton = false;
    public bool haspickedupitems = false;
    public AudioSource flashlightclick;
    private GameObject player;
    public GameObject flashlight;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SelectItem();
    }

    void Update()
    {
        DetectPlayerInRange();

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("StartingPickUpItems") && Input.GetKeyDown(KeyCode.E))
            {
                haspickedupitems = true;
                collider.gameObject.SetActive(false);
                inventoryItems.Add(collider.gameObject); 
            }
        }

        if (Input.GetKeyDown(KeyCode.T) && flashlighton == false && selectedItemIndex == 1)
        {
            flashlight.gameObject.SetActive(true);
            flashlightclick.Play();
            flashlighton = true;
        }
        else if (Input.GetKeyDown(KeyCode.T) && flashlighton == true && selectedItemIndex == 1)
        {
            flashlight.gameObject.SetActive(false);
            flashlightclick.Play();
            flashlighton = false;
        }

        if (Input.GetKeyDown(KeyCode.Q) && haspickedupitems == true)
        {
            SwitchItem();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }

        if (Input.GetKeyDown(KeyCode.E) && playerInRange == true)
        {
            PickupItem();
            haspickedupitems = true;
        }
    }

    void SwitchItem()
    {
        if (inventoryItems.Count == 0) return; 

        selectedItemIndex++;
        if (selectedItemIndex >= inventoryItems.Count)
        {
            selectedItemIndex = 0;
        }
        SelectItem();
    }

    void SelectItem()
    {
        for (int i = 0; i < itemSets.Length; i++)
        {
            bool shouldBeActive = i == selectedItemIndex || unparentedItems.Contains(itemSets[i].transform);
            ActivateItemSet(itemSets[i].transform, shouldBeActive);
        }
    }

    void ActivateItemSet(Transform itemSet, bool activate)
    {
        itemSet.gameObject.SetActive(activate);

        bool isUnparented = unparentedItems.Contains(itemSet);

        foreach (Transform child in itemSet)
        {
            if (child.CompareTag("Arms"))
            {
                child.gameObject.SetActive(activate && !isUnparented);
            }
            else if (ArrayContains(specifiedWeapons, child.gameObject))
            {
                child.gameObject.SetActive(activate);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    bool ArrayContains(GameObject[] array, GameObject obj)
    {
        foreach (GameObject item in array)
        {
            if (item == obj)
            {
                return true;
            }
        }
        return false;
    }

    #region DetectPlayerInRange
    void DetectPlayerInRange()
    {
        if (player == null)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        playerInRange = false;
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Throwable"))
            {
                Transform parentTransform = collider.transform.parent;
                if (parentTransform != null && parentTransform.parent == null)
                {
                    playerInRange = true;
                    break;
                }
            }
        }

    }
    #endregion

    #region ItemDrop

    void DropItem()
    {
        if (selectedItemIndex < 0 || selectedItemIndex >= itemSets.Length) return;

        GameObject itemSetToDrop = itemSets[selectedItemIndex];
        if (!unparentedItems.Contains(itemSetToDrop.transform))
        {
            unparentedItems.Add(itemSetToDrop.transform);
            ActivateItemSet(itemSetToDrop.transform, false);
            inventoryItems.Remove(itemSetToDrop.gameObject);
            itemSetToDrop.transform.SetParent(null, true);
            Rigidbody[] itemRigidbodies = itemSetToDrop.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rb in itemRigidbodies)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.AddForce(camera.transform.forward * throwForce, ForceMode.Impulse);
            }

            WeaponSway weaponSwayScript = itemSetToDrop.GetComponentInChildren<WeaponSway>();
            if (weaponSwayScript != null)
            {
                weaponSwayScript.enabled = false;
            }

            SelectItem();
        }
    }
    #endregion

    public Vector3 pickupPosition = new Vector3(-1.085f, 3.573f, -0.638f);

    #region ItemPickup
    void PickupItem()
    {
        if (unparentedItems.Count > 0)
        {
            Transform itemSetToPickup = unparentedItems[0];
            WeaponSway weaponSwayScript = itemSetToPickup.GetComponentInChildren<WeaponSway>();

            foreach (Transform child in itemSetToPickup)
            {
                if (ArrayContains(specifiedWeapons, child.gameObject))
                {
                    child.gameObject.SetActive(true);
                }
            }

            Rigidbody[] itemRigidbodies = itemSetToPickup.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in itemRigidbodies)
            {
                rb.isKinematic = true; 
            }

            itemSetToPickup.SetParent(transform, true);

            itemSetToPickup.localPosition = pickupPosition;
            itemSetToPickup.localRotation = Quaternion.identity;

            unparentedItems.Remove(itemSetToPickup);
            inventoryItems.Remove(itemSetToPickup.gameObject); // Add the item back to inventory

            if (weaponSwayScript != null)
            {
                weaponSwayScript.enabled = true;
            }

            SelectItem();
        }
    }
    #endregion
}
