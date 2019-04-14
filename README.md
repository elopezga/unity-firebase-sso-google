# Unity Firebase SSO for Google

This is a sample project on getting Single Sign-On (SSO) with Google on Unity.

### Do It Yourself
#### Create a Firebase project
1. Register as Unity app by clicking the Unity icon in firebase.
1. Download and add the config file (`google-services.json`) inside Assets folder.
1. Download Firebase SDK and import package `FirebaseAuth`.
1. If prompted, enable `Android Auto-Resoluton`.
1. In Unity, change your bundle id to match what you put when you configured the Firebase project.
1. Let Unity do its thang until it stops loading.

#### Import Google Single Sign-On Unity Package
1. Import the Google single sign-on unity package
1. Setup google single sign on in google api
1. If you get Task compiler error, delete the Unity.Tasks.dll in Parse with the 3.5 reference

#### Configure Google Single Sign-On in Firebase
1. Go to auth section
1. Enable Google
1. Generate SHA1 fingerprint and add to project settings.

#### Convert Google Sign in to Firebase Sign in