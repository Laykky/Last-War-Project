using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLaucher : MonoBehaviour
{
    [SerializeField] private Transform launchPoint;
    [SerializeField] private GameObject canonProjectile;
    [SerializeField] private float impulseForce = 20f;
    [SerializeField] private float velocityFactor = 1.5f;
    [SerializeField] private float startDelay = 2f;
    [SerializeField] private float vfxplayingdelay = 1f;
    [SerializeField] private float fireRate = 3f;

    [SerializeField] private float rotationAngle = 20f;
    [SerializeField] private float rotationTime = 10f;
    [SerializeField] private float rotationSpeed = 2f;

    private Quaternion initialRotation;
    private Quaternion leftRotation;
    private Quaternion rightRotation;

    [SerializeField] private ParticleSystem warningVFX;

    private void Start()
    {
        StartCoroutine(ShootProjectiles()); // 🔹 Lance la routine de tir
        StartCoroutine(RotationLoop()); // 🔹 Lance la routine de rotation après un délai
    }

    private IEnumerator ShootProjectiles()
    { 

        yield return new WaitForSeconds(startDelay); // 🔹 Attente initiale avant le premier tir

        while (true)
        {
            if (warningVFX != null)
                warningVFX.Play();
            yield return new WaitForSeconds(vfxplayingdelay);
            GameObject _projectile = Instantiate(canonProjectile, launchPoint.position, launchPoint.rotation);
            Rigidbody rb = _projectile.GetComponent<Rigidbody>();

            Vector3 shootDirection = launchPoint.forward + (Vector3.up * 0.5f);
            rb.AddForce(shootDirection * impulseForce * velocityFactor, ForceMode.Impulse);

            Destroy(_projectile, 5f);

            yield return new WaitForSeconds(fireRate);
        }
    }

    private IEnumerator RotationLoop()
    {
        yield return new WaitForSeconds(startDelay); // 🔹 Attente initiale avant de commencer la rotation

        initialRotation = transform.rotation;
        leftRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - rotationAngle, transform.eulerAngles.z);
        rightRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + rotationAngle, transform.eulerAngles.z);

        while (true)
        {
            yield return new WaitForSeconds(rotationTime);
            yield return StartCoroutine(RotateSmoothly(initialRotation, leftRotation));

            yield return new WaitForSeconds(rotationTime);
            yield return StartCoroutine(RotateSmoothly(leftRotation, initialRotation));

            yield return new WaitForSeconds(rotationTime);
            yield return StartCoroutine(RotateSmoothly(initialRotation, rightRotation));

            yield return new WaitForSeconds(rotationTime);
            yield return StartCoroutine(RotateSmoothly(rightRotation, initialRotation));
        }
    }

    private IEnumerator RotateSmoothly(Quaternion fromRotation, Quaternion toRotation)
    {
        float elapsedTime = 0f;
        float duration = rotationSpeed;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(fromRotation, toRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = toRotation;
    }
}

