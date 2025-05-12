package com.ctg.capturethegun.repository;

import com.ctg.capturethegun.model.Player;
import org.springframework.data.jpa.repository.JpaRepository;

public interface PlayerRepository extends JpaRepository<Player, Integer> {
}
