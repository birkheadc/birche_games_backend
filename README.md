# Birche Games Backend

This application serves as the backend for my browser game repository. It hosts static files (the games and their assets) as well as a rest API for populating the home page and uploading / managing games.

## Static Content

Static content is hosted in `wwwroot/static`. /static is needed so the reverse proxy knows to route requests to /api or /static to this application.

When a game is uploaded, that game is given a unique ID in the form of MongoDBs `ObjectId`, which is similar to a standard UUID. A document containing that games meta-data is created in the database. Then, the game's distributable is copied into `wwwroot/static/dists/{id}`, and unzipped there. The cover image for that game, a png used for its thumbnail, is copied into `wwwroot/static/covers/{id}`.

The front end then knows to search for these files in these locations, and can find them with just the game's id.

## Rest API

The API is mostly standard CRUD. I initially had trouble wiring POSTing of files as above. Eventually I made it work by using `[FromForm]` in the controller method, and using `FormData` in React when posting the data.

I also originally planned on creating a `help` route on each controller that would return a manual on what that controller was capable of, but I don't think I ever got around to expanding on this idea as I would have liked. I added this paragraph to the readme later in case anyone reading this was curious about the meaning of the classes in `src/Controllers/Docs`.

## Authentication

Authentication uses two steps. First, a session token is requested via an admin password. If the password is correct, a session token is returned to the front-end. The front-end can then use this session token to authenticate further protected requests.

## Deployment

I deploy using Docker. The following Environment Variables are needed for the application to run:

- `ASPNETCORE_ENVIRONMENT`: Set to `production` when deploying
- `ASPNETCORE_CONNECTION_STRING`: The connection string used by MongoDB
- `ASPNETCORE_DATABASE_NAME`: The name of the database
- `ASPNETCORE_GAMES_COLLECTION_NAME`: The name of the collection that Game meta-data will be stored in
- `ASPNETCORE_PASSWORD_COLLECTION_NAME`: The name of the collection where the admin password will be stored
- `ASPNETCORE_ADMIN_PASSWORD`: The default admin password for managing the application and database
- `ASPNETCORE_JWT_KEY`: A random string used to create session tokens, keep secret
- `ASPNETCORE_JWT_ISSUER`: Not sure if this is strictly needed
- `ASPNETCORE_JWT_AUDIENCE`: Same as issuer

The Dockerfile expects these to be passed as arguments, the argument names are the same as the environment variable name, minus the header `ASPNETCORE_`. I recommend using docker-compose and simply listing them in args in docker-compose.yml.
