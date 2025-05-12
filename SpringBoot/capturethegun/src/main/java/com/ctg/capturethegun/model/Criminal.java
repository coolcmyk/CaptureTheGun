package com.ctg.capturethegun.model;

import jakarta.persistence.*;

@Entity
public class Criminal {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int criminalId;

    private String name;

    private boolean armed;

    @ManyToOne
    private Room room;

    // Getters and Setters
    public int getCriminalId() {
        return criminalId;
    }
    public String getName() {
        return name;
    }
    public boolean isArmed() {
        return armed;
    }
    public Room getRoom() {
        return room;
    }


    public void setCriminalId(int criminalId) {
        this.criminalId = criminalId;
    }
    public void setName(String name) {
        this.name = name;
    }
    public void setArmed(boolean armed) {
        this.armed = armed;
    }
    public void setRoom(Room room) {
        this.room = room;
    }

    @Override
    public String toString() {
        return "Criminal{" +
                "criminalId=" + criminalId +
                ", name='" + name + '\'' +
                ", armed=" + armed +
                ", room=" + room +
                '}';
    }
}
