package com.ctg.capturethegun.model;

import jakarta.persistence.*;

@Entity
public class Weapon {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int weaponId;

    private String name;

    private String status;

    @OneToOne(mappedBy = "weapon")
    private Player player;

    @OneToOne(mappedBy = "weapon")
    private Flag flag;

    // Getters and Setters
    public int getWeaponId() {
        return weaponId;
    }
    public String getName() {
        return name;
    }
    public String getStatus() {
        return status;
    }
    public Player getPlayer() {
        return player;
    }
    public Flag getFlag() {
        return flag;
    }

    public void setWeaponId(int weaponId) {
        this.weaponId = weaponId;
    }
    public void setName(String name) {
        this.name = name;
    }
    public void setStatus(String status) {
        this.status = status;
    }
    public void setPlayer(Player player) {
        this.player = player;
    }
    public void setFlag(Flag flag) {
        this.flag = flag;
    }

    @Override
    public String toString() {
        return "Weapon{" +
                "weaponId=" + weaponId +
                ", name='" + name + '\'' +
                ", status='" + status + '\'' +
                ", player=" + player +
                ", flag=" + flag +
                '}';
    }
}
