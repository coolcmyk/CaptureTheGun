package com.ctg.capturethegun.repository;

import com.ctg.capturethegun.model.Weapon;
import org.springframework.data.jpa.repository.JpaRepository;

public interface WeaponRepository extends JpaRepository<Weapon, Integer> {
}
