package com.ctg.capturethegun.repository;

import com.ctg.capturethegun.model.Room;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface RoomRepository extends JpaRepository<Room, Integer> {
    List<Room> findByDescriptionContaining(String keyword);
}
