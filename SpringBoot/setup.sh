#!/bin/bash

echo "Building Spring Boot JAR..."
cd capturethegun
./gradlew build || { echo "Gradle build failed"; exit 1; }
cd ..

echo "Building Docker images and starting containers..."
docker-compose up -d --build

echo "Setup complete. App should be running on http://localhost:8080"