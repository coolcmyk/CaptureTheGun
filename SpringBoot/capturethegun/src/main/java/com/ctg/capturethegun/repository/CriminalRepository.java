package com.ctg.capturethegun.repository;

import com.ctg.capturethegun.model.Criminal;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface CriminalRepository extends JpaRepository<Criminal, Integer> {
    List<Criminal> findAll();
    List<Criminal> findById(int id);
    List<Criminal> saveCriminals(Criminal criminals);
}
