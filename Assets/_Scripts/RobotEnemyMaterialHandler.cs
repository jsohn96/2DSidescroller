using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles Material realted processed on enemy robots
public class RobotEnemyMaterialHandler : MonoBehaviour {

	[SerializeField] Material _material;

	[SerializeField] SkinnedMeshRenderer[] _robotSkinnedMeshRenderers;
	Material[] _robotMaterials;

	int _outlineColor;
	int _outlineWidth;
	int _outlineNormalExtrusion;
	float _outlineWidthValue = 1.033f;
	float _outlineNormalExtrusionValue = 0.038f;

	float _maxDamageExtrusionValue = 2.0f;
	bool _isBeingDestroyed = false;


	void Awake(){
		// Make a copy of the material to have a unique copy for this robot
		_material = new Material (_material);
	}

	void Start(){
		// swap enemy robot materials to a predefined enemy material
		int meshRenderersLength = _robotSkinnedMeshRenderers.Length;
		for (int i = 0; i < meshRenderersLength; i++) {
			_robotMaterials = _robotSkinnedMeshRenderers [i].materials;
			int robotMaterialsLength = _robotMaterials.Length;
			for (int j = 0; j < robotMaterialsLength; j++) {
				_robotMaterials [j] = _material;
			}
			_robotSkinnedMeshRenderers [i].materials = _robotMaterials;
		}

		//Get Property ID of the properties that will be edited
		_outlineColor = Shader.PropertyToID ("_OutlineColor");
		_outlineWidth = Shader.PropertyToID ("_OutlineWidth");
		_outlineNormalExtrusion = Shader.PropertyToID ("_OutlineNormalExtrusion");
	}

	//Toggle outline on mouse enter
	void OnMouseEnter(){
		if (!_isBeingDestroyed) {
			_material.SetFloat (_outlineWidth, _outlineWidthValue);
			_material.SetFloat (_outlineNormalExtrusion, _outlineNormalExtrusionValue);
		}
	}

	//Toggle outline off on mouse exit
	void OnMouseExit(){
		if (!_isBeingDestroyed) {
			_material.SetFloat (_outlineWidth, 0.0f);
			_material.SetFloat (_outlineNormalExtrusion, 0.0f);
		}
	}

	// Display the enemy destruction animation
	public IEnumerator DestroyEnemy() {
		yield return new WaitForSeconds (1.4f);
		AudioManager._audioManagerInstance.PlayDeath ();
		float timer = 0f;
		float duration = 0.5f;
		while (timer < duration) {
			timer += Time.deltaTime;
			_material.SetFloat(_outlineNormalExtrusion, Mathf.Lerp (0.0f, _maxDamageExtrusionValue, timer / duration));
			yield return null;
		}
		gameObject.SetActive (false);
		yield return null;
	}
}
