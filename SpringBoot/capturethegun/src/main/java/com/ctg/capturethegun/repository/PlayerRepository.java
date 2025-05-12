package com.ctg.capturethegun.repository;

import com.ctg.capturethegun.model.Player;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface PlayerRepository extends JpaRepository<Player, Integer> {
    List<Player> findByName(String name);
    List<Player> findBySuspicionLevelGreaterThan(int suspicionLevel);
}
