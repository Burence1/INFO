package com.Info.InfoApp.services;

import com.Info.InfoApp.auth.token.TokenService;
import com.Info.InfoApp.auth.token.confirmationToken;
import com.Info.InfoApp.models.User;
import com.Info.InfoApp.repositories.UserRepository;
import lombok.AllArgsConstructor;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.UUID;

@Service
@AllArgsConstructor
public class authService implements UserDetailsService {

    private final static String USER_NOT_FOUND_MSG = "no user with %s found";
    private final UserRepository userRepository;
    private final BCryptPasswordEncoder bCryptPasswordEncoder;
    private final TokenService tokenService;

    @Override
    public UserDetails loadUserByUsername(String email)
    throws UsernameNotFoundException {
        return userRepository.findByemail(email)
                .orElseThrow(()->
                        new UsernameNotFoundException(
                                String.format(USER_NOT_FOUND_MSG,email)
                        ));
    }

    public String signUpUser(User user){
        boolean userExists = userRepository.findByemail(user.getEmail()).isPresent();

        if(userExists){
            throw new IllegalStateException("email already registered");
        }

        String encodedPass = bCryptPasswordEncoder.encode(user.getPassword());

        user.setPassword(encodedPass);
        userRepository.save(user);

        String token = UUID.randomUUID().toString();

        confirmationToken _confirmationToken = new confirmationToken(
                token, LocalDateTime.now(),LocalDateTime.now().plusMinutes(15),
                user
        );

        tokenService.SaveConfirmationToken(_confirmationToken);

        return token;
    }

//    public int enableAppUser(String email) {
//        return userRepository.enableUser(email);
//    }
}
