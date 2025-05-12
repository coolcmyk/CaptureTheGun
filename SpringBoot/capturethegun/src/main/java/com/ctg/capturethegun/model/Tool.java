package com.ctg.capturethegun.model;

import jakarta.persistence.*;

@Entity
public class Tool {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int toolId;

    private String name;

    private String function;

    @ManyToOne
    private Player player;

    @ManyToOne
    private Puzzle puzzle;

    // Getters and Setters
    public int getToolId() {
        return toolId;
    }
    public String getName() {
        return name;
    }
    public String getFunction() {
        return function;
    }
    public Player getPlayer() {
        return player;
    }
    public Puzzle getPuzzle() {
        return puzzle;
    }

    public void setToolId(int toolId) {
        this.toolId = toolId;
    }
    public void setName(String name) {
        this.name = name;
    }
    public void setFunction(String function) {
        this.function = function;
    }
    public void setPlayer(Player player) {
        this.player = player;
    }
    public void setPuzzle(Puzzle puzzle) {
        this.puzzle = puzzle;
    }
    @Override
    public String toString() {
        return "Tool{" +
                "toolId=" + toolId +
                ", name='" + name + '\'' +
                ", function='" + function + '\'' +
                ", player=" + player +
                ", puzzle=" + puzzle +
                '}';
    }
}
