using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineCam;
    private CinemachineBasicMultiChannelPerlin noise;
    private float shakeDuration = 0.5f; //  Durée totale du shake
    private float shakeIntensity = 2f; //  Intensité du tremblement

    private void Awake()
    {
        cinemachineCam = GetComponent<CinemachineVirtualCamera>();
        noise = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (noise != null)
        {
            noise.m_AmplitudeGain = 0; // Désactive le shake au démarrage
        }
        else
        {
            Debug.LogError("Aucun composant `CinemachineBasicMultiChannelPerlin` trouvé !");
        }
    }

    public void ShakeCamera()
    {
        if (noise != null)
        {
            StartCoroutine(ShakeCoroutine());
        }
    }

    private IEnumerator ShakeCoroutine()
    {
        Debug.Log("Tremblement activé !");
        noise.m_AmplitudeGain = shakeIntensity; //Active le shake

        yield return new WaitForSeconds(shakeDuration); //Attend quelques millisecondes

        noise.m_AmplitudeGain = 0; //  Désactive le shake après le délai
        Debug.Log("Tremblement terminé !");
    }
}

