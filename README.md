# ColyseusNavmeshExample
 
## Server

Using [Colyseus](https://github.com/colyseus/colyseus) as authorative Game Server.

Using the npm [navmesh](https://github.com/mikewesthad/navmesh) library for pathfinding.

The navmesh which is the base for the pathfinding calculation is generated from Unity.

## Client

Using the [Colyseus Unity3d](https://github.com/colyseus/colyseus-unity3d) SDK for connecting to the server.

Using Unity3d for display the Game World.


## Docker

`cd server`

`docker build -t wenish/colyseus-server .`

`docker run -p 3000:3000 -d wenish/colyseus-server`