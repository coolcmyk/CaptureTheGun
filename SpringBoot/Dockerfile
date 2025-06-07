FROM openjdk:22-jdk-slim

WORKDIR /app

# Copy the built JAR file into the image
COPY capturethegun/build/libs/*.jar app.jar

EXPOSE 8080

CMD ["java", "-jar", "app.jar"]