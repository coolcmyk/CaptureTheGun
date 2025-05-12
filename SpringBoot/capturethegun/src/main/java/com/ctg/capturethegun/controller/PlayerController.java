package com.ctg.capturethegun.controller;

import com.ctg.capturethegun.model.Player;
import com.ctg.capturethegun.repository.PlayerRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/api/players")
public class PlayerController {

    @Autowired
    private PlayerRepository playerRepository;

    @GetMapping
    public List<Player> getAllPlayers() {
        return playerRepository.findAll();
    }

    @GetMapping("/{id}")
    public Optional<Player> getPlayerById(@PathVariable int id) {
        return playerRepository.findById(id);
    }

    @PostMapping
    public Player createPlayer(@RequestBody Player player) {
        return playerRepository.save(player);
    }

    @PutMapping("/{id}")
    public Player updatePlayer(@PathVariable int id, @RequestBody Player playerDetails) {
        return playerRepository.findById(id).map(player -> {
            player.setName(playerDetails.getName());
            player.setSuspicionLevel(playerDetails.getSuspicionLevel());
            // Set other fields as needed
            return playerRepository.save(player);
        }).orElseGet(() -> {
            playerDetails.setPlayerId(id);
            return playerRepository.save(playerDetails);
        });
    }

    @DeleteMapping("/{id}")
    public void deletePlayer(@PathVariable int id) {
        playerRepository.deleteById(id);
    }
}
