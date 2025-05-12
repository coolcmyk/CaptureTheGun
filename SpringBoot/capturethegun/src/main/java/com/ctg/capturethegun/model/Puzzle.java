package com.ctg.capturethegun.model;

import jakarta.persistence.*;
import java.util.List;

@Entity
public class Puzzle {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int puzzleId;

    private String type;

    private boolean solved;

    @ManyToOne
    private Room room;

    @ManyToOne
    private Player player;

    @OneToMany(mappedBy = "puzzle")
    private List<Flag> flags;

    // Getters and Setters
    public int getPuzzleId() {
        return puzzleId;
    }
    public String getType() {
        return type;
    }
    public boolean isSolved() {
        return solved;
    }
    public Room getRoom() {
        return room;
    }
    public Player getPlayer() {
        return player;
    }
    public List<Flag> getFlags() {
        return flags;
    }

    public void setPuzzleId(int puzzleId) {
        this.puzzleId = puzzleId;
    }
    public void setType(String type) {
        this.type = type;
    }
    public void setSolved(boolean solved) {
        this.solved = solved;
    }
    public void setRoom(Room room) {
        this.room = room;
    }
    public void setPlayer(Player player) {
        this.player = player;
    }
    public void setFlags(List<Flag> flags) {
        this.flags = flags;
    }

    @Override
    public String toString() {
        return "Puzzle{" +
                "puzzleId=" + puzzleId +
                ", type='" + type + '\'' +
                ", solved=" + solved +
                ", room=" + room +
                ", player=" + player +
                ", flags=" + flags +
                '}';
    }
}
