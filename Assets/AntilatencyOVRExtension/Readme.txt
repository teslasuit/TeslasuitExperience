./Samples/AltOVRSample.unity scene contains a simple example how to use AltSDK with Oculus unity tools version 1.23.0

You can use AltOVR.prefab, that duplicates OVRCameraRig.prefab behavior and adds all components required by AltSDK or you can make your own setup following this rules:

1. AltOculusHMDTracking component must be placed on OVRCameraRig prefab or other gameobject hierarchy that extends OVRCameraRig prefab.

2. AltEnvironment component must be placed on one of parent gameobjects for AltOculusHMDTracking. If you want to move or teleport player, you have to apply this transformations to AltEnvironment gameobject or one of its parents, otherwise the game zone borders and bars will be drawn at incorrect position. 

3. If you plan to deploy your application to Android device, there must be AltAndroid component somewhere in you scene. If you have several scenes in you project, you can place AltAndroid only in first loading scene.