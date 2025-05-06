using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // L'objet à suivre
    public float initialHeight;

    private Quaternion initialRotation;

    void Start()
    {
        // Enregistrer la rotation initiale de la caméra
        initialRotation = transform.rotation;
        
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Suivre la position de l'objet tout en gardant la hauteur et la rotation initiale
            transform.position = new Vector3(target.position.x, initialHeight, target.position.z);
            transform.rotation = initialRotation; // Garde la rotation initiale sans la modifier
        }
    }
}