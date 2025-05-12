package com.ctg.capturethegun.repository;

import com.ctg.capturethegun.model.Weapon;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface WeaponRepository extends JpaRepository<Weapon, Integer> {
    List<Weapon> findByName(String name);
    List<Weapon> findByStatus(String status);
}
