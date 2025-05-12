package com.ctg.capturethegun.controller;

import com.ctg.capturethegun.model.Puzzle;
import com.ctg.capturethegun.repository.PuzzleRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/api/puzzles")
public class PuzzleController {

    @Autowired
    private PuzzleRepository puzzleRepository;

    @GetMapping
    public List<Puzzle> getAllPuzzles() {
        return puzzleRepository.findAll();
    }

    @GetMapping("/{id}")
    public Optional<Puzzle> getPuzzleById(@PathVariable int id) {
        return puzzleRepository.findById(id);
    }

    @PostMapping
    public Puzzle createPuzzle(@RequestBody Puzzle puzzle) {
        return puzzleRepository.save(puzzle);
    }

    @PutMapping("/{id}")
    public Puzzle updatePuzzle(@PathVariable int id, @RequestBody Puzzle puzzleDetails) {
        return puzzleRepository.findById(id).map(puzzle -> {
            puzzle.setType(puzzleDetails.getType());
            puzzle.setSolved(puzzleDetails.isSolved());
            // Set other fields as needed
            return puzzleRepository.save(puzzle);
        }).orElseGet(() -> {
            puzzleDetails.setPuzzleId(id);
            return puzzleRepository.save(puzzleDetails);
        });
    }

    @DeleteMapping("/{id}")
    public void deletePuzzle(@PathVariable int id) {
        puzzleRepository.deleteById(id);
    }
}
