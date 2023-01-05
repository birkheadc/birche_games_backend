# Birche Games Backend

This application serves as the backend for my browser game repository. It hosts static files (the games and their assets) as well as a rest API for populating the home page and uploading / managing games.

## Static Content

Static content is hosted in `wwwroot/static`. /static is needed so the reverse proxy knows to route requests to /api or /static to this application.

When a game is uploaded, that game is given a unique ID in the form of MongoDBs `ObjectId`, which is similar to a standard UUID. A document containing that games meta-data is created in the database. Then, the game's distributable is copied into `wwwroot/static/dists/{id}`, and unzipped there. The cover image for that game, a png used for its thumbnail, is copied into `wwwroot/static/covers/{id}`.

The front end then knows to search for these files in these locations, and can find them with just the game's id.

## Rest API

The API is mostly standard CRUD. I initially had trouble wiring POSTing of files as above. Eventually I made it work by using `[FromForm]` in the controller method, and using `FormData` in React when posting the data.

## Authentication

Authentication is not yet completed, I plan to use a simple filter to check for Authentication header on protected methods, as well as a PasswordService and PasswordRepository to allow for changing and validating of password. There will only be one admin, so only a single entry will be needed.

## Deployment

I deploy using Docker. The following Environment Variables are needed for the application to run:

- `ASPNETCORE_ENVIRONMENT`: Set to `production` when deploying
- `ASPNETCORE_CONNECTION_STRING`: The connection string used by MongoDB
- `ASPNETCORE_DATABASE_NAME`: The name of the database
- `APSNETCORE_GAMES_COLLECTION_NAME`: The name of the collection that Game meta-data will be stored in
- `ASPNETCORE_PASSWORD_COLLECTION_NAME`: The name of the collection where the admin password will be stored
- `ASPNETCORE_ADMIN_PASSWORD`: The default admin password for managing the application and database

The Dockerfile expects these to be passed as arguments, the argument names are the same as the environment variable name, minus the header `ASPNETCORE_`. I recommend using docker-compose and simply listing them in args in docker-compose.yml.