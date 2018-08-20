# First Person Unity Camera

## Overview
This game asset is a first person camera controller that has been created using a rigidBody component to welcome physics interactions more easily compared to the character controller most first person cameras make use of. A prefab of the camera controller is available and you can very easily implement the camera to your own scene. Last but not least, the scripts used, are very clean and robust and can be very easily modified to your own needs.

## Technical Analysis
The first person camera prefab makes use of two gameObjects. The first person body object (parent) and a child camera object used as the main game camera. Let's dive into these Unity gameobjects.

#### FPController
It is an empty gameobject with a capsule collider and a rigidBody component. Moreover the _FPMovementController.cs_ script is attached that controls all the player's movements, from jumping to running and walking. Also an audiosource is attached that is being used for the footsteps and landing-jumping sounds.
In the script attached there are many values that you can modify so to find the style of movement that you seek for your player.

#### Main Camera
It is a camera object (tagged "main camera") that uses two scripts to achieve the desired behaviour of a first person camera.
1. _CameraController.cs_ : Is used for the rotation of the camera in the 3d space. With the camera also the rigidBody of the parent object is rotated.
2. _CameraHeadBod.cs_ : Handles the movement of the camera when the player is walking as well as when landing in the ground. More specifically the camera is moving up and down based on a specific period. The bobbing of the camera can be modified through the inspector. Note that running and walking have different values concerning the bobbing of the camera.

#### Extras
Inside the assets folder you can also find a folder named _firstPersonMechanics_. In this folder mechanics such as raycasting, object interaction and general first person mechanincs will be added regularly. Currently a camera raycaster is available to help you interact with different objects.

## Demo
You can try open the demo scene directly and check the first person camera through a level that will test all of the camera's features. Also, to use the camera in your scene , just use the prefab from the respective folder.

![fpcamera](https://user-images.githubusercontent.com/15057375/43147725-5aaebe48-8f6c-11e8-8213-bf61708ec883.gif)


