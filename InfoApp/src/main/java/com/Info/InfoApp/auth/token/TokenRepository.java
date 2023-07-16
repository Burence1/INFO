package com.Info.InfoApp.auth.token;

import org.springframework.transaction.annotation.Transactional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.math.BigInteger;
import java.time.LocalDateTime;
import java.util.Optional;

@Repository
@Transactional(readOnly = true)
public interface TokenRepository extends JpaRepository<confirmationToken, BigInteger> {

    Optional<confirmationToken> findByToken(String token);

//    @Transactional
//    @Modifying
//    @Query("UPDATE comfirmation_token c " +
//            "SET c.confirmedAt = ?1 " +
//            "WHERE c.token = ?0")
//    int updateConfirmedAt(String token,
//                          LocalDateTime confirmedAt);
}
