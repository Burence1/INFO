package com.Info.InfoApp.repositories;

import com.Info.InfoApp.models.User;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.Optional;

@Repository
@Transactional(readOnly = true)

public interface UserRepositoryCustom {
    Optional<User> findbyEmail(String email);
}
