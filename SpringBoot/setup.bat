@echo off
echo Building Spring Boot JAR...
cd capturethegun
call gradlew.bat build
if errorlevel 1 (
    echo Gradle build failed
    pause
    exit /b 1
)
cd ..

echo Building Docker images and starting containers...
docker-compose up -d --build

echo.
echo Setup complete. App should be running on http://localhost:8080
pause