package com.ctg.capturethegun.model;

import jakarta.persistence.*;
import java.util.List;

@Entity
public class Player {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int playerId;

    private String name;

    private int suspicionLevel;

    @OneToMany(mappedBy = "player")
    private List<Room> rooms;

    @OneToMany(mappedBy = "player")
    private List<Puzzle> puzzles;

    @OneToMany(mappedBy = "player")
    private List<Tool> tools;

    @OneToOne
    private Weapon weapon;

    // Getters and Setters

    public String getName() {
        return name;
    }
    public int getPlayerId() {
        return playerId;
    }
    public int getSuspicionLevel() {
        return suspicionLevel;
    }
    public List<Room> getRooms() {
        return rooms;
    }
    public List<Puzzle> getPuzzles() {
        return puzzles;
    }
    public List<Tool> getTools() {
        return tools;
    }
    public Weapon getWeapon() {
        return weapon;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setPlayerId(int playerId) {
        this.playerId = playerId;
    }
    public void setSuspicionLevel(int suspicionLevel) {
        this.suspicionLevel = suspicionLevel;
    }
    public void setRooms(List<Room> rooms) {
        this.rooms = rooms;
    }
    public void setPuzzles(List<Puzzle> puzzles) {
        this.puzzles = puzzles;
    }
    public void setTools(List<Tool> tools) {
        this.tools = tools;
    }
    public void setWeapon(Weapon weapon) {
        this.weapon = weapon;
    }

    
    @Override
    public String toString() {
        return "Player{" +
                "playerId=" + playerId +
                ", name='" + name + '\'' +
                ", suspicionLevel=" + suspicionLevel +
                ", rooms=" + rooms +
                ", puzzles=" + puzzles +
                ", tools=" + tools +
                ", weapon=" + weapon +
                '}';
    }
}
