package com.ctg.capturethegun.controller;

import com.ctg.capturethegun.model.Tool;
import com.ctg.capturethegun.repository.ToolRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/api/tools")
public class ToolController {

    @Autowired
    private ToolRepository toolRepository;

    @GetMapping
    public List<Tool> getAllTools() {
        return toolRepository.findAll();
    }

    @GetMapping("/{id}")
    public Optional<Tool> getToolById(@PathVariable int id) {
        return toolRepository.findById(id);
    }

    @PostMapping
    public Tool createTool(@RequestBody Tool tool) {
        return toolRepository.save(tool);
    }

    @PutMapping("/{id}")
    public Tool updateTool(@PathVariable int id, @RequestBody Tool toolDetails) {
        return toolRepository.findById(id).map(tool -> {
            tool.setName(toolDetails.getName());
            tool.setFunction(toolDetails.getFunction());
            // Set other fields as needed
            return toolRepository.save(tool);
        }).orElseGet(() -> {
            toolDetails.setToolId(id);
            return toolRepository.save(toolDetails);
        });
    }

    @DeleteMapping("/{id}")
    public void deleteTool(@PathVariable int id) {
        toolRepository.deleteById(id);
    }
}
