using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomShaderGUI : ShaderGUI {

//	MaterialProperty _MainTex = ShaderGUI.FindProperty("_MainTex", properties);

	public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] properties){

		//		base.OnGUI (materialEditor, properties);

		MaterialProperty useMainTexture = ShaderGUI.FindProperty ("_UseMainTexture", properties);
		if (useMainTexture != null) {
			materialEditor.ShaderProperty (useMainTexture, useMainTexture.displayName);

			if (useMainTexture.floatValue == 1) {
				MaterialProperty mainTex = ShaderGUI.FindProperty ("_MainTex", properties);
				materialEditor.ShaderProperty (mainTex, mainTex.displayName);
			}
		}

		MaterialProperty useColor = ShaderGUI.FindProperty ("_UseColor", properties);
		if (useColor != null) {
			materialEditor.ShaderProperty (useColor, useColor.displayName);
			if (useColor.floatValue == 1) {
				MaterialProperty color = ShaderGUI.FindProperty ("_Color", properties);
				materialEditor.ShaderProperty (color, color.displayName);
			}
		}

		MaterialProperty useOutlineColor = ShaderGUI.FindProperty ("_UseOutlineColor", properties);
		if (useOutlineColor != null) {	
			materialEditor.ShaderProperty (useOutlineColor, useOutlineColor.displayName);
			if (useOutlineColor.floatValue == 1) {
				MaterialProperty outlineColor = ShaderGUI.FindProperty ("_OutlineColor", properties);
				materialEditor.ShaderProperty (outlineColor, outlineColor.displayName);
			}
		}

		MaterialProperty useOutlineWidth = ShaderGUI.FindProperty ("_UseOutlineWidth", properties);
		if (useOutlineWidth != null) {	
			materialEditor.ShaderProperty (useOutlineWidth, useOutlineWidth.displayName);
			if (useOutlineWidth.floatValue == 1) {
				MaterialProperty outlineWidth = ShaderGUI.FindProperty ("_OutlineWidth", properties);
				materialEditor.ShaderProperty (outlineWidth, outlineWidth.displayName);
			}
		}
			
		MaterialProperty useNormalExtrusion = ShaderGUI.FindProperty ("_UseNormalExtrusion", properties);
		Debug.Log (useNormalExtrusion);
		if (useNormalExtrusion != null) {
			materialEditor.ShaderProperty (useNormalExtrusion, useNormalExtrusion.displayName);
			if (useNormalExtrusion.floatValue == 1) {
				MaterialProperty outlineNormalExtrusion = ShaderGUI.FindProperty ("_OutlineNormalExtrusion", properties);
				materialEditor.ShaderProperty (outlineNormalExtrusion, outlineNormalExtrusion.displayName);
			}
		}
	}
}
