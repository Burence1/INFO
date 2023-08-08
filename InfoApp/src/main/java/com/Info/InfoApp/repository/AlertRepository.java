package com.Info.InfoApp.repository;

import com.Info.InfoApp.Entity.Alert;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface AlertRepository extends JpaRepository<Alert, Long> {
    Alert findByAlertNo(String AlertNo);
}
