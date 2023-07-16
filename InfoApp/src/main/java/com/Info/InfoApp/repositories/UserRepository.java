package com.Info.InfoApp.repositories;

import com.Info.InfoApp.Entity.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigInteger;
import java.util.Optional;

@Repository
@Transactional(readOnly = true)
public interface UserRepository extends JpaRepository<User, BigInteger>{

    Optional<User> findByemail(String email);
//    @Transactional
//    @Modifying
//    @Query("UPDATE appuser a " +
//            "SET a.enabled = true WHERE a.email = ?1")
//    int enableAppUser(String email);
}
