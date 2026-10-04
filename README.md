# Playlist API

A backend API built with ASP.NET Core, EF Core, and SQL Server for managing music playlists. Users create playlists and add songs, while admins manage the song catalogue. Secured with JWT authentication and role-based authorisation.

## Features

- RESTful API endpoints for Auth, Songs, Playlists, and Songs in Playlists
- JWT authentication with User and Admin roles
- Ownership checks, so users can only view and edit their own playlists
- Input validation with clear error responses
- Entity Framework Core with SQL Server, schema managed via migrations
- 22 automated API tests in Postman

## Tech Stack

- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- SQL Server Express / LocalDB
- JWT Bearer authentication
- Postman

## Getting Started

### Prerequisites

- .NET SDK 8.0+
- SQL Server Express or LocalDB
- Postman (for running the tests)

### Setup

1. Clone the repo:
```
   git clone https://github.com/chrs1x/PlaylistApi.git
   cd PlaylistApi
```
2. Set a JWT signing key (32+ chars). It's stored using user-secrets, so it never goes in the repo:
```
   dotnet user-secrets init
   dotnet user-secrets set "JwtSettings:Key" "<random-32+-character-key>"
```
   The app won't start without it.
3. Configure the database connection string in `appsettings.json` (LocalDB by default).
4. Apply the migrations:
```
   dotnet ef database update
```
5. Launch the API:
```
   dotnet run --launch-profile http
```
   The API runs at `http://localhost:5155`.

### Creating an Admin

New users get the **User** role. To make an admin, register a user, then run:
```sql
UPDATE Users SET Role = 'Admin' WHERE Username = 'admin';
```
Log in again afterwards, so the new token includes the Admin role.

## Project Structure

/Controllers - API endpoints

/DTOs - Request and response models

/Models - Domain models

/Services - Business logic 

/Data - EF Core DbContext

/Migrations - EF Core migrations

/Utils - JWT token generation

/Postman - Postman test collection 

/Properties - Project settings

Program.cs, appsettings.json

## API Endpoints

Send the token from register/login as `Authorization: Bearer <token>`.

### Auth
POST `/api/auth/register` - Register a new user and get a token

POST `/api/auth/login` - Log in and get a token

### Songs
GET `/api/songs` - Get all songs 

GET `/api/songs/{id}` - Get song by ID 

POST `/api/songs` - Add a song to the catalogue (admin)

PATCH `/api/songs/{id}` - Update a song (admin)

DELETE `/api/songs/{id}` - Delete a song (admin)

### Playlists
GET `/api/playlists` - Get all playlists (admin)

GET `/api/playlists/user` - Get user's own playlists

GET `/api/playlists/{id}` - Get playlist by ID (owner or admin)

POST `/api/playlists` - Create a new playlist

PATCH `/api/playlists/{id}` - Rename a playlist (owner or admin)

DELETE `/api/playlists/{id}` - Delete a playlist (owner or admin)

### Songs in Playlists
GET `/api/playlists/{playlistId}/songs` - Get all songs in a playlist (owner or admin)

POST `/api/playlists/{playlistId}/songs/{songId}` - Add a song to a playlist (owner or admin)

DELETE `/api/playlists/{playlistId}/songs/{songId}` - Remove a song from a playlist (owner or admin)

## Testing

The `/Postman` folder has 22 automated tests. Each one checks the API returns the right status code: 401 when not logged in, 403 for the wrong role or someone else's playlist, 400 for invalid input, 404 when something doesn't exist, and 409 for duplicate songs.

<img width="1069" height="953" alt="tests" src="https://github.com/user-attachments/assets/3104aa7c-997c-40b0-a3d4-62f12520690c" />

To run them:
1. Create an admin account 
2. Import `Postman/PlaylistApi.postman_collection.json` into Postman
3. In the collection's Variables tab, set `adminUsername` and `adminPassword`
4. Click **Run** and run all requests in order

Each run registers its own users, so previous runs won't affect results.

## Learning Goals

- Learn how to use JWT authentication and role-based authorisation
- Learn how to restrict users to their own data
- Learn how to model many-to-many relationships and join tables
- Learn how to keep secrets (e.g. JWT) out of source control
- Learn how to write automated API tests that cover both success and error cases
