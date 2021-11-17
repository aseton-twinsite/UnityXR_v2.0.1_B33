﻿// Copyright © 2015-2021 Pico Technology Co., Ltd. All Rights Reserved.

using UnityEditor;
using UnityEngine;
using Unity.XR.PXR;


namespace Unity.XR.PXR.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PXR_OverLay))]
    public class PXR_OverLayEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var guiContent = new GUIContent();
            foreach (PXR_OverLay overlayTarget in targets)
            {
                EditorGUILayout.LabelField("Overlay Display Order", EditorStyles.boldLabel);
                guiContent.text = "Overlay Type";
                guiContent.tooltip = "Whether this overlay should layer behind the scene or in front of it.";
                overlayTarget.overlayType = (PXR_OverLay.OverlayType)EditorGUILayout.EnumPopup(guiContent, overlayTarget.overlayType);
                guiContent.text = "Layer Depth";
                guiContent.tooltip = "Depth value used to sort overlays in the scene, smaller value appears in front.";
                overlayTarget.layerDepth = EditorGUILayout.IntField(guiContent, overlayTarget.layerDepth);

                EditorGUILayout.Separator();
                guiContent.text = "Overlay Shape";
                guiContent.tooltip = "The shape of this overlay.";
                EditorGUILayout.LabelField(guiContent, EditorStyles.boldLabel);
                overlayTarget.overlayShape = (PXR_OverLay.OverlayShape)EditorGUILayout.EnumPopup(guiContent, overlayTarget.overlayShape);

                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Overlay Textures", EditorStyles.boldLabel);
                guiContent.text = "External Surface";
                guiContent.tooltip = "On Android, retrieve an Android Surface object to render to (e.g., video playback).";
                overlayTarget.isExternalAndroidSurface = EditorGUILayout.Toggle(guiContent, overlayTarget.isExternalAndroidSurface);
                var labelControlRect = EditorGUILayout.GetControlRect();
                EditorGUI.LabelField(new Rect(labelControlRect.x, labelControlRect.y, labelControlRect.width / 2, labelControlRect.height), new GUIContent("Left Texture", "Texture used for the left eye"));
                EditorGUI.LabelField(new Rect(labelControlRect.x + labelControlRect.width / 2, labelControlRect.y, labelControlRect.width / 2, labelControlRect.height), new GUIContent("Right Texture", "Texture used for the right eye"));

                var textureControlRect = EditorGUILayout.GetControlRect(GUILayout.Height(64));
                overlayTarget.layerTextures[0] = (Texture)EditorGUI.ObjectField(new Rect(textureControlRect.x, textureControlRect.y, 64, textureControlRect.height), overlayTarget.layerTextures[0], typeof(Texture), false);
                overlayTarget.layerTextures[1] = (Texture)EditorGUI.ObjectField(new Rect(textureControlRect.x + textureControlRect.width / 2, textureControlRect.y, 64, textureControlRect.height), overlayTarget.layerTextures[1] != null ? overlayTarget.layerTextures[1] : overlayTarget.layerTextures[0], typeof(Texture), false);

                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Color Scale And Offset", EditorStyles.boldLabel);
                guiContent.text = "Override Color Scale";
                guiContent.tooltip = "Manually set color scale and offset of this layer.";
                overlayTarget.overrideColorScaleAndOffset = EditorGUILayout.Toggle(guiContent, overlayTarget.overrideColorScaleAndOffset);
                if (overlayTarget.overrideColorScaleAndOffset)
                {
                    guiContent.text = "Color Scale";
                    guiContent.tooltip = "Scale that the color values for this overlay will be multiplied by.";
                    Vector4 colorScale = EditorGUILayout.Vector4Field(guiContent, overlayTarget.colorScale);

                    guiContent.text = "Color Offset";
                    guiContent.tooltip = "Offset that the color values for this overlay will be added to.";
                    Vector4 colorOffset = EditorGUILayout.Vector4Field(guiContent, overlayTarget.colorOffset);
                    overlayTarget.SetLayerColorScaleAndOffset(colorScale, colorOffset);
                }
            }

            if (GUI.changed)
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            }
        }
    }
}

