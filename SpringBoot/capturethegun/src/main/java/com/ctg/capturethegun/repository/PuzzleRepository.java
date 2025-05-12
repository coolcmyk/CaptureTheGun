package com.ctg.capturethegun.repository;

import com.ctg.capturethegun.model.Puzzle;
import org.springframework.data.jpa.repository.JpaRepository;

public interface PuzzleRepository extends JpaRepository<Puzzle, Integer> {
}
