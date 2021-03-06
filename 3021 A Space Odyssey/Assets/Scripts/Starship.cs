using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Starship : MonoBehaviour {

    [Header("Main settings")]
    public static float health = 100;
    public static float fuel = 100;
    public static float fuelUsage = 0.88f;
    public static float oxygen = 100;
    public static float oxygenUsage = 0.6f;

    public static float distance = 0;

    [Header("Movement")]
    public float accelerationForce = 6f;
    public float turboForce = 30f;
    public float rotationSpeed = 3f;
    public float maxSpeed = 10f;

    public GameObject model;

    public ParticleSystem engine, turbo, damageLight, damageMedium, damageCritical, explosion;

    public AudioSource engineActive, turboActive, explosionAudio;
    private float maxEngineVolume = 0.011f;

    private static Rigidbody rbody;

    public static void init() {
        health = 100;
        oxygen = 100;
        fuel = 100;
    }

    void Start() {
        init();
        rbody = this.GetComponent<Rigidbody>();
        turbo.Stop();
        engine.Stop();
        damageCritical.Stop();
        damageMedium.Stop();
        damageLight.Stop();
    }

    void Update() {
        // if (GameStateManager.isInit()) {
        //     init();
        // }
        if (GameStateManager.isInGame()) {
            // Controls
            float rotation = Input.GetAxis("Horizontal");
            float acceleration = Input.GetAxis("Vertical");
            StarshipController(rotation, acceleration);
            UpdateDistance();

            oxygen -= oxygenUsage * Time.deltaTime;

            if (health < 100) {
                health += 0.5f * Time.deltaTime;
            }

            if (health > 100) {
                health = 100;
            }
            if (fuel > 100) {
                fuel = 100;
            }

            if (oxygen > 100) {
                oxygen = 100;
            }

            if (oxygen <= 0) {
                health = -0.1f;
            }

            if (fuel <= 0) {
                fuel = 0;
                engineActive.volume = 0f;
                turboActive.volume = 0f;
            }

            if (health <= 70 && health >= 40) {
                damageLight.Play();
            } else {
                damageLight.Stop();
            }

            if (health < 40 && health >= 20) {
                damageMedium.Play();
            } else {
                damageMedium.Stop();
            }

            if (health < 20) {
                damageCritical.Play();
            } else {
                damageCritical.Stop();
            }
        }
        if (health <= 0) {
            health = 0;
        }

        if (health <= 0 && !GameStateManager.isGameover() && model.gameObject.activeSelf) {
            model.SetActive(false);
            turbo.Stop();
            turboActive.volume = 0f;
            engineActive.volume = 0f;
            engine.Stop();
            damageCritical.Stop();
            explosion.Play();
            explosionAudio.Play();
            GameStateManager.Gameover();

            //Explode
            Debug.Log("BOOOOOM!");
        }
        // }
    }


    private void UpdateDistance() {
        distance = (Vector3.zero - transform.position).magnitude / 6000;
    }

    private void StarshipController(float rotation, float acceleration) {

        if (GameStateManager.canStarShipMove()) {
            transform.Rotate(0, 0, -rotation * rotationSpeed * Time.deltaTime);
            if (fuel > 0) {

                if (Input.GetAxis("Vertical") != 0 && !Input.GetButton("Turbo")) {
                    engine.Play();
                    engineActive.volume = maxEngineVolume;
                    fuel -= fuelUsage * Time.deltaTime;
                } else {
                    engine.Stop();
                    engineActive.volume = 0f;
                }
                // Turbo?
                if (Input.GetButton("Turbo") && Input.GetAxis("Vertical") != 0) {
                    fuel -= 2.5f * fuelUsage * Time.deltaTime;
                    turbo.Play();
                    turboActive.volume = maxEngineVolume;
                    rbody.AddForce(transform.up * turboForce * acceleration);
                } else {
                    turbo.Stop();
                    turboActive.volume = 0f;
                    rbody.AddForce(transform.up * accelerationForce * acceleration);
                }
            } else {
                turbo.Stop();
                engine.Stop();
            }
            rbody.velocity = new Vector2(Mathf.Clamp(rbody.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rbody.velocity.y, -maxSpeed, maxSpeed));
        } else {
            rbody.velocity = Vector2.Lerp(rbody.velocity, new Vector2(0, 0), Time.deltaTime * 2); //breaks

            //No rotation
            // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime);
            turbo.Stop();
            engine.Stop();
            turboActive.volume = 0f;
            engineActive.volume = 0f;
        }
    }

    public static void ApplyDamage(float damage) {
        health -= damage;
    }

    public static float getVelocity() {
        return rbody.velocity.magnitude;
    }

    // Force freeze rotation on X e Y when bouncing
    void FixedUpdate() {
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
    }
}
