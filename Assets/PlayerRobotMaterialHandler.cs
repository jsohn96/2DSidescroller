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

	void Start(){
		//Get Property ID of the properties that will be edited
		_outlineColor = Shader.PropertyToID ("_OutlineColor");
		_outlineWidth = Shader.PropertyToID ("_OutlineWidth");
		_outlineNormalExtrusion = Shader.PropertyToID ("_OutlineNormalExtrusion");

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
		for (int i = 0; i < _outlineMaterialsLength; i++) {
			_outlineMaterials[i].SetFloat (_outlineWidth, _outlineWidthValue);
			_outlineMaterials[i].SetFloat (_outlineNormalExtrusion, _outlineNormalExtrusionValue);
		}
	}

	void OnMouseExit(){
		TurnOffOutline ();
	}

	void TurnOffOutline(){
		for (int i = 0; i < _outlineMaterialsLength; i++) {
			_outlineMaterials[i].SetFloat (_outlineWidth, 0.0f);
			_outlineMaterials[i].SetFloat (_outlineNormalExtrusion, 0.0f);
		}
	}

	//TODO: Change Color of the outline based on proximity to enemy?
	// May be better to have this on the enemy
}
