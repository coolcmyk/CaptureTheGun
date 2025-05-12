package com.ctg.capturethegun.controller;

import com.ctg.capturethegun.model.Criminal;
import com.ctg.capturethegun.repository.CriminalRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/api/criminals")
public class CriminalController {

    @Autowired
    private CriminalRepository criminalRepository;

    @GetMapping
    public List<Criminal> getAllCriminals() {
        return criminalRepository.findAll();
    }

    @GetMapping("/{id}")
    public Optional<Criminal> getCriminalById(@PathVariable int id) {
        return criminalRepository.findById(id);
    }

    @PostMapping
    public Criminal createCriminal(@RequestBody Criminal criminal) {
        return criminalRepository.saveCriminals(criminal);
    }

    @PutMapping("/{id}")
    public Criminal updateCriminal(@PathVariable int id, @RequestBody Criminal criminalDetails) {
        return criminalRepository.findById(id).map(criminal -> {
            criminal.setName(criminalDetails.getName());
            criminal.setArmed(criminalDetails.isArmed());
            // Set other fields as needed
            return criminalRepository.save(criminal);
        }).orElseGet(() -> {
            criminalDetails.setCriminalId(id);
            return criminalRepository.save(criminalDetails);
        });
    }

    @DeleteMapping("/{id}")
    public void deleteCriminal(@PathVariable int id) {
        criminalRepository.deleteById(id);
    }
}
