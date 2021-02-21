using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMovement : MonoBehaviour
{
    public Transform target;
    public Transform missile;
    public CharacterController controller;

    public float rotation_speed;
    public float max_speed;
    public float min_speed;
    public float speed_change_rate;
    float speed;

    float sqr_dst_old;
    float speed_multiplier;

    public float imprecision_scale;
    public float imprecision_magnitude;

    void Start () {
        speed = max_speed;
    }

    // Update is called once per frame
    void Update()
    {
        float current_rotation = -missile.rotation.eulerAngles.y + 90;
        current_rotation = Normalize(current_rotation);

        float zdistance = target.position.z - missile.position.z;
        float xdistance = target.position.x - missile.position.x;
        float desired_rotation = Mathf.Rad2Deg * Mathf.Atan(zdistance / xdistance);
        
        if (xdistance < 0 && zdistance < 0) {
            desired_rotation += 180;
        } else if (xdistance < 0 && zdistance > 0) {
            desired_rotation += 180;
        }

        desired_rotation = Normalize(desired_rotation);

        float final_rotation = DetermineFinalRotation (desired_rotation, current_rotation);
        missile.rotation = Quaternion.Euler(new Vector3(0, -final_rotation + 90, 0));

        Vector3 heading = target.position - missile.position;
        float sqr_dst = heading.magnitude * heading.magnitude;

        if (sqr_dst > sqr_dst_old) {
            speed -= speed_change_rate * Time.deltaTime;
            speed = Mathf.Max(speed, min_speed);
        } else {
            speed += speed_change_rate * Time.deltaTime;
            speed = Mathf.Min(speed, max_speed);
        }
        
        controller.Move(speed * Time.deltaTime * transform.forward);
    }

    float DetermineFinalRotation (float desired_rotation, float current_rotation) {
        
        if (Normalize(Mathf.Abs(Mathf.DeltaAngle(current_rotation, desired_rotation))) < rotation_speed * Time.deltaTime) {
            return desired_rotation;
        } else if (Normalize(Mathf.Abs(Mathf.DeltaAngle(current_rotation + rotation_speed * Time.deltaTime, desired_rotation))) < Normalize(Mathf.Abs(Mathf.DeltaAngle(current_rotation - rotation_speed * Time.deltaTime, desired_rotation)))) {
            current_rotation += rotation_speed * Time.deltaTime;
        } else {
            current_rotation -= rotation_speed * Time.deltaTime;
        }

        return current_rotation;
    }

    float Normalize (float angle) {
        if (angle < 0) {
            angle += 360;
        } else if (angle >= 360) {
            angle -= 360;
        }
        return angle;
    }
}