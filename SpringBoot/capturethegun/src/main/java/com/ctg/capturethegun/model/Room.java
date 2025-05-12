package com.ctg.capturethegun.model;

import jakarta.persistence.*;
import java.util.List;

@Entity
public class Room {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int roomId;

    private String description;

    @ManyToOne
    private Player player;

    @OneToMany(mappedBy = "room")
    private List<Puzzle> puzzles;

    // Getters and Setters
    public int getRoomId() {
        return roomId;
    }
    public String getDescription() {
        return description;
    }
    public Player getPlayer() {
        return player;
    }
    public List<Puzzle> getPuzzles() {
        return puzzles;
    }

    public void setRoomId(int roomId) {
        this.roomId = roomId;
    }
    public void setDescription(String description) {
        this.description = description;
    }
    public void setPlayer(Player player) {
        this.player = player;
    }
    public void setPuzzles(List<Puzzle> puzzles) {
        this.puzzles = puzzles;
    }

    @Override
    public String toString() {
        return "Room{" +
                "roomId=" + roomId +
                ", description='" + description + '\'' +
                ", player=" + player +
                ", puzzles=" + puzzles +
                '}';
    }
}
