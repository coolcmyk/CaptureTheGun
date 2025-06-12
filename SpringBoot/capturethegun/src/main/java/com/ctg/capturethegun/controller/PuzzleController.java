package com.ctg.capturethegun.controller;

import com.ctg.capturethegun.model.Puzzle;
import com.ctg.capturethegun.repository.PuzzleRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.Base64;
import java.util.List;
import java.util.Map;
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

    
    @PostMapping("/base64/encode")
    public String encodeBase64(@RequestBody Map<String, String> body) {
        String input = body.get("input");
        if (input == null) return "";
        return Base64.getEncoder().encodeToString(input.getBytes(StandardCharsets.UTF_8));
    }

    @PostMapping("/base64/decode")
    public String decodeBase64(@RequestBody Map<String, String> body) {
        String input = body.get("input");
        if (input == null) return "";
        try {
            byte[] decoded = Base64.getDecoder().decode(input);
            return new String(decoded, StandardCharsets.UTF_8);
        } catch (IllegalArgumentException e) {
            return "Invalid base64 input";
        }
    }

    @PostMapping("/latlon/translate")
    public Map<String, Object> translateLatLon(
            @RequestBody Map<String, Object> body) {
        String[] requiredWords = {"apocalypse", "is", "starting"};
        Object wordsObj = body.get("words");
        if (!(wordsObj instanceof List<?> words) || words.size() != 3) {
            return Map.of("success", false, "error", "Three words required");
        }
        for (int i = 0; i < 3; i++) {
            if (!requiredWords[i].equalsIgnoreCase(String.valueOf(words.get(i)))) {
                return Map.of("success", false, "error", "Incorrect words");
            }
        }
        Double lat = null, lon = null;
        try {
            lat = Double.valueOf(String.valueOf(body.get("lat")));
            lon = Double.valueOf(String.valueOf(body.get("lon")));
        } catch (Exception e) {
            return Map.of("success", false, "error", "Invalid lat/lon");
        }
        String translated = "Latitude: " + lat + ", Longitude: " + lon;
        return Map.of("success", true, "result", translated);
    }

    @PutMapping("/{id}")
    public Puzzle updatePuzzle(@PathVariable int id, @RequestBody Puzzle puzzleDetails) {
        return puzzleRepository.findById(id).map(puzzle -> {
            puzzle.setType(puzzleDetails.getType());
            puzzle.setSolved(puzzleDetails.isSolved());
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
