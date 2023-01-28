# Project Requirements

## Executive summary
The Aquatic Biosphere Project of Canada engages individuals of all ages and backgrounds with Canada’s aquatic biosphere through presentations and media, community programs, and an upcoming public attraction. To that end, they hope to develop an Android/iOS application that uses augmented reality (AR) to enhance the device’s camera with fun and informative content about Canada’s nature. At the moment, the application is not funded. Our product will show potential investors that such an application is possible, hopefully securing funding for the Aquatic Biosphere Project to further develop the application.

When a user first opens the product, they will confirm their location in order to ensure the accuracy of the displayed information. The product will then open the device camera, and the user will be free to use it to scan their environment. When the user brings a relevant object into the camera view, the product will augment the object with the appropriate AR objects. Note that the product will only be tailored to environments within the University of Alberta North Campus.

## Project Glossary
- **augment** - add additional virtual objects into the camera view so that the objects look as if they are there in 3d space
- **camera view** - the live camera feed of a device that is made visible on the device’s screen
- **device** - a handheld computing device with an integrated camera and a screen, running the android/ios operating system
- **application** - software running on a device; in the context of the summary, it refers mostly to the proposed but undeveloped future application
- **product** - the proof-of-concept application currently being developed
- **AR object** - augmented-reality object; fun or educational information, which may come in the form of 3d models, lines of text, images, among other mediums
- **environment** - a set location with visible objects that remain fairly consistent; this will be used to consistently test different camera views
- **surface** - flat plane in the real world on which the AR objects are augmented on. It can be determined either by recognition or by the user; e.g. pond, marsh, park


## User Stories

| Index       | User Story                                                                                                                       | Story Point | Acceptance test                                                                                                                                                                                    | MoSCoW                    |
|-------------|----------------------------------------------------------------------------------------------------------------------------------|-------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------|
| US 01.01    | As a User, I want the system to identify my location, so that I can share it with the app.                                       | 3           | <li> Location of user is identified. </li>                                                                                                                                                         | Must                      |
| US 01.02    | As a user, I want to be able to enter my location manually or change it, if it is incorrect.                                     | 3           | <li> Location information is changed to thew new manually selected one. </li>                                                                                                                      | Must                      |
| US 01.03    | As a user, I want to be able to select a surface that I’m pointing at from a drop down menu, to optimize the AR experience       | 2           | <li> Surface is correctly selected along with its information. </li>                                                                                                                               | Must                      |
| US 01.04    | As a User, I want the app to access my camera, so that it can display Augmented objects.                                         | 2           | <li> Camera is opened.</li>                                                                                                                                                                        | Must                      |
| US 01.05    | As an admin, I want to be able to add more 3D models, to increase user engagement.                                               | 2           | <li> New models and their info are added to the DB. </li>                                                                                                                                          | Must                      |
| US 02.01    | As a user, I want to be able to view augmented objects on my screen, so that I can interact with them.                           | 5           | <li> AR Objects are displayed correctly on the screen.                                                                                                                                             | Must                      |
| US 02.02    | As a user, I want to be able to view a brief description about the ecosystem I’m looking at, so that I know what to expect.      | 3           | <li> All information about the ecosystem is displayed in a new activity. </li>                                                                                                                     | Should                    |
| US 02.03.01 | As a user, I want to be able to click on an augmented object, so that I can learn information about it.                          | 3           | <li> User is viewing an augmented object through their camera </li>                                                                                                                                | Must                      |
| US 02.03.02 | As a user, I want to be able to navigate to external links for each object, so that I can gather more information.               | 3           | <li>From the popup information screen, user can click on a link to go to an external site where they learn more about the object </li>                                                             | Should                    |
| US 02.03.03 | As a user, I want to be able to exit out of external information tabs, so that I can view other Augmented objects.               | 3           | <li>From the external information screen, user can click on exit/back button and be back to the camera screen </li>                                                                                | Should                    |
| US 03.01    | As a user, I want to create an account, so I can save my progress and interaction information on the app                         | 5           | <li>User can select "Forgot Password" on the login screen.</li> <li> User can enter their email and receive a reset link. </li> <li> User can log in with their new password</li>                  | Could                     |
| US 03.02    | As a user, I want to login to my account, to get a personalized experience.                                                      | 5           | <li>User opens the app and can see a login screen</li> <li> User can enter their credentials and log in to their account</li> <li> User can choose "Forgot Password" to recover their account</li> | Could                     |
| US 03.03    | As a user, I want to be able to save object information in my records, so that I can read about it later.                        | 5           | <li>User can select an object</li> <li> User can choose to save selected object</li> <li> User can see a list of saved objects</li>                                                                | Could                     |
| US 04.01    | As a user, I want to be able to identify a species (eg. A real life plant) through the app, so that I can recognize new species. | 8           | <li> User can point camera at a species</li> <li> User can see information on possible matches for that species </li>                                                                              | Would like, but won't get |
| US 04.02    | As a user, I want to be able to share my findings with others, so that I can help others find those objects/species.             | 8           | <li> User can press share and select other users</li> <li> Other users can receive and see the object and its information </li>                                                                    | Would like but won't get  |

## Similar Products
* [ARCore Elements](https://play.google.com/store/apps/details?id=com.google.ar.unity.ddelements): 
  * Demo app by Google using ARCore platform. 
  * Has tips for User Movement, User Interface and Object Movement.


## Open Source Products
* https://github.com/viromedia/figment-ar: AR app built with [ViroReact](https://github.com/viromedia)
* https://github.com/Unity-Technologies/arfoundation-samples/: AR app demo built with [Unity's AR Foundation](https://unity.com/)


## Technical Resources
* AR Engine: [Unity](https://unity.com/)
  * [Unity as a library in other applications](https://docs.unity3d.com/2019.3/Documentation/Manual/UnityasaLibrary.html)
  * Platform: [AR Foundation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.0/manual/index.html)
    Created by Unity, has support for ARCore by Google and ARKit by Apple.
  * [Unity Mars](https://unity.com/products/unity-mars): Integrates with AR Foundation to build apps 
  * [Google's guide on getting started with AR Foundation](ttps://developers.google.com/ar/develop/unity-arf/getting-started-ar-foundation)
  * [Create an AR game using Unity's AR Foundation](https://codelabs.developers.google.com/arcore-unity-ar-foundation#0)
* [ViroReact](https://github.com/viromedia): Integrate directly with React Native. No longer in development
* Other resources: TBD







