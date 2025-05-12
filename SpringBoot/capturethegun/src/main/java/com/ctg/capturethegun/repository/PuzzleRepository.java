package com.ctg.capturethegun.repository;

import com.ctg.capturethegun.model.Puzzle;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface PuzzleRepository extends JpaRepository<Puzzle, Integer> {
    List<Puzzle> findByType(String type);
    List<Puzzle> findBySolved(boolean solved);
}
