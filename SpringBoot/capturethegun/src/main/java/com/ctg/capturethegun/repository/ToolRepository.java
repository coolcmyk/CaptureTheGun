package com.ctg.capturethegun.repository;

import com.ctg.capturethegun.model.Tool;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface ToolRepository extends JpaRepository<Tool, Integer> {
    List<Tool> findByName(String name);
    List<Tool> findByFunction(String function);
}
