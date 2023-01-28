# Software Design

## Architecture Diagram

Our project will use the N-tier layered architecture format, we plan on using React Native and Unity3D for the user interface of the presentation layer, and the main logic of our app will be contained in Unity3D. Google Maps API will be used in the business layer to assist location detection. We will use Unity3D to build all AR related functionality through the AR Foundation Framework that allows adaptation to both Android and iOS without further changes. Tentatively, we want to use Django ORM as the persistence layer for accessing AR model data, and a PostgreSQL database for storing 3D models that we will use for the AR experience. Currently we plan on hosting the backend database on a Google Cloud server. This architecture diagram may be subjected to change in future sprints. 

![Architecture](../images/architecture.png)


## UML Class Diagram

Our app can be divided into a number of central classes along with some helper ones. The user class makes calls that get the app user up and running like making calls that obtain the user's current location using the Location class (which further makes use of the class GeocoderResult that Google Maps API uses). The Camera class is responsible for displaying AR objects (which are classed as ARObject). ARObjects are heavily reliant on x, y, and z coordinates, which is why having a 3D vector class, Vector3, is convenient.

![UML](../images/uml.svg)

## Sequence Diagram

The sequence diagram features the four most important interactions in our app: login, location recognition, camera view, and information view. From opening the app, users may choose to login. After which, all users must set their location either automatically through a built in maps API, or manually enter their location if the GPS is incorrect. Once location is set, users will be navigated to their camera view that allows them to point their camera at aquatic objects in order to see an AR of said object on screen. Users may view information about objects by clicking on them, where they may choose to navigate further to external Wiki sites through embedded links. Users may choose to save objects to their account database, if logged in. 
![Sequence](../images/sequence.png)