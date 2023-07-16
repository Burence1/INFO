package com.Info.InfoApp.repositories;

import com.Info.InfoApp.models.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigInteger;
import java.util.Optional;

public interface UserRepository extends JpaRepository<User, BigInteger>{

    Optional<User> findByemail(String email);
    //User findbyEmail(String email);
//    @Transactional
//    @Modifying
//    @Query("UPDATE user a " +
//            "SET a.enabled = TRUE WHERE a.email = ?1")
//    int enableUser(String email);
}
