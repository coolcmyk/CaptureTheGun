package com.ctg.capturethegun.model;

import jakarta.persistence.*;

@Entity
public class Flag {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int flagId;

    private String form3D;

    private float latitude;

    private float longitude;

    @ManyToOne
    private Puzzle puzzle;

    @OneToOne
    private Weapon weapon;

    // Getters and Setters
    public int getFlagId() {
        return flagId;
    }
    public String getForm3D() {
        return form3D;
    }
    public float getLatitude() {
        return latitude;
    }
    public float getLongitude() {
        return longitude;
    }


    public void setFlagId(int flagId) {
        this.flagId = flagId;
    }

    public void setForm3D(String form3D) {
        this.form3D = form3D;
    }
    public void setLatitude(float latitude) {
        this.latitude = latitude;
    }
    public void setLongitude(float longitude) {
        this.longitude = longitude;
    }


    @Override
    public String toString() {
        return "Flag{" +
                "flagId=" + flagId +
                ", form3D='" + form3D + '\'' +
                ", latitude=" + latitude +
                ", longitude=" + longitude +
                ", puzzle=" + puzzle +
                ", weapon=" + weapon +
                '}';
    }

}
