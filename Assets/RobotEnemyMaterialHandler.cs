using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotEnemyMaterialHandler : MonoBehaviour {

	[SerializeField] Material _material;

	[SerializeField] SkinnedMeshRenderer[] _robotSkinnedMeshRenderers;
	Material[] _robotMaterials;

	int _outlineColor;
	int _outlineWidth;
	int _outlineNormalExtrusion;
	float _outlineWidthValue = 1.033f;
	float _outlineNormalExtrusionValue = 0.038f;


	void Awake(){
		// Make a copy of the material to have a unique copy for this robot
		_material = new Material (_material);
	}

	void Start(){
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

	void OnMouseEnter(){
		_material.SetFloat (_outlineWidth, _outlineWidthValue);
		_material.SetFloat (_outlineNormalExtrusion, _outlineNormalExtrusionValue);
	}

	void OnMouseExit(){
		_material.SetFloat (_outlineWidth, 0.0f);
		_material.SetFloat (_outlineNormalExtrusion, 0.0f);
	}
}
