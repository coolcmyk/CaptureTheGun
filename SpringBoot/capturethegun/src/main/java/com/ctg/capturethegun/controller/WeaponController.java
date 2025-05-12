package com.ctg.capturethegun.controller;

import com.ctg.capturethegun.model.Weapon;
import com.ctg.capturethegun.repository.WeaponRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/api/weapons")
public class WeaponController {

    @Autowired
    private WeaponRepository weaponRepository;

    @GetMapping
    public List<Weapon> getAllWeapons() {
        return weaponRepository.findAll();
    }

    @GetMapping("/{id}")
    public Optional<Weapon> getWeaponById(@PathVariable int id) {
        return weaponRepository.findById(id);
    }

    @PostMapping
    public Weapon createWeapon(@RequestBody Weapon weapon) {
        return weaponRepository.save(weapon);
    }

    @PutMapping("/{id}")
    public Weapon updateWeapon(@PathVariable int id, @RequestBody Weapon weaponDetails) {
        return weaponRepository.findById(id).map(weapon -> {
            weapon.setName(weaponDetails.getName());
            weapon.setStatus(weaponDetails.getStatus());
            weapon.setPlayer(weaponDetails.getPlayer());
            weapon.setFlag(weaponDetails.getFlag());
            return weaponRepository.save(weapon);
        }).orElseGet(() -> {
            weaponDetails.setWeaponId(id);
            return weaponRepository.save(weaponDetails);
        });
    }

    @DeleteMapping("/{id}")
    public void deleteWeapon(@PathVariable int id) {
        weaponRepository.deleteById(id);
    }
}
