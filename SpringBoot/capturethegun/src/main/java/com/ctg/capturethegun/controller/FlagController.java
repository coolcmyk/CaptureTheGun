package com.ctg.capturethegun.controller;

import com.ctg.capturethegun.model.Flag;
import com.ctg.capturethegun.repository.FlagRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.Base64;
import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/api/flags")
public class FlagController {

    @Autowired
    private FlagRepository flagRepository;

    @GetMapping
    public List<Flag> getAllFlags() {
        return flagRepository.findAll();
    }

    @GetMapping("/{id}")
    public Optional<Flag> getFlagById(@PathVariable int id) {
        return flagRepository.findById(id);
    }

    @PostMapping
    public Flag createFlag(@RequestBody Flag flag) {
        return flagRepository.save(flag);
    }

    @PutMapping("/{id}")
    public Flag updateFlag(@PathVariable int id, @RequestBody Flag flagDetails) {
        return flagRepository.findById(id).map(flag -> {
            flag.setForm3D(flagDetails.getForm3D());
            flag.setLatitude(flagDetails.getLatitude());
            flag.setLongitude(flagDetails.getLongitude());
            return flagRepository.save(flag);
        }).orElseGet(() -> {
            flagDetails.setFlagId(id);
            return flagRepository.save(flagDetails);
        });
    }

    @DeleteMapping("/{id}")
    public void deleteFlag(@PathVariable int id) {
        flagRepository.deleteById(id);
    }

    @GetMapping("/CLUE0")
    public String getClueZero() {
        String keyword = Base64.getEncoder().encodeToString("apocalypse".getBytes());
        String lat = Base64.getEncoder().encodeToString("30".getBytes());
        return "As the sun sets, a whisper travels through the air: '" + keyword + "' is a word that lingers in the minds of those who listen. Some say the journey begins at a place marked by " + lat + ", but only the curious will understand.";
    }

    @GetMapping("/CLUE1")
    public String getClueOne() {
        String keyword = Base64.getEncoder().encodeToString("is".getBytes());
        String lon = Base64.getEncoder().encodeToString("60".getBytes());
        return "In the heart of the city, a mural reads: '" + keyword + "'. Locals often mention a distant landmark, coordinates hidden in plain sight as " + lon + ". Sometimes, the answer is right before your eyes.";
    }

    @GetMapping("/CLUE2")
    public String getClueTwo() {
        String keyword = Base64.getEncoder().encodeToString("starting".getBytes());
        return "Legends tell of a time when everything was '" + keyword + "' anew. The wise say that to find your way, you must first unravel the meaning behind the words left behind.";
    }
}
