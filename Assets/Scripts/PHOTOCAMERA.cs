using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PHOTOCAMERA : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera playerCamera; 
    public int money = 0; 
    public Text moneyText;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && playerCamera.enabled)
        {
            TakePhoto();
            UpdateMoneyText();
        }
    }
    void IncreaseMoney(int IncreaseAmount)
    {
        money += IncreaseAmount;
        Debug.Log("Money increased to " + money);
    }

    void TakePhoto()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Photo"))
            {
                IncreaseMoney(5);
            }
            if (hit.collider.CompareTag("Ghost"))
            {
                IncreaseMoney(40);
                Debug.Log(money);
            }
        }
    }

   
    void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: $" + money;
        }
    }
}
