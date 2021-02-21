using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int rotation_speed;
    public int speed;
    public CharacterController controller;

    Vector3 rotation = new Vector3(0,0,0);

    // Update is called once per frame
    void Update()
    {
        rotation -= new Vector3(0, rotation_speed * -Input.GetAxis("Horizontal") * Time.deltaTime, 0);
        transform.rotation = Quaternion.Euler(rotation);
        controller.Move(Input.GetAxis("Vertical") * speed * Time.deltaTime * transform.forward);   
    }
}
