# Real-time collaborative  editor

[![Build](https://ci.appveyor.com/api/projects/status/5gde9vm8u9t2lnu5?svg=true)](https://ci.appveyor.com/project/dtretyakov/web-text-editor/)

## Overview

Web Text Editor is an application for collaborative text editing. It built on top of ASP.NET framework and uses LSEQ CRDT for resolving conflicting changes between contributors.

## Technologies

### Server-side

Application server powered by ASP.NET and uses following framework parts:

* MVC - for static content delivery.
* Web API - for RESTful data endpoints.
* SignalR - for real-time communication.

### Client-side

Client-side implemented as a Single Page Application (SPA) by leveraging AngularJS framework.

To provide responsible front-end user interface used Twitter Bootstrap framework.

## Data replication

To provide collaborative real-time editing application utilizes Conflict-free Replicated Data Types (CRDTs) which belongs to optimistic replication.

In particular used LSEQ algorithm as an improvement of Logoot algorithm. Both of them use operation-based data replication.
