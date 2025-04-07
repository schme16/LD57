using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

	public bool canBeHarmed = true;
	private int healthPoints = 3;
	public int maxHealth = 3;

	public UnityEvent OnDamage;
	public UnityEvent OnBelowHalfHealth;
	public UnityEvent OnNoHealth;
	public StudioEventEmitter breakAudio;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Start() {
		
		OnDamage.AddListener(() => {
			
			if (healthPoints < maxHealth / 2) {
				OnBelowHalfHealth.Invoke();
			}
			if (healthPoints <= 0) {
				OnNoHealth.Invoke();
			}
		});
		
		OnBelowHalfHealth.AddListener(() => {
			
		});
		
		OnNoHealth.AddListener(() => {
			
			//TODO: spawn effect?
			_.PlayAudio(breakAudio);
			Destroy(gameObject);
		});
		
		
		healthPoints = maxHealth;
	}
	
	public void TakeDamage(int damage) {
		healthPoints -= damage;
		OnDamage.Invoke();
	}
}
