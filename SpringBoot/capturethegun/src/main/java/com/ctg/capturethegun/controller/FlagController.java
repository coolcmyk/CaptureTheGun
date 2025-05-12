package com.ctg.capturethegun.controller;

import com.ctg.capturethegun.model.Flag;
import com.ctg.capturethegun.repository.FlagRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

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
            // Set other fields as needed
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
}
