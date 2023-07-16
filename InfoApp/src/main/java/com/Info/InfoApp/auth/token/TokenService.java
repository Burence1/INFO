package com.Info.InfoApp.auth.token;

import lombok.AllArgsConstructor;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.Optional;

@Service
@AllArgsConstructor
public class TokenService {

    private final TokenRepository tokenRepository;

    public void SaveConfirmationToken(confirmationToken token){
        tokenRepository.save(token);
    }

    public Optional<confirmationToken> getToken(String token){
        return tokenRepository.findByToken(token);
    }

//    public int setConfirmedAt(String token) {
//        return tokenRepository.updateConfirmedAt(
//                token, LocalDateTime.now());
//    }
}
