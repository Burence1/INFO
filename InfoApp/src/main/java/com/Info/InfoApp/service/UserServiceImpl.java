package com.Info.InfoApp.service;

import com.Info.InfoApp.Entity.User;
import com.Info.InfoApp.Entity.VerificationToken;
import com.Info.InfoApp.model.UserModel;
import com.Info.InfoApp.repository.UserRepository;
import com.Info.InfoApp.repository.VerificationTokenRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;
import java.io.IOException;
import java.util.Calendar;
import java.util.UUID;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

@Service
public class UserServiceImpl implements UserService{

    @Autowired
    private UserRepository userRepository;

    @Autowired
    private BCryptPasswordEncoder passwordEncoder;

    @Autowired
    private VerificationTokenRepository verificationTokenRepository;

    String regex = "^(.+)@(.+)$";
    Pattern pattern = Pattern.compile(regex);

    public boolean validateEmail(String email) throws IOException{
        try{
            Matcher matcher = pattern.matcher(email);
            var match = matcher.matches();
            var checkMail = userRepository.findByEmail(email);
            if(!match)
                throw new IOException("ENTER A VALID EMAIL ADDRESS");
            if(checkMail != null)
                return false;
            return true;
        }
        catch (Exception e){
            return false;
        }
    }

    @Override
    public User registerUser(UserModel userModel) throws IOException {
        User user = new User();
        boolean result = validateEmail(userModel.getEmail());
        if(result != true)
            throw new IOException("EMAIL ADDRESS EXISTS OR IS INVALID");

        user.setEmail(userModel.getEmail());
        user.setFirstName(userModel.getFirstName());
        user.setLastName(userModel.getLastName());
        user.setRole("USER");
        user.setPassword(passwordEncoder.encode(userModel.getPassword()));

        userRepository.save(user);
        return user;
    }

    @Override
    public User findUserByEmail(String email) {
            return userRepository.findByEmail(email);
    }

    @Override
    public void saveVerificationTokenForUser(String token, User user) {
        VerificationToken verificationToken = new VerificationToken(user,token);
        verificationTokenRepository.save(verificationToken);
    }

    @Override
    public String validateVerificationToken(String token) {
        VerificationToken verificationToken =verificationTokenRepository.findByToken(token);
        if(verificationToken == null)
            return "invalid";

        User user = verificationToken.getUser();
        Calendar calender = Calendar.getInstance();

        if((verificationToken.getExpirationTime().getTime()
                - calender.getTime().getTime()) <= 0){
            verificationTokenRepository.delete(verificationToken);
            return "Token Has Expired";
        }

        user.setEnabled(true);
        userRepository.save(user);
        return "valid";
    }

    @Override
    public VerificationToken generateNewVerificationToken(String oldToken) {
        VerificationToken verificationToken = verificationTokenRepository.findByToken(oldToken);

        verificationToken.setToken(UUID.randomUUID().toString());
        verificationTokenRepository.save(verificationToken);
        return verificationToken;
    }
}