package com.ctg.capturethegun.controller;

import com.ctg.capturethegun.model.Room;
import com.ctg.capturethegun.repository.RoomRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/api/rooms")
public class RoomController {

    @Autowired
    private RoomRepository roomRepository;

    @GetMapping
    public List<Room> getAllRooms() {
        return roomRepository.findAll();
    }

    @GetMapping("/{id}")
    public Optional<Room> getRoomById(@PathVariable int id) {
        return roomRepository.findById(id);
    }

    @PostMapping
    public Room createRoom(@RequestBody Room room) {
        return roomRepository.save(room);
    }

    @PutMapping("/{id}")
    public Room updateRoom(@PathVariable int id, @RequestBody Room roomDetails) {
        return roomRepository.findById(id).map(room -> {
            room.setDescription(roomDetails.getDescription());
            // Set other fields as needed
            return roomRepository.save(room);
        }).orElseGet(() -> {
            roomDetails.setRoomId(id);
            return roomRepository.save(roomDetails);
        });
    }

    @DeleteMapping("/{id}")
    public void deleteRoom(@PathVariable int id) {
        roomRepository.deleteById(id);
    }
}
