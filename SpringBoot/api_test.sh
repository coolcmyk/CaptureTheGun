#!/bin/bash

API_URL="http://localhost:8080/api"

echo "=== Criminals ==="
curl -X GET $API_URL/criminals; echo
curl -X GET $API_URL/criminals/1; echo
curl -X POST $API_URL/criminals -H "Content-Type: application/json" -d '{"name":"John Doe","armed":true,"room":null}'; echo
curl -X PUT $API_URL/criminals/1 -H "Content-Type: application/json" -d '{"name":"Jane Doe","armed":false,"room":null}'; echo
curl -X DELETE $API_URL/criminals/1; echo

echo "=== Players ==="
curl -X GET $API_URL/players; echo
curl -X GET $API_URL/players/1; echo
curl -X POST $API_URL/players -H "Content-Type: application/json" -d '{"name":"Player1","suspicionLevel":0}'; echo
curl -X PUT $API_URL/players/1 -H "Content-Type: application/json" -d '{"name":"Player2","suspicionLevel":1}'; echo
curl -X DELETE $API_URL/players/1; echo

echo "=== Weapons ==="
curl -X GET $API_URL/weapons; echo
curl -X GET $API_URL/weapons/1; echo
curl -X POST $API_URL/weapons -H "Content-Type: application/json" -d '{"name":"Gun","status":"active","player":null,"flag":null}'; echo
curl -X PUT $API_URL/weapons/1 -H "Content-Type: application/json" -d '{"name":"Knife","status":"inactive","player":null,"flag":null}'; echo
curl -X DELETE $API_URL/weapons/1; echo

echo "=== Tools ==="
curl -X GET $API_URL/tools; echo
curl -X GET $API_URL/tools/1; echo
curl -X POST $API_URL/tools -H "Content-Type: application/json" -d '{"name":"Lockpick","function":"open doors"}'; echo
curl -X PUT $API_URL/tools/1 -H "Content-Type: application/json" -d '{"name":"Crowbar","function":"pry"}'; echo
curl -X DELETE $API_URL/tools/1; echo

echo "=== Flags ==="
curl -X GET $API_URL/flags; echo
curl -X GET $API_URL/flags/1; echo
curl -X POST $API_URL/flags -H "Content-Type: application/json" -d '{"form3D":"cube","latitude":0.0,"longitude":0.0}'; echo
curl -X PUT $API_URL/flags/1 -H "Content-Type: application/json" -d '{"form3D":"sphere","latitude":1.0,"longitude":1.0}'; echo
curl -X DELETE $API_URL/flags/1; echo

echo "=== Rooms ==="
curl -X GET $API_URL/rooms; echo
curl -X GET $API_URL/rooms/1; echo
curl -X POST $API_URL/rooms -H "Content-Type: application/json" -d '{"description":"Main Hall"}'; echo
curl -X PUT $API_URL/rooms/1 -H "Content-Type: application/json" -d '{"description":"Lobby"}'; echo
curl -X DELETE $API_URL/rooms/1; echo

echo "=== Puzzles ==="
curl -X GET $API_URL/puzzles; echo
curl -X GET $API_URL/puzzles/1; echo
curl -X POST $API_URL/puzzles -H "Content-Type: application/json" -d '{"type":"riddle","solved":false}'; echo
curl -X PUT $API_URL/puzzles/1 -H "Content-Type: application/json" -d '{"type":"maze","solved":true}'; echo
curl -X DELETE $API_URL/puzzles/1; echo