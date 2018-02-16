using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRobotMaterialHandler : MonoBehaviour {
	// pre-assigned reference to outline materials
	[SerializeField] SkinnedMeshRenderer[] _skinnedMeshRenderer;
	Material[] _outlineMaterials;
	int _outlineMaterialsLength = 0;

	int _outlineColor;
	int _outlineWidth;
	int _outlineNormalExtrusion;
	float _outlineWidthValue = 1.033f;
	float _outlineNormalExtrusionValue = 0.038f;
	float _maxDamageExtrustionValue = 2.0f;

	bool _isTakingDamage = false;
	// animation curve to push character back upon impact with enemy
	[SerializeField] AnimationCurve _damageImpactCurve;
	Color _originColor = new Color (0f, 1f, 0f, 1f);
	Color _damagedColor = new Color(1.0f, 0f, 0f, 1f);

	void Start(){
		//Get Property ID of the properties that will be edited
		_outlineColor = Shader.PropertyToID ("_OutlineColor");
		_outlineWidth = Shader.PropertyToID ("_OutlineWidth");
		_outlineNormalExtrusion = Shader.PropertyToID ("_OutlineNormalExtrusion");

		//get reference to the characters materials for toggling outline/damage
		int skinnedMeshLength = _skinnedMeshRenderer.Length;
		for (int i = 0; i < skinnedMeshLength; i++) {
			_outlineMaterialsLength += _skinnedMeshRenderer [i].materials.Length;
		}
		_outlineMaterials = new Material[_outlineMaterialsLength];

		int outlineMaterialIndex = 0;
		for (int i = 0; i < skinnedMeshLength; i++) {
			int materialLength = _skinnedMeshRenderer [i].materials.Length;
			for (int j = 0; j < materialLength; j++) {
				_outlineMaterials[outlineMaterialIndex] = _skinnedMeshRenderer [i].materials [j];
				outlineMaterialIndex++;
			}
		}
		TurnOffOutline ();
	}

	void OnMouseEnter(){
		// Toggle outline on
		if (!_isTakingDamage) {
			for (int i = 0; i < _outlineMaterialsLength; i++) {
				_outlineMaterials [i].SetFloat (_outlineWidth, _outlineWidthValue);
				_outlineMaterials [i].SetFloat (_outlineNormalExtrusion, _outlineNormalExtrusionValue);
			}
		}
	}

	void OnMouseExit(){
		TurnOffOutline ();
	}

	void TurnOffOutline(){
		if (!_isTakingDamage) {
			for (int i = 0; i < _outlineMaterialsLength; i++) {
				_outlineMaterials [i].SetFloat (_outlineWidth, 0.0f);
				_outlineMaterials [i].SetFloat (_outlineNormalExtrusion, 0.0f);
			}
		}
	}

	// Increments Damage counter and display damage animation
	public void TakeDamage(){
		TurnOffOutline ();
		_isTakingDamage = true;
		GameManager._gameManagerInstance.DamageCnt++;
		StartCoroutine (TakingDamage ());
	}

	//Play the damaging animation through outline shader extrusion
	IEnumerator TakingDamage(){
		for (int i = 0; i < _outlineMaterialsLength; i++) {
			_outlineMaterials [i].SetColor (_outlineColor, _damagedColor);
		}
		yield return new WaitForSeconds (0.8f);
		AudioManager._audioManagerInstance.PlayDeath ();
		float timer = 0f;
		float duration = 0.8f;
		float impactValue;
		while (timer < duration){
			timer += Time.deltaTime;
			impactValue = Mathf.Lerp (0.0f, _maxDamageExtrustionValue, _damageImpactCurve.Evaluate(timer / duration));
			for (int i = 0; i < _outlineMaterialsLength; i++) {
				_outlineMaterials [i].SetFloat (_outlineNormalExtrusion, impactValue);
			}
			yield return null;		
		}
		for (int i = 0; i < _outlineMaterialsLength; i++) {
			_outlineMaterials [i].SetFloat (_outlineNormalExtrusion, 0.0f);
		}
		yield return null;
		for (int i = 0; i < _outlineMaterialsLength; i++) {
			_outlineMaterials [i].SetColor (_outlineColor, _originColor);
		}
		_isTakingDamage = false;
	}
}
