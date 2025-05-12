package com.ctg.capturethegun.repository;

import com.ctg.capturethegun.model.Flag;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface FlagRepository extends JpaRepository<Flag, Integer> {
    List<Flag> findByLatitudeAndLongitude(float latitude, float longitude);
    List<Flag> findByForm3D(String form3D);
}
