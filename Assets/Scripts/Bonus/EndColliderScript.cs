using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndColliderScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BonusShoot")
        {
            Destroy(other.gameObject);
        }

        if (other.tag == "HolderBonus")
        {
            Destroy(other.gameObject);
        }

        if (other.tag == "Bonus")
        {
            Destroy(other.gameObject);
        }
    }
}
