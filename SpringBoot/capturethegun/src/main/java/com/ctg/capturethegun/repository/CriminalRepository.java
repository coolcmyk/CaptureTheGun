package com.ctg.capturethegun.repository;

import com.ctg.capturethegun.model.Criminal;
import org.springframework.data.jpa.repository.JpaRepository;

public interface CriminalRepository extends JpaRepository<Criminal, Integer> {
}
